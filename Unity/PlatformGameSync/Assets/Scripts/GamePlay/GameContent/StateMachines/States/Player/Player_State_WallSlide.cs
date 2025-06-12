using FixMath.NET;
using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UVector2 = UnityEngine.Vector2;

public class Player_State_WallSlide : Player_State_Base {
    public Player_State_WallSlide(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine)
        : base(LogicActor_Player.kStrBool_WallSlide, logicPlayer, renderPlayer, stateMachine) { }


    protected override void OnEnter() {
        base.OnEnter();
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (LogicPlayer.groundDetected) {
            _stateMachine.ChangeState(this.LogicPlayer.StateIdle);
        }
    }

    public override void Exit() {
        base.Exit();
    }
}