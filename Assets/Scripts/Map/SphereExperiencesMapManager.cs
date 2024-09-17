using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SphereExperiencesMapManager : MonoBehaviour
{
    // Game Progression SO
    [Header("Character Progression")]
    [SerializeField] private GameProgressionSO gameProgressionSO;

    [SerializeField]
    List<PuzzleSphereInformation> puzzleSphereTargets;

    [SerializeField]
    SphereMapExperiencesManagerSO sphereMapExperiencesManagerSO;

    private DatabaseReference _databaseReference;

    private PuzzleSphereInformation _sphereInformationTest;

    private void Awake()
    {
        if (PlayerPrefs.GetString("user_id") != null)
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.GetReference($"/community_puzzles/");
        }
    }

    private void Start()
    {
        //add test 
        /*PuzzleSphereTarget t = new PuzzleSphereTarget(-0.07125645f, 0.253887653f, 1.11454189f);
        PuzzleSphereTarget u = new PuzzleSphereTarget(-0.241122082f, -0.0874981359f, 1.129848f);
        PuzzleSphereTarget v = new PuzzleSphereTarget(-0.201063752f, -0.03456563f, 0.7974294f);
        _sphereInformationTest = new PuzzleSphereInformation("Alan", new List<PuzzleSphereTarget>() { t,u,v });
        if (sphereMapExperiencesManagerSO.sphereInformationCollection.Count < 1)
        {
            sphereMapExperiencesManagerSO.sphereInformationCollection.Add(_sphereInformationTest);
        }*/
        // declaration

        // todo: add check if collection is not empty, then run fb call
        // execution block (can have multiple)



        Debug.Log("SphereExperiencesMapManager Start");
        
        // If player hasn't progressed to sphere puzzles, return
        if (gameProgressionSO.PlayerExplorerExperience < gameProgressionSO.explorerSecondTier) return;

        _databaseReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value != null)
                {
                    foreach (DataSnapshot targetSnapshot in snapshot.Children)
                    {
                        foreach (DataSnapshot targetInfo in targetSnapshot.Children)
                        {
                            PuzzleSphereInformation puzzleInformation = JsonConvert.DeserializeObject<PuzzleSphereInformation>(targetInfo.Value.ToString());
                            if (puzzleInformation != null)
                            {
                                puzzleSphereTargets.Add(puzzleInformation);
                                sphereMapExperiencesManagerSO.sphereInformationCollection.Add(puzzleInformation);
                                Debug.Log("creatorName: " + puzzleInformation.creatorName);
                                foreach (var target in puzzleInformation.puzzleSphereTarget)
                                {
                                    Debug.Log($"Target - x: {target.x}, y: {target.y}, z: {target.z}");
                                }
                            }
                            else
                            {
                                Debug.LogError("Deserialization returned null.");
                            }


                        }
                    }
                }
            }
        });
        
        // return block (spaces in between each)
    }
}
