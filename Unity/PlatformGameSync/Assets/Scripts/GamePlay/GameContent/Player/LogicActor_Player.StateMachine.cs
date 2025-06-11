using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.StateMachine;
using UnityEngine;

public partial class LogicActor_Player : LogicActor {
    public StateMachine stateMachine { get; private set; }

    public Player_IdleState state_Idle { get; private set; }
    public Player_MoveState state_Move { get; private set; }

    private void InitStateMachine() {
        stateMachine = new StateMachine();
        state_Idle = new(this, _renderPlayer, stateMachine, "Player-Idle");
        state_Move = new(this, _renderPlayer, stateMachine, "Player-Move");
        stateMachine.Init(state_Idle);
    }

    private void LogicFrameUpdate_StateMachine() {
        stateMachine.UpdateActiveState();
    }
}