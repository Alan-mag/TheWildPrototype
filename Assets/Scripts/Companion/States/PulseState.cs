using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseState : IState
{
    private CompanionController companion;

    public PulseState(CompanionController companion)
    {
        this.companion = companion;
    }

    public void Enter()
    {
        Debug.Log("Enter Pulse State");
        // Set Animation Trigger //
        companion.CompanionAnimator.SetTrigger("StartPulse");
    }

    public void Execute()
    {
        if (companion.IsPulsing)
        {
            // Handle setup VFX //
            companion.pulseOrb.SetActive(true);
            // Other Behaviors //
        }

        // Transitions //
        if (companion.IsScanning)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.scanState);
        }

        if (companion.IsReceiving)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.receiveState);
        }

        if (companion.IsIdle)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.idleState);
        }
    }

    public void Exit()
    {
        // Clear VFX //
        companion.pulseOrb.SetActive(false);
    }
}
