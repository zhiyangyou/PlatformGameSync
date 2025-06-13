using FixMath.NET;
using GamePlay.StateMachine;

public class Player_State_Idle : Player_State_Ground {
    #region 属性和字段

    #endregion


    public Player_State_Idle(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine)
        : base(LogicActor_Player.kStrBool_Idle, logicPlayer, renderPlayer, stateMachine) { }

    protected override void OnEnter() {
        base.OnEnter();

        LogicPlayer.SetXVelocity(Fix64.Zero);
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        LogicPlayer.SetXVelocity(Fix64.Zero);

        if (LogicPlayer.xInput.Value.X == LogicPlayer.facingDir && LogicPlayer.wallDetected)
            return;

        if (this.LogicPlayer.xInput.Value.X != Fix64.Zero) {
            this._stateMachine.ChangeState(LogicPlayer.StateMove);
        }
    }

    public override void Exit() {
        base.Exit();
    }
}