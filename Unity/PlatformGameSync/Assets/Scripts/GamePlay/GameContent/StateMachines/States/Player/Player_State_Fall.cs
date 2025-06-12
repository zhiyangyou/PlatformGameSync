using GamePlay.StateMachine;

public class Player_State_Fall: Player_State_Base {
    public Player_State_Fall(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName)
        : base(LogicActor_Player.kStrBool_JumpFall,logicPlayer, renderPlayer, stateMachine, stateName) {
        
        
    }

    protected override void OnEnter() {
        base.OnEnter();
    }
    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (LogicPlayer.groundDetected) {
            _stateMachine.ChangeState(this.LogicPlayer.StateIdle);   
        }
    }
    public override void Exit() {
        base.Exit();
    }
}