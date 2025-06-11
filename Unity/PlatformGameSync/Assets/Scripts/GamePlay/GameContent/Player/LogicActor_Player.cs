using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.StateMachine;
using UnityEngine;

public partial class LogicActor_Player {
    RenderObject_Player _renderPlayer => RenderObject as RenderObject_Player;
    public bool IsLocalActor { get; private set; }

    public InputSystem_Player InputSystem { get; private set; }

    private void InitPlayeColliderAttrs() {
        // this.BaseColliderLogic.entity.freezeRotation_X = true;
        // this.BaseColliderLogic.entity.freezeRotation_Y = true;
        // this.BaseColliderLogic.entity.freezeRotation_Z = true;
        // this.BaseColliderLogic.entity.freezePos_Z = true;
    }


    private void InitInputSystem() {
        InputSystem = new InputSystem_Player();
        InputSystem.Enable();
    }

    public override void OnCreate() {
        this.ColliderType = BEPU_ColliderType.Box;
        base.OnCreate();
        InitPlayeColliderAttrs();
        InitInputSystem();
        InitStateMachine();
    }

    public override void OnLogicFrameUpdate() {
        base.OnLogicFrameUpdate();
        LogicFrameUpdate_StateMachine();
    }

    public override void OnDestory() {
        InputSystem.Disable();
        base.OnDestory();
    }

    public void SetIsLocalPlayer(bool v) {
        IsLocalActor = v;
    }
}