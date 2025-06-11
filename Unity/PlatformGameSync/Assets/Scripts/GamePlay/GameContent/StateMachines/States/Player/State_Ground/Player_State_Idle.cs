using GamePlay.StateMachine;

public class Player_State_Idle : Player_State_Ground {
    #region 属性和字段

    #endregion


    public Player_State_Idle(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(logicPlayer, renderPlayer, stateMachine, stateName) { }

    protected override void OnEnter() {
        base.OnEnter();
        Animator.SetBool(kStrBool_Idle, true);
        LogicPlayer.DoIdle();
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (this.LogicPlayer.xInput.x != 0f) {
            this._stateMachine.ChangeState(LogicPlayer.StateMove);
        }
    }

    public override void Exit() {
        base.Exit();
        Animator.SetBool(kStrBool_Idle, false);
    }
}