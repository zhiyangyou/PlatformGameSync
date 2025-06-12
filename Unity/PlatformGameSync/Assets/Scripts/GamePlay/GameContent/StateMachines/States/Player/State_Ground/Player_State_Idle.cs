using FixMath.NET;
using GamePlay.StateMachine;

public class Player_State_Idle : Player_State_Ground {
    #region 属性和字段

    #endregion


    public Player_State_Idle(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName)
        : base(LogicActor_Player.kStrBool_Idle, logicPlayer, renderPlayer, stateMachine, stateName) { }

    protected override void OnEnter() {
        base.OnEnter();

        LogicPlayer.SetXVelocity(0);
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (this.LogicPlayer.xInput.Value.X != Fix64.Zero) {
            this._stateMachine.ChangeState(LogicPlayer.StateMove);
        }
    }

    public override void Exit() {
        base.Exit();
    }
}