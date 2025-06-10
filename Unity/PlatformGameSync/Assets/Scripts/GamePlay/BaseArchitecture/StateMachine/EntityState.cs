using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.StateMachine {
    public class EntityState {
        protected StateMachine _stateMachine;
        public string StateName { get; private set; }

        public EntityState(StateMachine stateMachine, string stateName) {
            _stateMachine = stateMachine;
            StateName = stateName;
        }


        public virtual void Enter() {
            Debug.LogError($"{StateName} enter state");
        }

        public virtual void Update() {
            Debug.LogError($"{StateName} update state ");
        }

        public virtual void Exit() {
            Debug.LogError($"{StateName} exit state ");
        }
    }
}