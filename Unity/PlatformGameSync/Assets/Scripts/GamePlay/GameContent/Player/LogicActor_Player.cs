using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.StateMachine;
using UnityEngine;

public class LogicActor_Player : LogicActor {
    public StateMachine _stateMachine = null;

    private EntityState _idleState = null;

    public bool IsLocalActor { get; private set; }

    public override void OnCreate() {
        base.OnCreate();
    }

    public override void OnLogicFrameUpdate() {
        base.OnLogicFrameUpdate();
    }

    public override void OnDestory() {
        base.OnDestory();
    }

    public void SetIsLocalPlayer(bool v) {
        IsLocalActor = v;
    }

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