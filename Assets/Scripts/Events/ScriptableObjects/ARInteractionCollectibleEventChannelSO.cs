using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/AR Interaction Collectible Event Channel")]
public class ARInteractionCollectibleEventChannelSO : DescriptionBaseSO
{
    public UnityAction<GameObject> OnEventRaised;

    public void RaiseEvent(GameObject gObject)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(gObject);
        }
    }
}
