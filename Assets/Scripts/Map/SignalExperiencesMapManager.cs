using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalExperiencesMapManager : MonoBehaviour
{
    // check db,if there are signals in /community_signals 
    // add to some persistent collection on script
    // [create model for object, including creator and puzzle data]
    // when spawning a signal on map, first check if that collection
    // has any values in it - use that to spawn signal on map
    // that signalmapobject should have persistent data for puzzle value
    // when opening puzzle solution scene, use persistent data on current
    // signal puzzle info to populate puzzle,
    // also have space for player name who created that puzzle

    // once finished, create another script and repeat steps for 3d puzzle sphere

    // once finished repeat steps for audio log, although that will need lat lng and
    // some type of audio recording (will need to figure out how that is done)
    private string userId;
    private DatabaseReference userStatsDbReference;

    [SerializeField] List<SignalData> signalCollection;

    [SerializeField] SignalMapExperiencesManagerSO signalMapExperienceSO;

    private void Awake()
    {
        if (PlayerPrefs.GetString("user_id") != null)
        {
            userId = PlayerPrefs.GetString("user_id");
            userStatsDbReference = FirebaseDatabase.DefaultInstance.GetReference($"/community_signals/");
        }
    }

    private void Start()
    {
        Debug.Log("SignalExperiencesMapManager Start");
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

                }
                else
                {
                    // add each signal to collection
                    foreach (DataSnapshot signalFirebaseObject in snapshot.Children)
                    {
                        SignalData signalData = new SignalData(); // isn't doing anything
                        // need to add sequence to signalData.sequence
                        foreach(DataSnapshot sequenceUnit in signalFirebaseObject.Children)
                        {
                            var sequenceInt = Int32.Parse(sequenceUnit.Value.ToString());
                            signalData.sequence.Add(sequenceInt); // sequence 
                        }
                        // signalData.creatorName = PlayerPrefs.GetString("user_id");
                        signalCollection.Add(signalData);
                        signalMapExperienceSO.signalCollection.Add(signalData);
                    }
                }
            };
        });

    }
}
