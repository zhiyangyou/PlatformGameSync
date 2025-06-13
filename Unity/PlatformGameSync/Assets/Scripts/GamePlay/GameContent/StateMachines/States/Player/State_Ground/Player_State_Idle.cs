using FixMath.NET;
using GamePlay.StateMachine;

public class Player_State_Idle : Player_State_Ground {
    #region 属性和字段

    #endregion


    public Player_State_Idle(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine)
        : base(LogicActor_Player.kStrBool_Idle, logicPlayer, renderPlayer, stateMachine) { }

    protected override void OnEnter() {
        base.OnEnter();

        LogicPlayer.SetVelocity_X(Fix64.Zero);
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        LogicPlayer.SetVelocity_X(Fix64.Zero);

        // 输入方向和人物朝向一致, 那么直接reture
        var inputDirSameToFacingDir =
            (LogicPlayer.xInput > Fix64.Zero && LogicPlayer.facingDir > Fix64.Zero)
            ||
            (LogicPlayer.xInput < Fix64.Zero && LogicPlayer.facingDir < Fix64.Zero);

        if (inputDirSameToFacingDir && LogicPlayer.wallDetected)
            return;

        if (this.LogicPlayer.xInput != Fix64.Zero) {
            this._stateMachine.ChangeState(LogicPlayer.StateMove);
        }
    }

    public override void Exit() {
        base.Exit();
    }
}