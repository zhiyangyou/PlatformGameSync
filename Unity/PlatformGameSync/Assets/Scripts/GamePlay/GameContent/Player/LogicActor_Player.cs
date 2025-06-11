using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.StateMachine;
using UnityEngine;

public partial class LogicActor_Player {
    RenderObject_Player _renderPlayer => RenderObject as RenderObject_Player;
    public bool IsLocalActor { get; private set; }
    public override void OnCreate() {
        base.OnCreate();
        InitStateMachine();
    }

    public override void OnLogicFrameUpdate() {
        base.OnLogicFrameUpdate();
        LogicFrameUpdate_StateMachine();
    }

    public override void OnDestory() {
        base.OnDestory();
    }

    public void SetIsLocalPlayer(bool v) {
        IsLocalActor = v;
    }
}