using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private CompanionController companion;
    public IdleState(CompanionController companionController)
    {
        this.companion = companionController; 
    }

    public void Enter()
    {
        Debug.Log("Enter Idle State");
        companion.CompanionAnimator.SetTrigger("StartIdle");
    }

    public void Execute()
    {

        // Transitions //
        if (companion.IsScanning)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.scanState);
        }

        if (companion.IsReceiving)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.receiveState);
        }

        if (companion.IsPulsing)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.pulseState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exit Idle State");
    }
}
