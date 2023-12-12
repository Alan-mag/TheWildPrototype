using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ARInteractionEvent : UnityEvent<string, Vector3>
{

}

public class ARInteractionEventListener : MonoBehaviour
{
    [SerializeField] private ARInteractionEventChannelSO _channel = default;

    public ARInteractionEvent OnEventRaised;

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

    private void Respond(string name, Vector3 position)
    {
        if (OnEventRaised != null)
        {
            Debug.LogFormat("ARInteractionEvent Raised: ",name, " ",position.ToString());
            OnEventRaised.Invoke(name, position);
        }
    }
}
