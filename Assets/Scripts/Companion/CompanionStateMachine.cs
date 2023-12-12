using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CompanionStateMachine
{
    public IState CurrentState { get; set; }

    public IdleState idleState;
    public ScanState scanState;
    public ReceiveState receiveState;
    public PulseState pulseState;

    public event Action<IState> stateChanged;

    public CompanionStateMachine(CompanionController companion)
    {
        this.idleState = new IdleState(companion);
        this.scanState = new ScanState(companion);
        this.receiveState = new ReceiveState(companion);
        this.pulseState = new PulseState(companion);
    }

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();

        stateChanged?.Invoke(state);
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();

        stateChanged?.Invoke(nextState);
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Execute();
        }
    }
}
