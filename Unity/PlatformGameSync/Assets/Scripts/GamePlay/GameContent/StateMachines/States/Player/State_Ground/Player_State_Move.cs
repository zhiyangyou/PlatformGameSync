using FixMath.NET;
using GamePlay.StateMachine;


public class Player_State_Move : Player_State_Ground {
    public Player_State_Move(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName)
        : base(LogicActor_Player.kStrBool_Move, logicPlayer, renderPlayer, stateMachine, stateName) { }

    protected override void OnEnter() {
        base.OnEnter();
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (LogicPlayer.xInput.Value.X == Fix64.Zero) {
            this._stateMachine.ChangeState(LogicPlayer.StateIdle);
        }
        LogicPlayer.SetXVelocityByXInput();
    }

    public override void Exit() {
        base.Exit();
    }
}