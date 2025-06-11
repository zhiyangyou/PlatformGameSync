using GamePlay.StateMachine;
using UnityEngine;

public class Player_IdleState : Player_StateBase {
    public Player_IdleState(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(logicPlayer, renderPlayer, stateMachine, stateName) {
        
    }
    public override void Enter() { }

    public override void Update() {

        if (Input.GetKeyDown(KeyCode.D)) {
            LogicPlayer.stateMachine.ChangeState(LogicPlayer.state_Move);
        }
        
    }
    public override void Exit() { }
}