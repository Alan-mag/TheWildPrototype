using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalCollectionItem : MonoBehaviour
{
    [SerializeField] public SignalData itemSignalData;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private ChosenSignalExperienceSO chosenSignalExperienceSO;

    public void HandleItemClick()
    {
        if (itemSignalData != null && chosenSignalExperienceSO != null)
        {
            chosenSignalExperienceSO.chosenSignal = itemSignalData;
            SceneManager.LoadScene(sceneToLoad);
        } 
        else
        {
            Debug.Log("SignalCollectionItem:: " + "itemSignalData or chosenSignalExperienceSO was null");
        }
    }
}
