using GamePlay.StateMachine;

public class Player_MoveState : Player_StateBase {
    public Player_MoveState(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(logicPlayer, renderPlayer, stateMachine, stateName) {
        
    }
    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}