using GamePlay.StateMachine;

public class Player_State_Jump : Player_State_Base {
    public Player_State_Jump(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName)
        : base(LogicActor_Player.kStrBool_JumpFall, logicPlayer, renderPlayer, stateMachine, stateName) { }

    protected override void OnEnter() {
        base.OnEnter();
        LogicPlayer.SetYVelocity(LogicPlayer.jumpForce);
    }

    public override void LogicFrameUpdate() {
        base.LogicFrameUpdate();
        if (PhysicsEntity.LinearVelocity.Y <= 0) {
            _stateMachine.ChangeState(LogicPlayer.StateFall);
        }
    }

    public override void Exit() {
        base.Exit();
    }
}