using GamePlay.StateMachine;

public class Player_MoveState : Player_StateBase {
    public Player_MoveState(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(logicPlayer, renderPlayer, stateMachine, stateName) { }

    public override void Enter() {
        Animator.SetBool(kStrBool_Move, true);
    }

    public override void Update() {
        if (this.xInput.x == 0f) {
            this._stateMachine.ChangeState(LogicPlayer.state_Idle);
        }
        else {
            LogicPlayer.DoMove(xInput.); 2025年6月11日17:16:02  处理DoMove...  2025年6月11日17:16:10
        }
        
        
    }

    public override void Exit() {
        Animator.SetBool(kStrBool_Move, false);
    }
}