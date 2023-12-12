using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/AR Interaction Event Channel Audio Log")]
public class ARInteractionAudioLogEventChannelSO : ARInteractionEventChannelSO
{
    public new UnityAction<double, string, Vector3> OnEventRaised;

    public void RaiseEvent(double id, string name, Vector3 position)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(id, name, position);
        }
    }
}
