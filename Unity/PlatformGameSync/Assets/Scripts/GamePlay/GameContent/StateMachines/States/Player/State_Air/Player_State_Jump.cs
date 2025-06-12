using GamePlay.StateMachine;

public class Player_State_Jump : Player_State_Air {
    public Player_State_Jump(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine)
        : base(LogicActor_Player.kStrBool_JumpFall, logicPlayer, renderPlayer, stateMachine) { }

    protected override void OnEnter() {
        base.OnEnter();
        LogicPlayer.SetYVelocityByJumpForce();
     
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