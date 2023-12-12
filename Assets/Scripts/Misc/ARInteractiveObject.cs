using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 For handling event listening on AR Interaction Channel.
 Attach this script to any AR object that can be interacted
 with by companion character.

 Todo: I could create a channel for each type of interaction object,
 then pass name, vector3 position, and id -- should be able to then
 play the correct experience [I could also pass through whatever other 
 data I need, like maybe message for audio log?]
 */

// maybe make this interface?
// all ar interaction objects should have response
// but that response will have event invocation with variable args
public class ARInteractiveObject : MonoBehaviour
{
    [SerializeField] ARInteractionEventChannelSO _arInteractionEventChannel = default;
    [SerializeField] ARInteractionAudioLogEventChannelSO _arInteractionAudioLogEventChannel = default;
    // [SerializeField] ARInteractionEventChannelSO _arInteractionHistoricalImageEventChannel = default; // todo

    // may be able to handle this better when you have interface?
    public double id;
    public ARInteractionEvent OnEventRaised;
    public ARInteractionAudioLogEvent OnEventRaisedAudioLog;
    // public ARInteractionHistoricalImageEvent OnEventRaisedHistoricalImage; // todo

    private void OnEnable()
    {
        if (_arInteractionEventChannel != null)
        {
            _arInteractionEventChannel.OnEventRaised += Respond;
        }

        if (_arInteractionAudioLogEventChannel != null)
        {
            _arInteractionAudioLogEventChannel.OnEventRaised += RespondAudioLog;
        }
    }

    private void OnDisable()
    {
        if (_arInteractionEventChannel != null)
        {
            _arInteractionEventChannel.OnEventRaised -= Respond;
        }

        if (_arInteractionAudioLogEventChannel != null)
        {
            _arInteractionAudioLogEventChannel.OnEventRaised -= RespondAudioLog;
        }
    }

    // needs conditions:
    // either make an event channel for each object interaction

    // or have script that listens and runs appropriate script according
    // to object name or something?
    private void Respond(string name, Vector3 position)
    {
        if (OnEventRaised != null)
        {
            Debug.Log("ARInteractionEvent Raised: " + name + " " + position.ToString());
            OnEventRaised.Invoke(name, position);
        }
    }

    private void RespondAudioLog(double id, string name, Vector3 position)
    {
        if (OnEventRaisedAudioLog != null)
        {
            Debug.Log("ARInteractionEvent Raised: " + " " + id + " " + name + " " + position.ToString());
            OnEventRaisedAudioLog.Invoke(id, name, position);
            SceneManager.LoadScene("Expd_AudioLog_Player");
        }
    }
}
