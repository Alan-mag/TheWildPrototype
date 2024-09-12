using Firebase.Database;
using Firebase.Extensions;
using Niantic.Lightship.Maps.Coordinates;
using Niantic.Lightship.Maps.Unity.Builders.BaseTypes;
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
    [SerializeField] private GameObject playerCreatedAudioLogMapObject;

    [SerializeField] private GameObject expeditionMapObject;

    [SerializeField] LightshipMap lightshipMap;

    // TODO: at some point hopefully this can be handled outside of the ligthshipmap file [where it is now]
    /*[SerializeField]
    private List<FeatureBuilderBase> arExperienceBuilders;*/

    [SerializeField]
    private GameObject _mapTilePrefab;

    [SerializeField] Dictionary<string, int> mapExperiencesCollection;

    private string userId;
    private DatabaseReference userStatsDbReference;
    private ObjectPool<MapTileObject> _mapTileObjectPool;

    private void Awake()
    {
        if (PlayerPrefs.GetString("user_id") != null)
        {
            userId = PlayerPrefs.GetString("user_id");
            userStatsDbReference = FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/stats/");
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
                    // Get community audio logs (curated)
                    StartCoroutine(ExecuteAfterTime(3, RetrieveAudioLogs)); // delaying to account for map reposition // todo: fix map reposition

                    // Get player created audio logs
                    StartCoroutine(ExecuteAfterTime(3, RetrievePlayerCreatedAudioLogs));

                    // test expedition stuff:
                    StartCoroutine(ExecuteAfterTime(3, TestSpawnExpedition));

                    // StartCoroutine(ExecuteAfterTime(3, InitializeMapPOIExperiences));
                }
            }
        });
    }

    private void RetrieveAudioLogs()
    {
        FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager>();
        firebaseManager.GetAudioLogs(SpawnAudioLogs);
    }

    private void RetrievePlayerCreatedAudioLogs()
    {
        FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager>();
        firebaseManager.GetPlayerCreatedAudioLogs(SpawnPlayerCreatedAudioLogs);
    }

    private void SpawnAudioLogs(List<AudioLogData> logs)
    {
        foreach (AudioLogData log in logs)
        {
            LatLng latLng = new LatLng(log.latitude, log.longitude);
            GameObject audioObj = Instantiate(audioLogMapObject, lightshipMap.LatLngToScene(in latLng), Quaternion.identity);
            audioObj.GetComponent<AudioLogMapObject>().title = "";
            audioObj.GetComponent<AudioLogMapObject>().description = log.message;
            audioObj.GetComponent<AudioLogMapObject>().fmodAudioSourceReference = log.fmodEventReference;
            audioObj.GetComponent<AudioLogMapObject>().group = log.group;
        }
    }

    private void SpawnPlayerCreatedAudioLogs(List<PlayerAudioLogData> logs)
    {
        foreach (PlayerAudioLogData log in logs)
        {
            LatLng latLng = new LatLng(log.latitude, log.longitude);
            GameObject audioMapObj = Instantiate(
                playerCreatedAudioLogMapObject,
                lightshipMap.LatLngToScene(in latLng),
                Quaternion.identity
            );
            PlayerAudioLogMapObject playerAudioLog = audioMapObj.GetComponent<PlayerAudioLogMapObject>();
            playerAudioLog.latitude = log.latitude.ToString();
            playerAudioLog.longitude = log.longitude.ToString();
            playerAudioLog.filename = log.filename;
            playerAudioLog.username = log.username;
            playerAudioLog.HandleSetup();
        }
    }

    private void TestSpawnExpedition()
    {
        LatLng latLng1 = new LatLng(47.64911685880216, -122.34881377439586);
        Instantiate(expeditionMapObject, lightshipMap.LatLngToScene(in latLng1), Quaternion.identity);
    }

    // todo: make this public in util file
    IEnumerator ExecuteAfterTime(float time, Action callback)
    {
        yield return new WaitForSeconds(time);

        callback();
    }

    // TODO implement someday
    /*private void InitializeMapPOIExperiences()
    {
        // Initialize maptile feature builders
        arExperienceBuilders.ForEach(builder => builder.Initialize(lightshipMap));
        _mapTileObjectPool = new ObjectPool<MapTileObject>(
                _mapTilePrefab.GetComponent<MapTileObject>(),
                mapTileObject => mapTileObject.Initialize(arExperienceBuilders),
                mapTileObject => mapTileObject.Release()
        );
    }*/

    public void AddMapExperienceToCollection(int id)
    {
        mapExperiencesCollection.Add("TestMapCompleteEvent", id);
    }
}
