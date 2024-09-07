using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalCollectionItem : MonoBehaviour
{
    // Store Single SignalData info, reference to currentSignalSO, scene change ahndler
    // on selected, make this signal Data, current signalso,
    // change scene
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
