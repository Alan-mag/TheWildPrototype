using Firebase.Database;
using Firebase.Extensions;
using Niantic.Lightship.Maps.Coordinates;
using Niantic.Lightship.Maps.Unity.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationMapManager : MonoBehaviour
{

    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject tutorialMapSpawner;
    [SerializeField] private GameObject audioLogMapObject;

    [SerializeField] private GameObject expeditionMapObject;

    [SerializeField] LightshipMap lightshipMap;

    private string userId;
    private DatabaseReference userStatsDbReference;

    private void Awake()
    {
        if (PlayerPrefs.GetString("user_id") != null)
        {
            userId = PlayerPrefs.GetString("user_id");
            userStatsDbReference = FirebaseDatabase.DefaultInstance.GetReference($"/{userId}/stats/");
        }
    }

    private void Start()
    {
        userStatsDbReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value == null)
                {
                    Vector3 spawnLocation = new Vector3(20.0f, 10.0f, 40.0f);
                    GameObject tutorialSpawnObj = Instantiate(tutorialMapSpawner, spawnLocation, Quaternion.identity);
                }
                else
                {
                    // render map stuff
                    // lightshipMap.SpawnGameplayFeatures();
                    StartCoroutine(ExecuteAfterTime(3, RetrieveAudioLogs)); // delaying to account for map reposition // todo: fix map reposition

                    // test expedition stuff:
                    StartCoroutine(ExecuteAfterTime(3, TestSpawnExpedition));
                }
            }
        });
    }

    private void RetrieveAudioLogs()
    {
        FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager>();
        firebaseManager.GetAudioLogs(SpawnAudioLogs);
    }

    // todo: does this spawn only items on current map? 
    // or every log that will be in database -- might be issue
    private void SpawnAudioLogs(List<AudioLogData> logs)
    {
        foreach (AudioLogData log in logs)
        {
            LatLng latLng = new LatLng(log.latitude, log.longitude);
            // Debug.Log(log.message);
            GameObject audioObj = Instantiate(audioLogMapObject, lightshipMap.LatLngToScene(in latLng), Quaternion.identity);
            audioObj.GetComponent<AudioLogMapHandler>().SetObjectMessageLog(log.message);
        }
    }

    private void TestSpawnExpedition()
    {
        LatLng latLng1 = new LatLng(47.64911685880216, -122.34881377439586);
        Instantiate(expeditionMapObject, lightshipMap.LatLngToScene(in latLng1), Quaternion.identity);
    }

    IEnumerator ExecuteAfterTime(float time, Action callback)
    {
        yield return new WaitForSeconds(time);

        callback();
    }
}
