using GamePlay.StateMachine;

public class Player_StateBase : EntityState {
    protected LogicActor_Player LogicPlayer;
    protected RenderObject_Player RenderPlayer;
    protected InputSystem_Player InputPlayer;

    public Player_StateBase(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(stateMachine, stateName) {
        LogicPlayer = logicPlayer;
        RenderPlayer = renderPlayer;
        InputPlayer = logicPlayer.InputSystem;
    } 

    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}