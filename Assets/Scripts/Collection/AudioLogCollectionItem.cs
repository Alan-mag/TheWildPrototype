using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioLogCollectionItem : MonoBehaviour
{
    [SerializeField] public PlayerAudioLogData itemAudioLogData;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private ChosenAudioLogExperienceSO chosenAudioLogExperienceSO;

    public void HandleItemClick()
    {
        if (itemAudioLogData != null && chosenAudioLogExperienceSO != null)
        {
            chosenAudioLogExperienceSO.chosenAudioLog = itemAudioLogData;
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("AudioLogCollectionItem:: " + "itemAudioLogData or chosenAudioLogExperienceSO was null");
        }
    }
}
