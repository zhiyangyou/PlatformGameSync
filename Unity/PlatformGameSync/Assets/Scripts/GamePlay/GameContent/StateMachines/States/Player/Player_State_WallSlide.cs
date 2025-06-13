using FixMath.NET;
using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using WorldSpace.GameWorld;
using UVector2 = UnityEngine.Vector2;

public class Player_State_WallSlide : Player_State_Base {
    public Player_State_WallSlide(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine)
        : base(LogicActor_Player.kStrBool_WallSlide, logicPlayer, renderPlayer, stateMachine) { }


    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();

        HandlerWallSlide();

        if (LogicPlayer.jumpPressed) {
            _stateMachine.ChangeState(this.LogicPlayer.StateWallJump);
        }

        if (!LogicPlayer.wallDetected) {
            _stateMachine.ChangeState(this.LogicPlayer.StateFall);
        }

        if (LogicPlayer.groundDetected) {
            _stateMachine.ChangeState(this.LogicPlayer.StateIdle);
            LogicPlayer.Flip();
        }
    }

    private void HandlerWallSlide() {
        // LogicPlayer.SetVelocity_X(Fix64.Zero); // 沿着墙体下滑, 避免X轴偏移
        // 用户按住方向: 下
        if (LogicPlayer.yInput < Fix64.Zero) {
            var oldV = PhysicsEntity.LinearVelocity.Y;
            LogicPlayer.SetVelocity_Y(oldV);
            LogicPlayer.SetVelocity_X(LogicPlayer.xInput);
        }
        else {
            var oldV = PhysicsEntity.LinearVelocity.Y * LogicPlayer.wallSlideSpeedRate;
            LogicPlayer.SetVelocity_Y(oldV);
            LogicPlayer.SetVelocity_X(LogicPlayer.xInput);
        }
    }
}