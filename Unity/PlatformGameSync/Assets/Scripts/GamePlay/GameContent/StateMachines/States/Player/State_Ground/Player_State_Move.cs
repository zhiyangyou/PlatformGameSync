using GamePlay.StateMachine;


public class Player_State_Move : Player_State_Ground {
    public Player_State_Move(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(logicPlayer, renderPlayer, stateMachine, stateName) { }

    protected override void OnEnter() {
        base.OnEnter();
        Animator.SetBool(kStrBool_Move, true);
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (LogicPlayer.xInput.x == 0f) {
            this._stateMachine.ChangeState(LogicPlayer.StateIdle);
        }
        else {
            LogicPlayer.DoMove(LogicPlayer.xInput.ToFixedVector2().ToVector3());
        }
    }

    public override void Exit() {
        base.Exit();
        Animator.SetBool(kStrBool_Move, false);
    }
}