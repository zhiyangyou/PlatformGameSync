using FixMath.NET;
using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UVector2 = UnityEngine.Vector2;

public class Player_State_Air : Player_State_Base {
    public Player_State_Air(string boolTriggerName, LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine)
        : base(boolTriggerName, logicPlayer, renderPlayer, stateMachine) { }


    protected override void OnEnter() {
        base.OnEnter();
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (LogicPlayer.xInput.Value.X != Fix64.Zero) {
            LogicPlayer.SetXVelocityByXInput(LogicPlayer.moveSpeedAirRate);
        }
    }

    public override void Exit() {
        base.Exit();
    }
}