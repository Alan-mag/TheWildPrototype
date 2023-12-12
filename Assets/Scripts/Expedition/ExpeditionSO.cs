using UnityEngine;
using UnityEngine.SceneManagement;


// Could probably generalize this -->
// create model that can take in various 'stages'
// options:
// type of puzzle
// 3d sphere, signal, audio log, historical image etc.
// if audio log --> message
// if historical image --> get image asset for that step
// if open ended stage or sequence stage
// etc.

[CreateAssetMenu(menuName = "Expeditions/Expedition Data")]
public class ExpeditionSO : ScriptableObject
{
    [Header("Level information")]
    [SerializeField] private int totalLevels;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int pathTaken = 0;

    [Header("Companion lines")]
    [SerializeField] private string audioLogMessage;
    [SerializeField] private string scannerMessage;
    [SerializeField] private string historicalImageMessage;
    [SerializeField] private string finalStepMessage;

    [Header("Images")]
    [SerializeField] private Texture2D historicalImage;

    public int TotalLevels
    {
        get { return totalLevels; }
        set { totalLevels = value; }
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }

    public string AudioLogMessage
    {
        get { return audioLogMessage; }
    }

    public string ScannerMessage
    {
        get { return scannerMessage; }
    }

    public string HistoricalImageMessage
    {
        get { return historicalImageMessage; }
    }

    public string FinalStepMessage
    {
        get { return finalStepMessage; }
    }

    public int GetPathTaken
    {
        get { return pathTaken; }
    }

    public void SetPathTaken(int pathValue)
    {
        pathTaken = pathValue;
    }

    // eventually should handle this in event channel
    /*public void CompleteStage()
    {
        if (CurrentLevel == totalLevels) // whatever is last
        {
            // done -- return
            CurrentLevel = 0;
            SceneManager.LoadScene("MapTest");
        }
        else
        {
            CurrentLevel = CurrentLevel + 1; // todo will fix with overhaul
            SceneManager.LoadScene("ExpeditionScene");
        }
    }*/
}
