using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioLog/Chosen Audio Log Experience")]
public class ChosenAudioLogExperienceSO : ScriptableObject
{
    [SerializeField] public PlayerAudioLogData chosenAudioLog;
}