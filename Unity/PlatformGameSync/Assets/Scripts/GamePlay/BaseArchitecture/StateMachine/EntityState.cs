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


        public void Enter() {
            OnEnter();
            LogicFrameUpdate(); // Enter后立即更新一帧, 避免迟滞
        }

        protected abstract void OnEnter();
        
        public abstract void LogicFrameUpdate();

        public abstract void Exit();
    }
}