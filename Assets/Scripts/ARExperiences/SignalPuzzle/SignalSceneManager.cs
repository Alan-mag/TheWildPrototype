using Niantic.Lightship.Maps.Unity.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] TextMeshProUGUI creatorNameText;

    [Header("Sequence information")]
    [SerializeField] List<int> guessedTones;
    [SerializeField] List<int> signalSequence = new List<int> (3);
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

    [SerializeField] ChosenSignalExperienceSO chosenSignalExperienceSO;
    [SerializeField] SignalMapExperiencesManagerSO signalMapExperienceSO;

    // scene handlers
    SceneChangeHandler sceneChangeHandler;
    
    [SerializeField] ExpeditionLevelHandler expeditionSceneHandler;

    [Header("Progression")]
    [SerializeField] GameProgressionSO gameProgressionSO;

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

    // TODO: create something similar for Puzzle Sphere 
    private void Start()
    {
        // Todo: add a 'chosen signal puzzle' SO
        // that will bypass this signal Collection check here
        // that way it can be set from inventory or tosolve, and 
        // if will be easy to select a specific puzzle vs general community puzzles
        if (!chosenSignalExperienceSO.chosenSignal.sequence.IsEmpty())
        {
            signalSequence.AddRange(chosenSignalExperienceSO.chosenSignal.sequence);
            creatorNameText.text = chosenSignalExperienceSO.chosenSignal.creatorName;

            UpdateGuessIndicatorsActive();
            audioManager.PopulateAudioClips();
            weaveManager.PopulateThreads();
            chosenSignalExperienceSO.chosenSignal = null;
        } 
        else if (signalMapExperienceSO.signalCollection.Count > 0)
        {
            // use signal sequence from SO todo: randomize which gets picked
            System.Random rand = new System.Random();
            int index = rand.Next(signalMapExperienceSO.signalCollection.Count);
            var signalCollectionArray = signalMapExperienceSO.signalCollection.ToArray();
            for (int i = 0; i <= index; i++)
            {
                if (i != index)
                {

                }
                else
                {
                    signalSequence.AddRange(signalMapExperienceSO.signalCollection[i].sequence);
                    creatorNameText.text = signalMapExperienceSO.signalCollection[i].creatorName;
                }
            }
            UpdateGuessIndicatorsActive();
            audioManager.PopulateAudioClips();
            weaveManager.PopulateThreads();
        }
        else
        {
            // generate random sequence
            GenerateRandomSequence();
        }
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

    private void GenerateRandomSequence()
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
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Adventurer, (float)gameProgressionSO.adventurerExperienceSmall);
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Explorer, (float)gameProgressionSO.adventurerExperienceMedium);
            _incrementedExp = true;
        }
    }
    /********************/
}
