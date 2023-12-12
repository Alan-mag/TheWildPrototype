using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpeditionLevelHandler : MonoBehaviour
{
    // [SerializeField] private IntEventChannelSO _onCompletedStage = default;
    [SerializeField] ExpeditionSO expeditionData = default;

    public void CompleteStage()
    {
        expeditionData.CurrentLevel = expeditionData.CurrentLevel + 1;

        if (expeditionData.CurrentLevel == expeditionData.TotalLevels)
        {
            expeditionData.CurrentLevel = 0;
            expeditionData.SetPathTaken(0);
            SceneManager.LoadScene("MapTest");
        } else
        {
            SceneManager.LoadScene("ExpeditionScene");
        }
    }
}
