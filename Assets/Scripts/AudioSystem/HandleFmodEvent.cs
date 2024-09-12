using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HandleFmodEvent : MonoBehaviour
{
    // [SerializeField]
    // private string fmodEventName; // VO/HQ Expedition Intro
    [SerializeField] private ChosenAudioLogExperienceSO chosenAudioLogExperienceSO;

    public void PlayFmodEvent()
    {
        if (AudioLogInfo.FmodAudioSourceReference != null && 
            chosenAudioLogExperienceSO.chosenAudioLog.filename == null)
        {
            var audioEvent = RuntimeManager.CreateInstance("event:" + AudioLogInfo.FmodAudioSourceReference);
            audioEvent.start();
            audioEvent.release();
        }

        AudioLogInfo.FmodAudioSourceReference = null;
        AudioLogInfo.Description = null;
        AudioLogInfo.Title = null;
    }

    public void PlayFmodEventFromReference(string fmodEventName)
    {
        var audioEvent = RuntimeManager.CreateInstance("event:" + fmodEventName);
        audioEvent.start();
        audioEvent.release();
    }
}
