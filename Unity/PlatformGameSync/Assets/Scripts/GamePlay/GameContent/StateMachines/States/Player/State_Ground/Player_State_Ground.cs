using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UVector2 = UnityEngine.Vector2;

public class Player_State_Ground : Player_State_Base {
    public Player_State_Ground(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName)
        : base(logicPlayer, renderPlayer, stateMachine, stateName) {

    }


    protected override void OnEnter() { }

    public override void LogicFrameUpdate() {
        if (LogicPlayer.jumpPressed) {
            Debug.LogError("Jump.. ");
        }
    }

    public override void Exit() { }
}