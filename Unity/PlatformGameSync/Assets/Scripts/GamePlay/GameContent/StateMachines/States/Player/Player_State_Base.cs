using FixMath.NET;
using GamePlay.StateMachine;
using UnityEngine;

public class Player_State_Base : EntityState {
    protected const string kStrBool_Idle = "isIdle";
    protected const string kStrBool_Move = "isMove";
    protected const string kStrBool_JumpFall = "isJumpFall";
    protected const string kStrFloat_yVelocity = "yVelocity";

    protected LogicActor_Player LogicPlayer;
    protected RenderObject_Player RenderPlayer;
    protected InputSystem_Player InputPlayer;
    protected Animator Animator;

    public Player_State_Base(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(stateMachine, stateName) {
        LogicPlayer = logicPlayer;
        RenderPlayer = renderPlayer;
        InputPlayer = logicPlayer.InputSystem;
        Animator = renderPlayer.Animator;
    }


    protected override void OnEnter() { }

    public override void LogicFrameUpdate() {
        Animator.SetFloat(kStrFloat_yVelocity, (float)LogicPlayer.BaseColliderLogic.entity.LinearVelocity.Y);
    }

    public override void Exit() { }
}