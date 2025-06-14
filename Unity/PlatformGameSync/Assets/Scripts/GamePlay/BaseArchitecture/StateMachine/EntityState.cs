using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;

namespace GamePlay.StateMachine {
    public abstract class EntityState {
        protected StateMachine _stateMachine;
        public string StateName { get; private set; }

        public EntityState(StateMachine stateMachine, string stateName) {
            _stateMachine = stateMachine;
            StateName = stateName;
        }


        public void Enter() {
            OnEnter();
        }

        protected abstract void OnEnter();

        public abstract void LogicFrameUpdate();

        public abstract void Exit();
    }
}