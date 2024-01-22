using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HandleFmodEvent : MonoBehaviour
{
    [SerializeField]
    private string fmodEventName;

    public void PlayFmodEvent()
    {
        if (fmodEventName != null)
        {
            var audioEvent = RuntimeManager.CreateInstance(fmodEventName);
            audioEvent.start();
            audioEvent.release();
        }
    }
}
