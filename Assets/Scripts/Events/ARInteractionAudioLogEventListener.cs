using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ARInteractionAudioLogEvent : UnityEvent<double, string, Vector3>
{

}

public class ARInteractionAudioLogEventListener : MonoBehaviour
{
    [SerializeField] private ARInteractionAudioLogEventChannelSO _channel = default;

    public ARInteractionAudioLogEvent OnEventRaised;

    private void OnEnable()
    {
        if (_channel != null)
        {
            _channel.OnEventRaised += Respond;
        }
    }

    private void OnDisable()
    {
        if (_channel != null)
        {
            _channel.OnEventRaised -= Respond;
        }
    }

    private void Respond(double id, string name, Vector3 position)
    {
        if (OnEventRaised != null)
        {
            Debug.LogFormat("ARInteractionAudioLogEvent Raised: ", id.ToString(), " ", name, " ", position.ToString());
            OnEventRaised.Invoke(id, name, position);
        }
    }
}
