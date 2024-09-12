using Firebase.Extensions;
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCreatedAudioLogHander : MonoBehaviour
{
    [SerializeField] private ChosenAudioLogExperienceSO chosenAudioLogExperienceSO;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private FirebaseManager firebaseManager;
    [SerializeField] private string audioLogFilename;

    private void Start()
    {
        if (chosenAudioLogExperienceSO.chosenAudioLog != null)
        {
            audioLogFilename = chosenAudioLogExperienceSO.chosenAudioLog.filename;
        }
    }

    public void HandlePlayerCreatedAudioLog()
    {
        GetAudioFileFromFirebase();
    }
    private void GetAudioFileFromFirebase()
    {
        // Get a reference to the storage service, using the default Firebase App
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Create a storage reference from our storage service
        StorageReference storageRef =
            storage.GetReferenceFromUrl("gs://thewild-3f4c7.appspot.com");

        // Create a child reference
        // imagesRef now points to "images"
        StorageReference playerAudioLogsRef = storageRef.Child("player-audio-logs");

        // Create a reference to the file you want to upload
        StorageReference audioLogDbRef = storageRef.Child($"player-audio-logs/{audioLogFilename}.wav");

        // Fetch the download URL
        audioLogDbRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Download URL: " + task.Result);
                StartCoroutine(GetAudioClip(task.Result.ToString()));
            }
            else
            {
                Debug.Log("PlayerCreatedAudioLogHander:: fetch audio log didn't work");
            }
        });
    }

    private IEnumerator GetAudioClip(string uri)
    {
        Debug.Log("PlayerCreatedAudioLogHander:: GetAudioClip");
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("PlayerCreatedAudioLogHander:: " + www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("PlayerCreatedAudioLogHander:: audio clip returned" + myClip);
                audioSource.clip = myClip;
                audioSource.Play();
            }
        }
    }
}
