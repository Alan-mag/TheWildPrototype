using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// for prototype:
// this class should generate a  random sequence of 4 integers [1 4 2 5 ]
// then use weavemanager and audio manager to setup scene with
// art assets spawning and sound playing that match the randomly generated sequence
public class SignalSceneManager : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] GameObject gameplayCanvas;

    [Header("Sequence information")]
    [SerializeField] List<int> guessedTones;
    [SerializeField] List<int> signalSequence = new List<int> { };
    [SerializeField] List<int> visualSignalSequence = new List<int> { };

    [Header("Guess Indicator Objects")]
    [SerializeField] public GameObject[] guessIndicators = new GameObject[] { };

    [Header("Art Assets")]
    [SerializeField] public GameObject[] threadObjects = new GameObject[] { };

    [Header("Audio Assets")]
    [SerializeField] public AudioClip[] audioClips = new AudioClip[] { };

    // todo refactor with delegates actions -- don't need these refs
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private WeaveManager weaveManager;

    // scene handlers
    SceneChangeHandler sceneChangeHandler;
    
    [SerializeField] ExpeditionLevelHandler expeditionSceneHandler;

    public delegate void OnSequenceGenerated();
    public static OnSequenceGenerated onSequenceGenerated;
    private bool _incrementedExp;


    private void Awake()
    {
        if (gameplayCanvas == null)
            throw new ArgumentException("Gameplay Canvas not set in SignalSceneManager");

        if (sceneChangeHandler == null)
        {
            sceneChangeHandler = gameObject.GetComponent<SceneChangeHandler>();
        }
    }

    private void Start()
    {
        GenerateSequence();
    }

    private void CheckIfCorrectSequence()
    {
        var isMatchingSequence = guessedTones.SequenceEqual(signalSequence);
        if (isMatchingSequence)
        {
            Debug.Log("correct sequence!");
            HandleSuccess();
        } 
    }

    private void GenerateSequence()
    {
        var numOfTones = UnityEngine.Random.Range(2, 5);
        for (int i = 0; i < numOfTones; i++)
        {
            signalSequence.Add(UnityEngine.Random.Range(0, 3));
        }
        UpdateGuessIndicatorsActive();
        // onSequenceGenerated?.Invoke(); // TODO should be done this way - need persistent manager too
        audioManager.PopulateAudioClips();
        weaveManager.PopulateThreads();

    }

    private void UpdateGuessIndicatorsActive()
    {
        for (int i = 0; i < signalSequence.Count; i++)
        {
            guessIndicators[i].SetActive(true);
        }  
    }

    private void UpdateGuessIndicatorAlpha(float value, int idx)
    {
        // get guess indicator image
        GameObject guessI = guessIndicators[idx];
        Image i = guessI.GetComponent<Image>();
        var tempColor = i.color;
        tempColor.a = value;
        i.color = tempColor;
    }

    private void ClearAllGuessIndicators()
    {
        for (int i = 0; i < guessedTones.Count; i++)
        {
            UpdateGuessIndicatorAlpha(0.2f, i);
        }
    }

    // public functions //

    public int[] GetSignalSequenceAsArray()
    {
        return signalSequence.ToArray();
    }

    public void RegisterGuessTone(int toneInt)
    {
        if (guessedTones.Count >= signalSequence.Count)
        {
            ClearAllGuessIndicators();
            guessedTones.Clear();
            guessedTones.Add(toneInt);
            UpdateGuessIndicatorAlpha(1.0f, guessedTones.Count - 1);
        }
        else
        {
            guessedTones.Add(toneInt);
            UpdateGuessIndicatorAlpha(1.0f, guessedTones.Count - 1);
            if (guessedTones.Count == signalSequence.Count)
            {
                CheckIfCorrectSequence();
            }
        }
    }

    public void CollectThreadObject(int threadNumber)
    {
        visualSignalSequence.Add(threadNumber);

        if (visualSignalSequence.Count == signalSequence.Count)
        {
            Debug.Log("collected all sequence!");
            HandleSuccess();
        }
    }

    public void HandleSuccess()
    {
        gameplayCanvas.SetActive(true);
        IncrementExp();
        // todo: will need a way to handle 
        // AR scenes either from map - or from expedition sequence
        if (expeditionSceneHandler != null)
        {
            expeditionSceneHandler.CompleteStage();
        } else
        {
            sceneChangeHandler.ChangeScene();
        }
    }

    /********************
     * Firebase test
     */

    private void IncrementExp()
    {
        if (!_incrementedExp)
        {
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Adventurer, 0.5f);
            _incrementedExp = true;
        }
    }
    /********************/
}
