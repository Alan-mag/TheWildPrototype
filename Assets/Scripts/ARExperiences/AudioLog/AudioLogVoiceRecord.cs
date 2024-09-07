using Niantic.Lightship.AR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioLogVoiceRecord : MonoBehaviour
{
    [Header("Database Reference")]
    [SerializeField] FirebaseManager firebaseManager;

    [Header("Recording Functionality")]
    [SerializeField] private Button recordButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button playbackButton;
    [SerializeField] private Button saveButton;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int clipLength = 10;

    [Header("Location Service")]
    [SerializeField] private Button startLocationButton;
    
    private double _latitude;
    private double _longitude;
    private Byte[] _audioClipBytes;
    private bool _enabledLocation;

    public void StartRecording()
    {
        // first name is 'Android audio input' for my android test device
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        audioSource.clip = Microphone.Start(Microphone.devices.First().ToString(), false, clipLength, 44100);
        recordButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
    }
    public void StopRecording()
    {
        Microphone.End(Microphone.devices.First().ToString());
        stopButton.gameObject.SetActive(false);
        recordButton.gameObject.SetActive(true);
        playbackButton.gameObject.SetActive(true);
        saveButton.gameObject.SetActive(true);
    }

    public void PlaybackRecording()
    {
        audioSource.Play();
    }

    public void SaveAudioLog()
    {
        Debug.Log("Save audio clip!");
        Debug.Log("latitude: " + _latitude);
        Debug.Log("longitude: " + _longitude);
        Debug.Log("Audio clip exists: " + audioSource.clip != null);
        _audioClipBytes = ConvertAudioClipToByteArray(audioSource.clip);
        PlayerAudioLogData logData = new PlayerAudioLogData(
            _latitude.ToString(),
            _longitude.ToString(),
            _audioClipBytes
        );
        Debug.Log(logData);
        // firebaseManager.AddPlayerCreatedAudioLogToDatabase("[\r\n  0,\r\n  1,\r\n  2\r\n]");
        firebaseManager.AddPlayerCreatedAudioLogToDatabase(logData.ToJson());
        // todo: save to firebase
        // convert clip to byte array
        // add data to PlayerAudioLog object
        // turn that object into string
        // call firebase to save that data
    }

    private byte[] ConvertAudioClipToByteArray(AudioClip audioClip)
    {
        var samples = new float[audioClip.samples];

        audioClip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];

        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        return bytesData;

    }

    public void StartGetLocation()
    {
        StartCoroutine(GetLocation());
    }

    IEnumerator GetLocation()
    {
        // Check if the user has location service enabled.
        if (!UnityEngine.Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        // Starts the location service.
        UnityEngine.Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (UnityEngine.Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (UnityEngine.Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + UnityEngine.Input.location.lastData.latitude + " " + UnityEngine.Input.location.lastData.longitude + " " + UnityEngine.Input.location.lastData.altitude + " " + UnityEngine.Input.location.lastData.horizontalAccuracy + " " + UnityEngine.Input.location.lastData.timestamp);
            // Stops the location service if there is no need to query location updates continuously.
            UnityEngine.Input.location.Stop();
            _latitude = UnityEngine.Input.location.lastData.latitude;
            _longitude = UnityEngine.Input.location.lastData.longitude;
        }
    }
}
