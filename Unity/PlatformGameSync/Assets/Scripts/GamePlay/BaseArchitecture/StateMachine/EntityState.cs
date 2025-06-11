using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.StateMachine {
    public abstract class EntityState {
        protected StateMachine _stateMachine;
        public string StateName { get; private set; }

        public EntityState(StateMachine stateMachine, string stateName) {
            _stateMachine = stateMachine;
            StateName = stateName;
        }


        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();
    }
}