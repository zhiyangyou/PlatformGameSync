using FixMath.NET;
using GamePlay.StateMachine;
using UnityEngine;

public class Player_State_Base : EntityState {
    protected LogicActor_Player LogicPlayer;
    protected RenderObject_Player RenderPlayer;
    protected Animator Animator;
    protected BEPU_CustomEntity PhysicsEntity;
    protected string BoolTriggerName;
    protected Fix64 stateTimer = Fix64.Zero;

    public Player_State_Base(string boolTriggerName, LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine) : base(stateMachine, boolTriggerName) {
        BoolTriggerName = boolTriggerName;
        LogicPlayer = logicPlayer;
        RenderPlayer = renderPlayer;
        Animator = renderPlayer.Animator;
        PhysicsEntity = LogicPlayer.BaseColliderLogic.entity;
    }


    protected override void OnEnter() {
        Animator.SetBool(BoolTriggerName, true);
    }

    public override void LogicFrameUpdate() {
        Animator.SetFloat(LogicActor_Player.kStrFloat_yVelocity, (float)PhysicsEntity.LinearVelocity.Y);

        if (LogicPlayer.dashPressed && CanDash()) {
            _stateMachine.ChangeState(LogicPlayer.StateDash);
        }
    }

    protected bool CanDash() {
        
        if (LogicPlayer.wallDetected) {
            return false;
        }
        if (_stateMachine.currentState == LogicPlayer.StateDash) {
            return false;
        }
        
        return true;
    }
    public override void Exit() {
        Animator.SetBool(BoolTriggerName, false);
    }
}