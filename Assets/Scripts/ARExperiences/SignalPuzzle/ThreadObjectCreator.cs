using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadObjectCreator : MonoBehaviour
{
    [SerializeField] SignalCreationManager signalCreationManager;
    [SerializeField] int signalNumber;

    private void OnMouseDown()
    {
        signalCreationManager.SpawnSignal(signalNumber);
    }
}
