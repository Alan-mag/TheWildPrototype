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
    [SerializeField] private GameObject completeButton;
    [SerializeField] private FirebaseManager firebaseManager;

    public delegate void AudioCallback();

    public void HandleAddingTutorialExperience()
    {
        // completeButton.SetActive(true);
        firebaseManager.AddFirstExperience(EXPERIENCE_TYPE.Adventurer, 1);
    }
}
