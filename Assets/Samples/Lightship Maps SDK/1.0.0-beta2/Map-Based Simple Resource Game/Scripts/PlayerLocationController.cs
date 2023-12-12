// Copyright 2022 Niantic, Inc. All Rights Reserved.

using System;
using System.Collections;
using Niantic.Lightship.Maps.Coordinates;
using Niantic.Lightship.Maps.Unity.Core;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Niantic.Lightship.Maps.Samples.GameSample
{
    /// <summary>
    /// This class positions the gameobject based on gps data from device and updates
    /// the LightshipMap with these movements to keep the map view centered on the player
    /// The map view position is updated periodically to avoid spamming the map service.
    /// Much of this class is also handling movement in editor using WASD
    ///
    /// TODO: use the camera angle to center the map on the visible area
    /// </summary>
    public class PlayerLocationController : MonoBehaviour
    {
        [SerializeField]
        private LightshipMap _lightshipMap;

        [SerializeField]
        private LatLng _defaultCoordinates;

        [SerializeField]
        private float _editorMovementSpeed;

        [SerializeField]
        private Camera _camera;

        private double _lastGpsUpdateTime;
        private Vector3 _targetMapPosition;
        private Vector3 _currentMapPosition;
        private float _lastMapViewUpdateTime;

        public Action<string> OnGPSError; // event to notify the UI about any issues with the GPS location

        private void Start()
        {
            _lightshipMap.MapOriginChanged += OnMapOriginChanged;

            // start the player at a default location, mainly used for editor testing
            UpdatePlayerLocation((float)_defaultCoordinates.Latitude, (float)_defaultCoordinates.Longitude);

            _currentMapPosition = _targetMapPosition = transform.position;

            StartCoroutine(UpdateGPSLocation());
        }

        private void OnMapOriginChanged(LatLng center)
        {
            var offset = _targetMapPosition - _currentMapPosition;
            _currentMapPosition = _lightshipMap.LatLngToScene(center);
            _targetMapPosition = _currentMapPosition + offset;
        }

        private IEnumerator UpdateGPSLocation()
        {
            yield return null;

            if (Application.isEditor)
            {
                while (isActiveAndEnabled)
                {
                    UpdateEditorInput();
                    yield return null;
                }
            }
            else
            {
#if UNITY_ANDROID
                // request location permission for Android
                if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                {
                    Permission.RequestUserPermission(Permission.FineLocation);
                    while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                    {
                        // wait till permission is enabled
                        yield return new WaitForSeconds(1.0f);
                    }
                }
#endif

                // Check if the user has location service enabled.
                if (!Input.location.isEnabledByUser)
                {
                    OnGPSError?.Invoke("Location permission not enabled");
                    yield break;
                }

                // Starts the location service.
                Input.location.Start();

                // Waits until the location service initializes
                int maxWait = 20;
                while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
                {
                    yield return new WaitForSeconds(1);
                    maxWait--;
                }

                // If the service didn't initialize in 20 seconds this cancels location service use.
                if (maxWait < 1)
                {
                    OnGPSError?.Invoke("GPS initialisation timed out");
                    yield break;
                }

                // If the connection failed this cancels location service use.
                if (Input.location.status == LocationServiceStatus.Failed)
                {
                    OnGPSError?.Invoke("Unable to determine device location");
                    yield break;
                }
                else
                {
                    while (isActiveAndEnabled)
                    {
                        var gpsInfo = Input.location.lastData;
                        if (gpsInfo.timestamp > _lastGpsUpdateTime)
                        {
                            _lastGpsUpdateTime = gpsInfo.timestamp;
                            UpdatePlayerLocation(gpsInfo.latitude, gpsInfo.longitude);
                        }

                        yield return null;
                    }
                }

                // Stops the location service if there is no need to query location updates continuously.
                Input.location.Stop();
            }
        }

        private void UpdatePlayerLocation(float latitude, float longitude)
        {
            // new gps location data available, will lerp the players position to this new coordinate, or jump if it is far
            _targetMapPosition = _lightshipMap.LatLngToScene(new LatLng(latitude, longitude));
        }

        public void Update()
        {
            // update the map view position based on where our player is, this will actually be last frames position
            // but the map update needs to happen first as the player is positioned on the map relative to the
            // offset to the tile parent node
            UpdateMapViewPosition();

            // maintain the players position on the map, and interpolate to new coordinates as they come in

            // interpolate players map position without the camera offset, so that camera movements don't result in lerps

            // jump rather than interpolate if the coordinates are really far
            float movementDistance = (_currentMapPosition - _targetMapPosition).magnitude;
            if (movementDistance > 200.0f)
                _currentMapPosition = _targetMapPosition;

            // rotate the player to face their movement, as long as it's not a stationary
            if (movementDistance > 0.1f)
            {
                var forward = (_targetMapPosition - _currentMapPosition).normalized;
                var rotation = Quaternion.LookRotation(forward, Vector3.up);
                transform.rotation = rotation;
            }

            _currentMapPosition = Vector3.Lerp(_currentMapPosition, _targetMapPosition, Time.deltaTime / 2.0f);
            transform.position = _currentMapPosition;
        }

        private void UpdateEditorInput()
        {
            // for editor, move the character around with keyboard rather than GPS

            var movementVector = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movementVector += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movementVector -= Vector3.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movementVector += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movementVector += Vector3.right;
            }

            // make editor movement relative to camera facing direction
            var cameraForward = _camera.transform.forward;
            float yRotation = Vector3.SignedAngle(Vector3.forward, cameraForward, Vector3.up);
            movementVector = Quaternion.AngleAxis(yRotation, Vector3.up) * movementVector;

            _targetMapPosition += movementVector * _editorMovementSpeed * Time.deltaTime;
        }

        private void UpdateMapViewPosition()
        {
            // only update the map tile view periodically so as not to spam tile fetches
            if (Time.time < _lastMapViewUpdateTime + 1.0f)
            {
                return;
            }

            _lastMapViewUpdateTime = Time.time;

            // Update the map's view based on where our player is
            _lightshipMap.SetMapCenter(transform.position);
        }
    }
}
