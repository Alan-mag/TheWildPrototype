using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStateMachine
    {
        public IState CurrentState { get; set; }

        public IdleState idleState;
        public WalkingState walkingState;

        public event Action<IState> stateChanged;

        public PlayerStateMachine(PlayerController player)
        {
            this.idleState = new IdleState(player);
            this.walkingState = new WalkingState(player);
        }

        public void Initialize(IState state)
        {
            CurrentState = state;
            state.Enter();
            stateChanged?.Invoke(state);
        }

        public void TransistionTo(IState nextState)
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
}
