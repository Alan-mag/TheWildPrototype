using Firebase.Database;
using Firebase.Storage;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Niantic.Lightship.SharedAR.Networking;
using Newtonsoft.Json;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using System.IO;

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
    [SerializeField] GameProgressionSO gameProgressionSO;

    private static string UriFileScheme = Uri.UriSchemeFile + "://";
    protected CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

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

    public void AddPlayerCreatedAudioLogToDatabase(PlayerAudioLogData playerAudioLog)
    {
        Debug.Log("AddAudioLogToDatabase:: " + playerAudioLog.filename);

        // Get a reference to the storage service, using the default Firebase App
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Create a storage reference from our storage service
        StorageReference storageRef =
            storage.GetReferenceFromUrl("gs://thewild-3f4c7.appspot.com");

        // Create a child reference
        // imagesRef now points to "images"
        StorageReference playerAudioLogsRef = storageRef.Child("player-audio-logs");

        // Create a reference to the file you want to upload
        StorageReference audioLogDbRef = storageRef.Child($"player-audio-logs/{playerAudioLog.filename}.wav"); // TODO: update test

        var metaData = new MetadataChange();
        metaData.ContentType = "audio/wav";
        string localFilePath = "file://" + Application.persistentDataPath + "/" + playerAudioLog.filename + ".wav";

        audioLogDbRef.PutFileAsync(localFilePath, metaData)
            .ContinueWith((Task<StorageMetadata> task) => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                    FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/audio_logs/").Push().SetRawJsonValueAsync(playerAudioLog.ToJson());
                    FirebaseDatabase.DefaultInstance.GetReference($"/player_created_audio_logs/").Push().SetRawJsonValueAsync(playerAudioLog.ToJson());
                    File.Delete(localFilePath);
                }
            });

        // Todo: this needs firebase datastorage, not firebase realtime db, probably need to create another file
        // need a query for 'friends' audio logs
        // FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/audio_logs/").Push().SetRawJsonValueAsync(playerAudioLog.ToJson()); // player audio logs stored in user info
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
    
    public void GetPlayerCreatedAudioLogs(Action<List<PlayerAudioLogData>> callback) // eventually add param for current location, then filter by that
    {
        List<PlayerAudioLogData> audioLogsList = new List<PlayerAudioLogData>();

        FirebaseDatabase.DefaultInstance.GetReference("/player_created_audio_logs/")
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
                        PlayerAudioLogData log = JsonUtility.FromJson<PlayerAudioLogData>(logObject.GetRawJsonValue()); // this is how you do the mapping !!
                        audioLogsList.Add(log);
                    }
                    callback(audioLogsList);
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
    public void GetPlayerSignals(Action<List<SignalData>> callback)
    {
        // what we return might change depending on type, since we're not just returning a string? or I guess we could parse
        // a json string in switch case and go from there?
        List<SignalData> playerSignals = new List<SignalData>();

        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/signals/").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("FirebaseManager:: Error: unable to access player/signals db reference");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value == null)
                {
                    Debug.Log("SignalExperiencesMapManager:: Warning: no player/signals db objects to pull");
                }
                else
                {
                    foreach (DataSnapshot playerSignalSnapshot in snapshot.Children)
                    {
                        Debug.Log("FirebaseManager:: " + playerSignalSnapshot.GetRawJsonValue());
                        SignalData signalData = JsonConvert.DeserializeObject<SignalData>(playerSignalSnapshot.GetRawJsonValue());
                        playerSignals.Add(signalData);
                    }
                    callback(playerSignals);
                }
            };
        });
    }

    public void GetPlayerSpheres(Action<List<PuzzleSphereInformation>> callback)
    {
        Debug.Log("FirebaseManager:: " + "GetPlayerSpheres");
        // what we return might change depending on type, since we're not just returning a string? or I guess we could parse
        // a json string in switch case and go from there?
        List<PuzzleSphereInformation> playerSpheres = new List<PuzzleSphereInformation>();

        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/sphere_puzzles/").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("FirebaseManager:: Error: unable to access player/spheres db reference");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("FirebaseManager:: " + snapshot.Value);
                if (snapshot.Value == null)
                {
                    Debug.Log("SignalExperiencesMapManager:: Warning: no player/spheres db objects to pull");
                }
                else
                {
                    foreach (DataSnapshot playerSphereSnapshot in snapshot.Children)
                    {
                        foreach (DataSnapshot sphereObject in playerSphereSnapshot.Children)
                        {
                            Debug.Log("FirebaseManager:: " + sphereObject.Value);
                            PuzzleSphereInformation puzzleData = JsonConvert.DeserializeObject<PuzzleSphereInformation>(sphereObject.Value.ToString());
                            Debug.Log("FirebaseManager:: " + puzzleData);
                            playerSpheres.Add(puzzleData);

                        }

                    }
                    callback(playerSpheres);
                }
            };
        });
    }

    public void GetPlayerAudioLogs(Action<List<PlayerAudioLogData>> callback)
    {
        // what we return might change depending on type, since we're not just returning a string? or I guess we could parse
        // a json string in switch case and go from there?
        List<PlayerAudioLogData> playerAudioLogs = new List<PlayerAudioLogData>();

        FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/audio_logs/").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("FirebaseManager:: Error: unable to access player/audio_logs db reference");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value == null)
                {
                    Debug.Log("SignalExperiencesMapManager:: Warning: no player/audio_logs db objects to pull");
                }
                else
                {
                    foreach (DataSnapshot playerAudioLogSnapshot in snapshot.Children)
                    {
                        Debug.Log("FirebaseManager:: " + playerAudioLogSnapshot.GetRawJsonValue());
                        PlayerAudioLogData audioLogData = JsonConvert.DeserializeObject<PlayerAudioLogData>(playerAudioLogSnapshot.GetRawJsonValue());
                        playerAudioLogs.Add(audioLogData);
                    }
                    callback(playerAudioLogs);
                }
            };
        });
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////// MANAGE STATS SECTION ////////////////////////////////////////////////
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
    ///////////////////////////////////////////////////////////////////////////////////////////////////
}


