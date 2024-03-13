using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HandleFmodEvent : MonoBehaviour
{
    // [SerializeField]
    // private string fmodEventName; // VO/HQ Expedition Intro

    public void PlayFmodEvent(string fmodEventName)
    {
        if (fmodEventName != null)
        {
            var audioEvent = RuntimeManager.CreateInstance("event:" + fmodEventName);
            audioEvent.start();
            audioEvent.release();
        }
    }
}
