using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.StateMachine;
using UnityEngine;

public class Player : MonoBehaviour {
    public StateMachine _stateMachine = null;

    private EntityState _idleState = null;

    private void Awake() {
        _stateMachine = new StateMachine();
        _idleState = new EntityState(_stateMachine, "Player-Idle");
    }

    private void Start() {
        _stateMachine.Init(_idleState);
    }

    private void Update() {
        _stateMachine.currentState.Update();
    }
}