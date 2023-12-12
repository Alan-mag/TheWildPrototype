using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanState : IState
{
    private CompanionController companion;

    public ScanState(CompanionController companion)
    {
        this.companion = companion;
    }

    public void Enter()
    {
        Debug.Log("Enter Scan State");
        // Set Animation Trigger //
        companion.CompanionAnimator.SetTrigger("StartScan");
    }

    public void Execute()
    {
        if (companion.IsScanning)
        {
            // Handle setup VFX //
            companion.scanSurvey.SetActive(true);
            // Other Behaviors //
        }

        // Transitions //
        if (companion.IsPulsing)
        {
            companion.CompanionStateMachine.TransitionTo(companion.CompanionStateMachine.pulseState);
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
        companion.scanSurvey.SetActive(false);
    }
}
