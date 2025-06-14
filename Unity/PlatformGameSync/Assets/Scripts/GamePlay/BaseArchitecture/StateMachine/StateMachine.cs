using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;

namespace GamePlay.StateMachine {
    public class StateMachine {
        public Fix64 deltaTime { get; private set; }
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

        public void Update(Fix64 deltaTime) {
            this.deltaTime = deltaTime;
            currentState?.LogicFrameUpdate();
        }
    }
}