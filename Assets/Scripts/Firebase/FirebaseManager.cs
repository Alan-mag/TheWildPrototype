using Firebase.Database;
using Firebase.Storage;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Niantic.Lightship.SharedAR.Networking;

// create firebase utility file?

public enum EXPERIENCE_TYPE
{
    Explorer,
    Adventurer,
    Creator,
}

// TODO:
// This file is a mess, need to clean
// should have persistent firebase api
// call api with type and value to increment, and have it save to db
// should have local saved db in PlayerPrefs 
// and firebase db updates
// when pulling for stats page, eventually pull from PlayerPrefs (not necessary for prototype but it would limit db calls)

public class FirebaseManager : MonoBehaviour
{
    [SerializeField] string userId;
    [SerializeField] DatabaseReference userRootDbReference;
    [SerializeField] DatabaseReference userStatsDbReference;

    private void Awake()
    {
        if (PlayerPrefs.GetString("user_id") != null)
        {
            userId = PlayerPrefs.GetString("user_id");
            userRootDbReference = FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/");
            userStatsDbReference = FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/stats/");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (userId == null)
        {
            throw new UnassignedReferenceException("User Id in FirebaseManager is null on Start.");
        }
    }

    // TODO: eventually should probably return status
    public void AddPlayerCreatedPuzzle(string puzzleData)
    {
        Debug.Log("AddPlayerCreatedPuzzle");
        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/sphere_puzzles/").Push().SetRawJsonValueAsync(puzzleData); // todo: clean up

        // pool of puzzles in db
        FirebaseDatabase.DefaultInstance.GetReference($"/community_puzzles/").Push().SetRawJsonValueAsync(puzzleData);
    }

    public void AddAudioLogToDatabase(string audioLogData)
    {
        Debug.Log("AddAudioLogToDatabase");
        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/audio_logs/").Push().SetRawJsonValueAsync(audioLogData); // player audio logs stored in user info
        FirebaseDatabase.DefaultInstance.GetReference($"/community_audio_logs/").Push().SetRawJsonValueAsync(audioLogData); // community audio logs
    }

    public void AddPlayerCreatedAudioLogToDatabase(string audioLogData)
    {
        Debug.Log("AddAudioLogToDatabase");
        Debug.Log(audioLogData);

        // Get a reference to the storage service, using the default Firebase App
        /*FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Create a storage reference from our storage service
        StorageReference storageRef =
            storage.GetReferenceFromUrl("gs://thewild-3f4c7.appspot.com");

        // Create a child reference
        // imagesRef now points to "images"
        StorageReference playerAudioLogsRef = storageRef.Child("player-audio-logs");

        // Create a reference to the file you want to upload
        StorageReference audioLogTestRef = storageRef.Child("player-audio-logs/playerAudioLog1.txt"); // TODO: update test


        audioLogTestRef.PutBytesAsync(audioLogData)
            .ContinueWith((Task<StorageMetadata> task) => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Metadata contains file metadata such as size, content-type, and md5hash.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                }
            });*/


        // Todo: this needs firebase datastorage, not firebase realtime db, probably need to create another file
        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/audio_logs/").Push().SetRawJsonValueAsync(audioLogData); // player audio logs stored in user info
        // FirebaseDatabase.DefaultInstance.GetReference($"/community_created_audio_logs/").Push().SetRawJsonValueAsync(audioLogData); // community audio logs
    }

    public void AddSignalSequenceToDatabase(string signalSeqData)
    {
        Debug.Log("AddSignalSequenceToDatabase");
        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/signals/").Push().SetRawJsonValueAsync(signalSeqData);
        FirebaseDatabase.DefaultInstance.GetReference($"/community_signals/").Push().SetRawJsonValueAsync(signalSeqData);
    }

    public void GetAudioLogs(Action<List<AudioLogData>> callback) // eventually add param for current location, then filter by that
    {
        List<AudioLogData> audioLogsList = new List<AudioLogData>();

        FirebaseDatabase.DefaultInstance.GetReference("/community_audio_logs/")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error getting audio logs from db");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (DataSnapshot logObject in snapshot.Children)
                    {
                        AudioLogData log = JsonUtility.FromJson<AudioLogData>(logObject.GetRawJsonValue()); // this is how you do the mapping !!
                        audioLogsList.Add(log);
                    }
                    callback(audioLogsList);
                }
            });
    }

    // TODO:
    // clean up this delegate nested mess
    public void UpdatePlayerExperience(EXPERIENCE_TYPE expType, float incrementValue)
    {
        GetPlayerExperienceForStat(expType, incrementValue, AddPlayerExperience);
    }

    public void AddPlayerExperience(EXPERIENCE_TYPE expType, float incrementValue) // test method, should return success or failure
    {

        userStatsDbReference.Child(expType.ToString()).SetValueAsync(incrementValue);
    }

    public void AddFirstExperience(EXPERIENCE_TYPE expType, float intValue)
    {
        Debug.Log("AddFirstExperience");
        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/stats/{expType.ToString()}/").SetValueAsync(intValue);
    }

    public void GetPlayerExperienceForStat(EXPERIENCE_TYPE expType, float intValue, Action<EXPERIENCE_TYPE, float> callback)
    {
        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/stats/{expType.ToString()}")
            .GetValueAsync().ContinueWithOnMainThread(task =>
           {
               if (task.IsFaulted)
               {
                   Debug.LogError("Error getting player exp from db");
                   AddFirstExperience(expType, intValue);
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;
                   Debug.Log(snapshot);
                   float newValue;
                   if (snapshot.Value != null)
                   {
                       newValue = intValue + float.Parse(snapshot.Value.ToString());
                   }
                   else
                   {
                       newValue = intValue;
                   }
                   Debug.Log(snapshot.Value);
                   callback(expType, newValue);
               }
           });
    }

    public void GetPlayerList(Action<List<string>> callback)
    {
        List<string> playerList = new List<string>();

        FirebaseDatabase.DefaultInstance.GetReference($"players/")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error getting players from db");
                    // return playerList;
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot);
                    if (snapshot.Value != null)
                    {
                        foreach (var child in snapshot.Children)
                        {
                            foreach (var i in child.Children)
                            {
                                //Debug.Log(i.Value);
                                // playerList.Add(i.Value.ToString());
                                if (i.Key == "username")
                                {
                                    Debug.Log(i.Value);
                                    playerList.Add(i.Value.ToString());
                                }
                            }
                            callback(playerList);
                        }
                    }
                    else
                    {
                        // no players in db
                        // return playerList;
                    }
                }
                // return playerList;
            });
        // return playerList;
    }


    ////////////////////////////// COLLECTIONS METHODS ////////////////////////////////////////////////
    public void GetPlayerPuzzleData(string puzzleType, Action<string> callback)
    {
        Debug.Log("GetPlayerPuzzleData");

        // what we return might change depending on type, since we're not just returning a string? or I guess we could parse
        // a json string in switch case and go from there?

        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/{puzzleType}/").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted) { Debug.Log("Unable to get player data at puzzle reference"); } // todo: how to handle empty use case?
            else
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot.GetRawJsonValue());
                    callback(snapshot.GetRawJsonValue());

                    // could foreach child in snapshot --> map to signal object
                    // then return list of signal objects, handle those with 
                    // ui rendering, and use SO to select correct signal depending on which is selected and pass to 
                    // signal scene
                }
            }
        });
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////
}