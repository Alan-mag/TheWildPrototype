using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveState : IState
{
    private CompanionController companion;

    public ReceiveState(CompanionController companionController)
    {
        this.companion = companionController;
    }

    public void Enter()
    {
        // Set Animation Trigger //
        companion.CompanionAnimator.SetTrigger("StartReceiving");
    }

    public void Execute()
    {
        if (companion.IsReceiving)
        {
            // Handle setup VFX //
            companion.receiverBeam.SetActive(true);
            // Other Behaviors //
        }


        // Transitions //
        if (companion.IsScanning)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.scanState);
        }

        if (companion.IsPulsing)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.pulseState);
        }

        if (companion.IsIdle)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.idleState);
        }
    }

    public void Exit()
    {
        // Clear VFX //
        companion.receiverBeam.SetActive(false);
    }
}
