using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.StateMachine {
    public class StateMachine {
        public EntityState currentState { get; private set; }

        public void Init(EntityState initState) {
            currentState = initState;
            initState.Enter();
        }

        public void ChangeState(EntityState newState) {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void UpdateActiveState() {
            currentState?.Update();
        }
    }
}