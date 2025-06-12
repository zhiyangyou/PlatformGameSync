using FixMath.NET;
using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UVector2 = UnityEngine.Vector2;

public class Player_State_Ground : Player_State_Base {
    public Player_State_Ground(string boolTriggerName, LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName)
        : base(boolTriggerName, logicPlayer, renderPlayer, stateMachine, stateName) { }


    protected override void OnEnter() {
        base.OnEnter();
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (PhysicsEntity.LinearVelocity.Y < Fix64.Zero && this.LogicPlayer.groundDetected == false)
            _stateMachine.ChangeState(LogicPlayer.StateFall);

        if (LogicPlayer.jumpPressed.Value) {
            _stateMachine.ChangeState(LogicPlayer.StateJump);
        }
    }

    public override void Exit() {
        base.Exit();
    }
}