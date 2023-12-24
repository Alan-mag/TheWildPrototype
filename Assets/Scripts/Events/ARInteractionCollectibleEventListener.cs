using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ARInteractionCollectibleEvent : UnityEvent<GameObject>
{

}

public class ARInteractionCollectibleEventListener : MonoBehaviour
{
    [SerializeField] private ARInteractionCollectibleEventChannelSO _channel = default;

    public ARInteractionCollectibleEvent OnEventRaised;

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

    private void Respond(GameObject gObject)
    {
        if (OnEventRaised != null)
        {
            Debug.LogFormat("ARInteractionCollectibleEvent Raised");
            OnEventRaised.Invoke(gObject);
        }
    }
}
