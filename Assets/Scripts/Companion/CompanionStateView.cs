using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompanionController))]
public class CompanionStateView : MonoBehaviour
{
    private CompanionController companion;
    private CompanionStateMachine companionStateMachine;

    public event Action PulseEnvironmentTriggered;

    private void Awake()
    {
        companion = GetComponent<CompanionController>();

        /*companionStateMachine = companion.CompanionStateMachine; // todo this StateMachine is null - setting it in Start() now

        companionStateMachine.stateChanged += OnStateChanged;*/
    }

    private void Start()
    {
        companionStateMachine = companion.CompanionStateMachine; // todo this StateMachine is null

        companionStateMachine.stateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        companionStateMachine.stateChanged -= OnStateChanged;
    }

    private void OnStateChanged(IState state)
    {
        // Todo: implement any other onstatechange functionality here
        Debug.Log("OnStateChanged in CompanionStateView class");

        if (companion.IsPulsing)
        {
            PulseEnvironmentTriggered?.Invoke();
        }
    }
}
