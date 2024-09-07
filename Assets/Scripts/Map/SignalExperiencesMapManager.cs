using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalExperiencesMapManager : MonoBehaviour
{
    private DatabaseReference communitySignalDatabaseReference;
    [SerializeField] List<SignalData> signalCollection;
    [SerializeField] SignalMapExperiencesManagerSO signalMapExperienceSO;

    private void Awake()
    {
        communitySignalDatabaseReference = FirebaseDatabase.DefaultInstance.GetReference($"/community_signals/");
    }

    private void Start()
    {
        communitySignalDatabaseReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("SignalExperiencesMapManager:: Error: unable to access community_signals db reference");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value == null)
                {
                    Debug.Log("SignalExperiencesMapManager:: Warning: no community_signal db objects to pull");
                }
                else
                {
                    foreach (DataSnapshot signalFirebaseObject in snapshot.Children)
                    {
                        SignalData signalData = JsonConvert.DeserializeObject<SignalData>(signalFirebaseObject.GetRawJsonValue());
                        signalCollection.Add(signalData);
                        signalMapExperienceSO.signalCollection.Add(signalData);
                    }
                }
            };
        });

    }
}
