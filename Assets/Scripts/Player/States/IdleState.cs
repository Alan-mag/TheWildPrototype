using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class IdleState : IState
    {
        private PlayerController player;

        public IdleState(PlayerController player)
        {
            this.player = player;
        }

         public void Enter()
        {

        }

        public void Execute()
        {
            // transisiton to walking
            // idle pose
            if (player.Speed < 0.1)
            {
                // player.PlayerAnimator.Play("BasicMotions@Idle01 - Idle02");
                player.PlayerAnimator.SetFloat("speed", player.Speed);
            }
            else
            {
                player.PlayerStateMachine.TransistionTo(player.PlayerStateMachine.walkingState);
            }
        }

        public void Exit()
        {

        }
    }
}
