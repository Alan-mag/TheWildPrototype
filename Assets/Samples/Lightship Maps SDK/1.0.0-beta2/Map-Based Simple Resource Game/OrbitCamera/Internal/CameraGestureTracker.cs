// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    /// <summary>
    /// Camera Controller Gesture Tracker for Niantic-standard map camera interactions
    /// </summary>
    public class CameraGestureTracker : IScreenInputGesture
    {
        public event Action<float> Zoomed;
        public event Action<float> Rotated;
        public event Action<float> ManuallyRotated;

        private const float GROUND_CLAMP_COS_THRESHOLD = -0.1f;
        private static readonly Plane GroundPlane = new Plane(Vector3.up, 0.0f);

        private readonly Camera _raycastCamera;
        private readonly GameObject _focusObject;
        private float _zoomFraction;

        private Vector3 _lastSwipePosition;
        private int _lastSwipeFrame;

        private Vector2 _scrollDelta = Vector2.zero;

        // potential first input for the doubletap
        private InputEvent _firstZoomTap;
        private bool _firstTapEnded = false;

        // 2nd input, only not null during a double-tap-to-zoom
        private InputEvent _secondZoomTap;

        private readonly List<InputEvent> _transformationEvents = new List<InputEvent>();

        private bool _isCurrentlyZooming;

        private bool _wasRotatingLastFrame;
        private Vector3 _lastTouch0Position;
        private Vector3 _lastTouch1Position;
        private Vector3 _prevZoomPosition;
        private InputEvent _lastTouch0;
        private InputEvent _lastTouch1;

        private Settings _settings;

        public CameraGestureTracker(Camera raycastCamera, GameObject focusObject, Settings settings)
        {
            _raycastCamera = raycastCamera;
            _focusObject = focusObject;
            _settings = settings;
            _zoomFraction = settings.DefaultZoom;

            GesturesEnabled = true;
        }

        public bool GesturesEnabled { get; set; }

        /// <summary>
        /// Normalized fraction (0.0 - 1.0) of where we are between fully zoomed in
        /// and fully zoomed out.
        /// </summary>
        public float ZoomFraction
        {
            get { return _zoomFraction; }
            set
            {
                _zoomFraction = value;
                if (Zoomed != null)
                {
                    Zoomed(_zoomFraction);
                }
            }
        }

        private float rotationAngleDegrees;

        public float RotationAngleDegrees
        {
            get { return rotationAngleDegrees; }
            set
            {
                rotationAngleDegrees = value;
                if (Rotated != null)
                {
                    Rotated(rotationAngleDegrees);
                }
            }
        }

        public Camera GetRaycastCamera()
        {
            return _raycastCamera;
        }

        private Vector3 ClampDirToGround(Vector3 dir)
        {
            if (dir.y > -Mathf.Epsilon)
            {
                dir.y = GROUND_CLAMP_COS_THRESHOLD;
            }

            return dir;
        }

        private bool IsCurrentlyRotating
        {
            get
            {
                return GesturesEnabled
                    && !_isCurrentlyZooming
                    &&
                    // Expects only one finger down
                    _transformationEvents.Count == 1
                    && (
                        _transformationEvents[0].Phase == InputPhase.Began
                        || _transformationEvents[0].Phase == InputPhase.Held
                    );
            }
        }

        private void MouseZoom()
        {
            if (!GesturesEnabled)
            {
                return;
            }

            // don't zoom the camera if the scroll is happening over the UI
            if (_transformationEvents.Any(e => TransformOverUI(e.Transform.Value)))
            {
                return;
            }

            ZoomFraction = Mathf.Clamp01(
                ZoomFraction
                    + (Time.unscaledDeltaTime * _settings.MouseScrollZoomSpeed * _scrollDelta.y)
            );
        }

        // returns true if currently zooming
        private bool TouchZoomAndRotate()
        {
            if (!GesturesEnabled)
            {
                return false;
            }

            InputEvent touch0 = null;
            InputEvent touch1 = null;

            uint touch0Id = (_lastTouch0 != null) ? _lastTouch0.Transform.Value.Id : 0xfffffff;
            uint touch1Id = (_lastTouch1 != null) ? _lastTouch1.Transform.Value.Id : 0xfffffff;

            foreach (var touch in _transformationEvents)
            {
                var possibleTouchId = touch.Transform.Value.Id;
                if (touch.Phase == InputPhase.Began)
                {
                    if (_lastTouch0 == null)
                    {
                        _lastTouch0 = touch;
                        touch0Id = possibleTouchId;
                        _lastTouch0Position = _lastTouch0.Transform.Value.Position;
                        touch0 = _lastTouch0;
                    }
                    else if (_lastTouch1 == null)
                    {
                        _lastTouch1 = touch;
                        touch1Id = possibleTouchId;
                        _lastTouch1Position = _lastTouch1.Transform.Value.Position;
                        touch1 = _lastTouch1;
                    }
                }
                else if (touch.Phase == InputPhase.Ended || touch.Phase == InputPhase.Canceled)
                {
                    if (_lastTouch0 != null && possibleTouchId == touch0Id)
                    {
                        _lastTouch0 = null;
                        // done zooming for now
                    }

                    if (_lastTouch1 != null && possibleTouchId == touch1Id)
                    {
                        _lastTouch1 = null;
                    }
                }
                else if (touch.Phase == InputPhase.Held)
                {
                    if (_lastTouch0 != null && possibleTouchId == touch0Id)
                    {
                        touch0 = touch;
                    }
                    else if (_lastTouch1 != null && possibleTouchId == touch1Id)
                    {
                        touch1 = touch;
                    }
                }
            }

            // If we didn't find both touches, keep tracking any stray touch's
            // position.
            if (touch0 == null || touch1 == null)
            {
                return false;
            }

            if ((touch0.Transform == null) || (touch1.Transform == null))
            {
                return false;
            }

            var touch0Transform = touch0.Transform.Value;
            var touch1Transform = touch1.Transform.Value;

            // if both touches are on UI, filter it out, but if only 1
            // is over UI allow it

            if (TransformOverUI(touch0Transform) && TransformOverUI(touch1Transform))
            {
                return false;
            }

            // calculate the final pinch zoom difference
            var touch0Pos = touch0Transform.Position;
            var touch1Pos = touch1Transform.Position;

            Vector3 lastPos0 = _lastTouch0Position;
            Vector3 lastPos1 = _lastTouch1Position;

            float curDist = (touch1Pos - touch0Pos).magnitude;
            float prevDist = (lastPos1 - lastPos0).magnitude;
            float pinchChange = prevDist - curDist;

            var screenWidthInches = Screen.width / Screen.dpi;
            pinchChange *= screenWidthInches;
            ZoomFraction = Mathf.Clamp01(
                ZoomFraction + pinchChange * _settings.TouchPinchZoomSpeed
            );

            // Now check if there is any rotation in this zoom
            var lastDirection = (_lastTouch1Position - _lastTouch0Position).normalized;
            var thisDirection = (touch1Pos - touch0Pos).normalized;
            var determinant =
                (lastDirection.x * thisDirection.y) - (lastDirection.y * thisDirection.x);

            RotateByDirections(lastDirection, thisDirection, determinant);

            _lastTouch0Position = touch0Pos;
            _lastTouch1Position = touch1Pos;
            return true;
        }

        private void ProcessZoom()
        {
            if (!GesturesEnabled)
            {
                _isCurrentlyZooming = false;
                return;
            }

            var wasZooming = _isCurrentlyZooming;
            _isCurrentlyZooming = false;
            if (!Mathf.Approximately(0.0f, _scrollDelta.y))
            {
                MouseZoom();
                _isCurrentlyZooming = true;
            }

            if (TouchZoomAndRotate())
            {
                _isCurrentlyZooming = true;
            }
            else if (wasZooming)
            {
                _lastTouch0 = null;
                _lastTouch1 = null;
            }

            if (_settings.DoubleTapZoomEnabled && DoubleTapZoom())
            {
                _isCurrentlyZooming = true;
            }
            else if (_isCurrentlyZooming)
            {
                // 2 finger zoom cancels double tap zoom
                ResetDoubleTapZoom();
            }
        }

        private void ResetDoubleTapZoom()
        {
            _firstZoomTap = null;
            _secondZoomTap = null;
            _firstTapEnded = false;
        }

        // TODO(mortoc): we need a general purpose strict statemachine to handle
        // this sort of thing.
        //
        // Returns true while zooming
        private bool DoubleTapZoom()
        {
            if (!GesturesEnabled)
            {
                return false;
            }

            // if the current possible doubletap is timed out without the second tap
            // occuring, clear out the first tap
            if (
                _firstZoomTap != null
                && _secondZoomTap == null
                && _firstZoomTap.Time < Time.unscaledTime - _settings.DoubleTapMaxTime
            )
            {
                ResetDoubleTapZoom();
            }

            // Find potential start if present
            if (_firstZoomTap == null)
            {
                _firstZoomTap = _transformationEvents.FirstOrDefault(
                    ie => ie.Phase == InputPhase.Began && !TransformOverUI(ie.Transform.Value)
                );

                _firstTapEnded = false;
                return false;
            }

            // Has start tap timed out?
            if (
                _secondZoomTap == null
                && _firstZoomTap != null
                && Time.unscaledTime - _firstZoomTap.Time > _settings.DoubleTapMaxTime
            )
            {
                ResetDoubleTapZoom();
                return false;
            }

            // has the first tap ended?
            if (_firstZoomTap != null && _firstTapEnded == false)
            {
                _firstTapEnded = _transformationEvents.Any(ie =>
                {
                    return ie.Source == _firstZoomTap.Source
                        && (ie.Phase == InputPhase.Ended || ie.Phase == InputPhase.Canceled);
                });
            }

            // At this point we know we have a valid first tap, see if we have a 2nd.
            if (_firstTapEnded && _secondZoomTap == null)
            {
                _secondZoomTap = _transformationEvents.FirstOrDefault(
                    ie => ie.Phase == InputPhase.Began && !TransformOverUI(ie.Transform.Value)
                );

                if (_secondZoomTap != null)
                {
                    _secondZoomTap.Consume(this);
                    _prevZoomPosition = _secondZoomTap.Transform.Value.Position;
                }

                return true;
            }

            if (_firstTapEnded && _firstZoomTap != null && _secondZoomTap != null)
            {
                // Zooming detected
                InputEvent duplicateBeganZoomEvent = _transformationEvents.FirstOrDefault(ie =>
                {
                    return (
                        ie.Phase == InputPhase.Began
                        && ie.Transform.Value.Id == _secondZoomTap.Transform.Value.Id
                    );
                });

                if (duplicateBeganZoomEvent != null)
                {
                    duplicateBeganZoomEvent.Consume(this);
                    return true;
                }

                var currentZoomEvent = _transformationEvents.FirstOrDefault(ie =>
                {
                    return (
                        ie.Phase == InputPhase.Held
                        && ie.Transform.Value.Id == _secondZoomTap.Transform.Value.Id
                    );
                });

                if (currentZoomEvent == null)
                {
                    ResetDoubleTapZoom();
                    return false;
                }
                else
                {
                    currentZoomEvent.Consume(this);
                    var newZoomPosition = currentZoomEvent.Transform.Value.Position;
                    if (currentZoomEvent.Phase == InputPhase.Held)
                    {
                        ZoomFraction = Mathf.Clamp01(
                            ZoomFraction
                                - (newZoomPosition - _prevZoomPosition).y
                                    * _settings.DoubleTapZoomSpeed
                        );
                    }

                    _prevZoomPosition = newZoomPosition;
                }

                return true;
            }

            return false;
        }

        private bool TransformOverUI(TransformData inputTransform)
        {
            var inputPos = new Vector2(
                inputTransform.Position[0] * Screen.width,
                inputTransform.Position[1] * Screen.height
            );

            return GuiUtil.IsScreenPositionOverUi(inputPos);
        }

        private void ProcessSwipe()
        {
            if (!GesturesEnabled)
            {
                _wasRotatingLastFrame = false;
                return;
            }

            var currentlyRotating = IsCurrentlyRotating;

            if (currentlyRotating)
            {
                var touch = _transformationEvents[0];
                var inputTransform = touch.Transform.Value;

                if (!_wasRotatingLastFrame)
                {
                    // we can only start if begin and not over UI
                    if (touch.Phase != InputPhase.Began || TransformOverUI(inputTransform))
                    {
                        return;
                    }
                }

                var swipePosition = inputTransform.Position;

                if (_lastSwipeFrame != Time.frameCount - 1)
                {
                    // no delta on first frame, fix it by getting the first position from
                    // Began
                    _lastSwipePosition = swipePosition;
                }

                var touchRay = _raycastCamera.ViewportPointToRay(swipePosition);
                var lastTouchRay = _raycastCamera.ViewportPointToRay(_lastSwipePosition);

                touchRay.direction = ClampDirToGround(touchRay.direction);
                lastTouchRay.direction = ClampDirToGround(lastTouchRay.direction);

                float rayDist;
                float lastRayDist;
                GroundPlane.Raycast(touchRay, out rayDist);
                GroundPlane.Raycast(lastTouchRay, out lastRayDist);

                var groundTouch = touchRay.GetPoint(rayDist);
                var lastGroundTouch = lastTouchRay.GetPoint(lastRayDist);

                var centerToLast = (lastGroundTouch - _focusObject.transform.position).normalized;
                var centerToThis = (groundTouch - _focusObject.transform.position).normalized;
                var determinant =
                    (centerToLast.x * centerToThis.z) - (centerToLast.z * centerToThis.x);
                RotateByDirections(centerToLast, centerToThis, determinant);

                _lastSwipePosition = swipePosition;
                _lastSwipeFrame = Time.frameCount;
            }

            _wasRotatingLastFrame = currentlyRotating;
        }

        // rotates from directionA toward directionB
        private void RotateByDirections(Vector3 directionA, Vector3 directionB, float direction)
        {
            if (
                Mathf.Approximately(directionA.magnitude, 0.0f)
                || Mathf.Approximately(directionB.magnitude, 0.0f)
            )
            {
                return;
            }

            var dot = Vector3.Dot(directionA, directionB);
            if (!Mathf.Approximately(1.0f, dot))
            {
                var angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (direction < 0.0f)
                {
                    angle *= -1.0f;
                }

                RotationAngleDegrees += angle;
                if (ManuallyRotated != null)
                {
                    ManuallyRotated(RotationAngleDegrees);
                }
            }
        }

        public void ProcessEvent(InputEvent inputEvent)
        {
            if (!GesturesEnabled)
            {
                return;
            }

            if (inputEvent.Transform.HasValue)
            {
                _transformationEvents.Add(inputEvent);
            }

            if (inputEvent.ScrollDelta.HasValue)
            {
                _scrollDelta += inputEvent.ScrollDelta.Value;
            }
        }

        public void PostProcessInput()
        {
            if (!GesturesEnabled)
            {
                return;
            }

            Assert.IsNotNull(_focusObject, "MapCameraGesture.RaycastCamera cannot be null");

            ProcessZoom();
            ProcessSwipe();

            _transformationEvents.Clear();
            _scrollDelta = Vector2.zero;
        }

        public override string ToString()
        {
            return string.Format(
                "MapCameraGesture: [Rotating: {0}, Zooming: {1}]\n" + "ZoomRatio:{2} Angle: {3}",
                _wasRotatingLastFrame,
                _isCurrentlyZooming,
                ZoomFraction,
                RotationAngleDegrees
            );
        }

        [Serializable]
        public class Settings
        {
            public float DefaultZoom = 0.75f;
            public float MouseScrollZoomSpeed = 1.5f;
            public float TouchPinchZoomSpeed = 2.0f;
            public bool DoubleTapZoomEnabled = false;
            public float DoubleTapZoomSpeed = 6.0f;
            public float DoubleTapMaxTime = 0.5f;
        }
    }
}
