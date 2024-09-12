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
            chosenAudioLogExperienceSO.chosenAudioLog.latitude = itemAudioLogData.latitude;
            chosenAudioLogExperienceSO.chosenAudioLog.longitude = itemAudioLogData.longitude;
            chosenAudioLogExperienceSO.chosenAudioLog.filename = itemAudioLogData.filename;

            StartCoroutine(ChangeScene());
        }
        else
        {
            Debug.Log("AudioLogCollectionItem:: " + "itemAudioLogData or chosenAudioLogExperienceSO was null");
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneToLoad);
    }
}
