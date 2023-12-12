using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/AR Interaction Event Channel")]
public class ARInteractionEventChannelSO : DescriptionBaseSO
{
    public UnityAction<string, Vector3> OnEventRaised;

    public void RaiseEvent(string name, Vector3 position)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(name, position);
    }
}
