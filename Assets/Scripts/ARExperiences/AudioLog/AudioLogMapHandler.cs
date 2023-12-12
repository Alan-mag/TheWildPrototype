using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLogMapHandler : MonoBehaviour
{
    public string thisAudioLogMessage;

    public string GetAudioLogMessage()
    {
        return thisAudioLogMessage;
    }
    public void SetObjectMessageLog(string msg)
    {
        thisAudioLogMessage = msg;
    }
}
