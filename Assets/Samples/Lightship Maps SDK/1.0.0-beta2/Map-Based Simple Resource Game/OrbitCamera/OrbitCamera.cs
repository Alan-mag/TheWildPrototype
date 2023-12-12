// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Niantic.Platform.Util.OrbitCameraInternal;

namespace Niantic.Platform.Util
{
    /// <summary>
    /// Camera for Niantic-standard map camera interactions, similar to pokemon go camera
    /// </summary>
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField]
        private float _minimumZoomDistance = 23f;

        [SerializeField]
        private float _maximumZoomDistance = 99f;

        [SerializeField]
        private float _minimumPitchDegrees = 20.0f;

        [SerializeField]
        private float _maximumPitchDegrees = 60.0f;

        [SerializeField]
        private float _verticalFocusOffset = 10.0f;

        [SerializeField]
        private CameraGestureTracker.Settings _gestureSettings = null;

        [SerializeField]
        private Camera _camera = null;

        [SerializeField]
        private GameObject _focusObject = null;

        private InputService _inputService;

        private CameraGestureTracker _gestureTracker;
        private IZoomCurveEvaluator _zoomCurveEvaluator;

        public CameraGestureTracker GestureTracker
        {
            get { return _gestureTracker; }
        }

        public void Awake()
        {
            _gestureTracker = new CameraGestureTracker(_camera, _focusObject, _gestureSettings);

            _inputService = new InputService(EventSystem.current);
            _inputService.Initialize(new IScreenInputGesture[] { _gestureTracker });

            _zoomCurveEvaluator = new QuadraticAngleZoomCurveEvaluator();
            _zoomCurveEvaluator.SetCameraParameters(
                new NianticCameraParameters(
                    _minimumZoomDistance,
                    _maximumZoomDistance,
                    _minimumPitchDegrees,
                    _maximumPitchDegrees,
                    _verticalFocusOffset
                )
            );
        }

        public void Update()
        {
            _inputService.Update();
        }

        // Late update to ensure we use the latest avatar position
        public void LateUpdate()
        {
            float rotationAngleDegrees = _gestureTracker.RotationAngleDegrees;
            float rotationAngleRadians = Mathf.Deg2Rad * rotationAngleDegrees;
            float zoomFraction = _gestureTracker.ZoomFraction;

            float distance = _zoomCurveEvaluator.GetDistanceFromZoomFraction(zoomFraction);
            float pitchDegrees = _zoomCurveEvaluator.GetAngleFromDistance(distance);
            float elevMeters = _zoomCurveEvaluator.GetElevationFromDistance(distance);

            // Position camera above the x-z plane, according to our pitch and
            // distance constraints.
            Vector3 offsetPos = new Vector3(
                -distance * Mathf.Sin(rotationAngleRadians),
                elevMeters,
                -distance * Mathf.Cos(rotationAngleRadians)
            );

            _camera.transform.position = _focusObject.transform.position + offsetPos;
            _camera.transform.rotation = Quaternion.Euler(pitchDegrees, rotationAngleDegrees, 0.0f);
        }
    }
}
