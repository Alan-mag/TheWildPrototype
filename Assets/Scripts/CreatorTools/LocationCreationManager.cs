// Copyright 2022 Niantic, Inc. All Rights Reserved.
// TODO: Refactor for ARDK 3.0

using System;
using Newtonsoft.Json;
using Niantic.ARDK;
// using Niantic.ARDK.LocationService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// using LocationServiceStatus = Niantic.ARDK.LocationService.LocationServiceStatus;

public class LocationCreationManager : MonoBehaviour
{
    /*[SerializeField]
    private RuntimeEnvironment _coverageClientRuntime = RuntimeEnvironment.Default;

    [SerializeField]
    private InputField _locationUpdateDistanceField;

    [SerializeField]
    private TMP_InputField _audioLogTextField;

    [SerializeField]
    private Button _startGpsButton;

    [SerializeField]
    private Image _gpsImage;

    [SerializeField]
    private Text _currLocationText;

    private Color _gpsEnabledColor = new Color(102 / 255f, 204 / 255f, 255 / 255f);

    private ILocationService _locationService;

    private double _latitude;
    private double _longitude;

    private bool _enabledLocation;

    void Awake()
    {
        _locationService = LocationServiceFactory.Create(_coverageClientRuntime);
    }

    void OnEnable()
    {
        _startGpsButton.onClick.AddListener(ToggleLocationService);
    }

    void OnDisable()
    {
        _startGpsButton.onClick.RemoveListener(ToggleLocationService);
    }

    void OnDestroy()
    {
        if (_locationService != null && _enabledLocation)
            ToggleLocationService();
    }

    void ToggleLocationService()
    {
        if (!_enabledLocation)
        {
            if (_coverageClientRuntime == RuntimeEnvironment.Mock)
            {
                var spoofService = (SpoofLocationService)_locationService;

                // Optional. If no location is specified, a default location near the Ferry Building in
                // San Francisco will be surfaced.
                // Location here is near Coit Tower in San Francisco, about 1.2km from the coverage areas
                // surfaced in mock requests.
                spoofService.SetLocation(37.802241533471964, -122.40578895525384);
            }

            _locationService.StatusUpdated += OnStatusUpdated;
            _locationService.LocationUpdated += OnLocationUpdated;

            int updateDistance = 1;
            if (!string.IsNullOrWhiteSpace(_locationUpdateDistanceField.text))
            {
                if (!Int32.TryParse(_locationUpdateDistanceField.text, out updateDistance))
                {
                    Debug.LogError("Location update distance input must be a number.");
                    return;
                }
            }

            _locationService.Start(10f, updateDistance);

            _gpsImage.color = _gpsEnabledColor;
            _enabledLocation = true;
        }
        else
        {
            _locationService.StatusUpdated -= OnStatusUpdated;
            _locationService.LocationUpdated -= OnLocationUpdated;
            _locationService.Stop();

            _enabledLocation = false;

            if (_gpsImage != null)
                _gpsImage.color = Color.white;
        }
    }

    void OnStatusUpdated(LocationStatusUpdatedArgs args)
    {
        switch (args.Status)
        {
            case LocationServiceStatus.Stopped:
                _currLocationText.text = "N/A";
                break;

            case LocationServiceStatus.PermissionFailure:
                _currLocationText.text = "User denied permission to location service.";
                break;

            case LocationServiceStatus.DeviceAccessError:
                _currLocationText.text = "User disabled system location services.";
                break;

            case LocationServiceStatus.Running:
                StartTravel();
                break;
        }
    }

    void OnLocationUpdated(LocationUpdatedArgs args)
    {
        _latitude = args.LocationInfo.Coordinates.Latitude;
        _longitude = args.LocationInfo.Coordinates.Longitude;
        _currLocationText.text = args.LocationInfo.ToString();
    }

    void StartTravel()
    {
        if (_locationService is SpoofLocationService)
        {
            var spoofService = _locationService as SpoofLocationService;

            // Travel 100 meters with a 30º bearing at 1m/sec
            spoofService.StartTravel(30, 100, 1);
        }
    }

    public void SaveExperienceToDatabase()
    {
        Debug.Log("latitude: " + _latitude);
        Debug.Log("longitude: " + _longitude);
        Debug.Log("text: " + _audioLogTextField.text);
        AudioLogData data = new AudioLogData(_latitude, _longitude, _audioLogTextField.text);

        FirebaseManager firebaseManager = GameObject.FindObjectOfType<FirebaseManager>();
        firebaseManager.AddPlayerCreatedAudioLogToDatabase(data.ToJson());
    }*/
}


