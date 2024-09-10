using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SphereCollectionItem : MonoBehaviour
{
    [SerializeField] public PuzzleSphereInformation itemPuzzleData;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private ChosenSphereExperienceSO chosenSphereExperienceSO;

    public void HandleItemClick()
    {
        if (itemPuzzleData != null && chosenSphereExperienceSO != null)
        {
            chosenSphereExperienceSO.chosenSpherePuzzle = itemPuzzleData;
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("SignalCollectionItem:: " + "itemSignalData or chosenSignalExperienceSO was null");
        }
    }
}
