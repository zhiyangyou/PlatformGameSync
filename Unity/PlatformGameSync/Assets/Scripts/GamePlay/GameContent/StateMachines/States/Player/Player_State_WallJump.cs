using FixMath.NET;
using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using WorldSpace.GameWorld;
using UVector2 = UnityEngine.Vector2;

public class Player_State_WallJump : Player_State_Base {
    public Player_State_WallJump(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine)
        : base(LogicActor_Player.kStrBool_JumpFall, logicPlayer, renderPlayer, stateMachine) { }

    protected override void OnEnter() {
        base.OnEnter();
        LogicPlayer.SetVelocity_X(LogicPlayer.wallJumpForce.X * -LogicPlayer.facingDir);
        LogicPlayer.SetVelocity_Y(LogicPlayer.wallJumpForce.Y);
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        var v = PhysicsEntity.LinearVelocity;
        if (v.Y < Fix64.Zero) {
            _stateMachine.ChangeState(LogicPlayer.StateFall);
        }
        if (LogicPlayer.wallDetected) {
            _stateMachine.ChangeState(LogicPlayer.StateWallSlide);
        }
    }
}