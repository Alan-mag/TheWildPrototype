using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.Threading;

public class HandleFmodEvent : MonoBehaviour
{
    // [SerializeField]
    // private string fmodEventName; // VO/HQ Expedition Intro
    [SerializeField] private ChosenAudioLogExperienceSO chosenAudioLogExperienceSO;

    private void Start()
    {
        Debug.Log("Audio Log Info:");
        Debug.Log(AudioLogInfo.Title);
        Debug.Log(AudioLogInfo.Description);
        Debug.Log(AudioLogInfo.FmodAudioSourceReference);
        Debug.Log("ChosenAudioLogSO Info:");
        Debug.Log(chosenAudioLogExperienceSO.chosenAudioLog.filename);
    }

    public void PlayFmodEvent()
    {
        // this might be messing up playing community audio logs - I don't know 
        // if we are ever clearning chosenAudioLog SO
        if (AudioLogInfo.FmodAudioSourceReference != null 
            // && chosenAudioLogExperienceSO.chosenAudioLog.filename == null
            )
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

    // Todo: stop fmod event
    private void OnDestroy()
    {
        FMODUnity.RuntimeManager.MuteAllEvents(true);
    }
}
