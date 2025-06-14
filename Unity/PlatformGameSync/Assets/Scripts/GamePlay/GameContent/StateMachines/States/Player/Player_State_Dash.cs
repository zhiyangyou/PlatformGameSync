using FixMath.NET;
using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using WorldSpace.GameWorld;
using UVector2 = UnityEngine.Vector2;
using FVector3 = BEPUutilities.Vector3;

public class Player_State_Dash : Player_State_Base {
    private FVector3 oldGravity;

    private Fix64 dashDir;
    public Player_State_Dash(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine)
        : base(LogicActor_Player.kStrBool_Dash, logicPlayer, renderPlayer, stateMachine) { }


    protected override void OnEnter() {
        base.OnEnter();
        dashDir = LogicPlayer.facingDir;
        FVector3? entityGravity = LogicPlayer.BaseColliderLogic.entity.Gravity;
        oldGravity = entityGravity == null ? LogicPlayer.BaseColliderLogic.entity.Space.ForceUpdater.Gravity : entityGravity.Value;
        LogicPlayer.BaseColliderLogic.entity.Gravity = FVector3.Zero;
        stateTimer = LogicPlayer.dashDuration; // 补偿一帧时长
        // LogicFrameUpdate();
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        
        CancelDashIfNeed();
        
        LogicPlayer.SetVelocity_X(LogicPlayer.dashSpeed * dashDir);
        LogicPlayer.SetVelocity_Y(Fix64.Zero);

        stateTimer -= _stateMachine.deltaTime;
        if (stateTimer <= Fix64.Zero) {
            if (LogicPlayer.groundDetected) {
                _stateMachine.ChangeState(LogicPlayer.StateIdle);
            }
            else {
                _stateMachine.ChangeState(LogicPlayer.StateFall);
            }
        }
    }

    public override void Exit() {
        base.Exit();
        LogicPlayer.BaseColliderLogic.entity.Gravity = oldGravity;
        LogicPlayer.SetVelocity_X(Fix64.Zero);
        LogicPlayer.SetVelocity_Y(Fix64.Zero);
    }


    private void CancelDashIfNeed() {
        if (LogicPlayer.wallDetected) {
            if (LogicPlayer.groundDetected) {
                _stateMachine.ChangeState(LogicPlayer.StateIdle);
            }
            else {
                _stateMachine.ChangeState(LogicPlayer.StateWallSlide);
            }
        }
    }
}