using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaygroundInteractiveObject : MonoBehaviour
{
    [SerializeField] ARInteractionEventChannelSO _arInteractionEventChannel = default;

    public ARInteractionEvent OnEventRaised;

    private void OnEnable()
    {
        if (_arInteractionEventChannel != null)
        {
            _arInteractionEventChannel.OnEventRaised += Respond;
        }
    }

    private void OnDisable()
    {
        if (_arInteractionEventChannel != null)
        {
            _arInteractionEventChannel.OnEventRaised -= Respond;
        }
    }

    private void Respond(string name, Vector3 position)
    {
        if (OnEventRaised != null)
        {
            Debug.Log("ARInteractionEvent Raised: " + name);
            OnEventRaised.Invoke(name, position);
        }
    }
}
