using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Todo:
/// <summary>
/// first, the tutorial should only show up when player has no experience (check stats from db on start)
/// 
/// Tutorial scene goals:
/// - teach player signal scene puzzle (just collect stuff, this is minimum)
/// - could also teach: select audio log, solve 3D puzzle
/// - the reward: map showing points of interest experiences, and location specific (geotagged created) experiences
/// 
/// </summary>
public class TutorialManager : MonoBehaviour
{
    /*[Header("UI")]
    [SerializeField] private GameObject tutorialCanvas; // audio log ui, signal scene ui, puzzle sphere help & scene dialogue
    [SerializeField] private GameObject completedCanvas;
    [SerializeField] private GameObject audioCanvas;
    [SerializeField] private GameObject signalCanvas;
    [SerializeField] private TextMeshProUGUI narrativeText;
    
    [SerializeField] private GameObject signalGameObjectParent;
    [SerializeField] private GameObject audioLogObject;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject puzzleTutorialObject;*/


    [SerializeField] private GameObject completeButton;
    [SerializeField] private FirebaseManager firebaseManager;

    public delegate void AudioCallback();
    private int numberOfSignals = 3;

    public void HandleFinishedTutorial()
    {
        completeButton.SetActive(true);
        firebaseManager.AddFirstExperience(EXPERIENCE_TYPE.Adventurer, 1);
    }

    /*private void Start()
    {
        narrativeText.text = "<<Emergency Impact::Detected>>"; // setup first text to screen
    }

    #region Audio Tutorial
    public void OnSelectAudioLog()
    {
        audioCanvas.SetActive(true);
        Destroy(audioLogObject); // for now, destroy
    }

    public void PlayAudioFromAudioLog()
    {
        // audioSource.Play();
        PlayAudioWithCallback(audioSource.clip, SetupSignalTutorial);
        audioCanvas.SetActive(false);
        SetNarrativeText("Robotic log playing...we need to collect signals in the area to start repairs.");
    }

    public void PlayAudioWithCallback(AudioClip clip, AudioCallback callback)
    {
        audioSource.PlayOneShot(clip);
        StartCoroutine(DelayedCallback(clip.length, callback));
    }

    private IEnumerator DelayedCallback(float time, AudioCallback callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public void StopAudioFromAudioLog()
    {
        audioSource.Stop();
    }
    #endregion


    // todo: spawn signals around area
    // update text to explain puzzle
    // once solved, disable ui and spawn puzzle sphere
    // repeat
    // once done - change state of robot, update text (now you have a map!)
    // rough tutorial done

    #region Signal Tutorial
    private void SetupSignalTutorial()
    {
        signalGameObjectParent.SetActive(true);
        signalCanvas.SetActive(true);
        SetNarrativeText("Signal tutorial text...");
    }

    private void SetNarrativeText(string text)
    {
        narrativeText.text = text;
    }

    public void CollectThread(GameObject signal)
    {
        Destroy(signal);
        numberOfSignals--;
        if (numberOfSignals <= 0)
        {
            signalCanvas.SetActive(false);
            SetupPuzzleTutorial();
        }
    }

    #endregion

    #region Puzzle Tutorial
    private void SetupPuzzleTutorial()
    {
        puzzleTutorialObject.SetActive(true);
        SetNarrativeText("Puzzle tutorial text...");
    }

    // todo: add puck stuff
    #endregion

    #region Finished Tutorial
    public void HandleFinishedTutorial()
    {
        puzzleTutorialObject.SetActive(false);
        completedCanvas.SetActive(true);
        firebaseManager.AddFirstExperience(EXPERIENCE_TYPE.Adventurer, 1);
    }
    #endregion*/
}
