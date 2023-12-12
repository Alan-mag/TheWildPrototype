using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WalkingState : IState
    {
        private PlayerController player;
        public WalkingState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {

        }

        public void Execute()
        {
            // walking animations here
            if (player.Speed < 0.1)
            {
                player.PlayerStateMachine.TransistionTo(player.PlayerStateMachine.idleState);
            }
            else
            {
                //player.PlayerAnimator.Play("BasicMotions@Walk01 - Forwards");
                player.PlayerAnimator.SetFloat("speed", player.Speed);
            }
        }

        public void Exit()
        {

        }
    }
}
