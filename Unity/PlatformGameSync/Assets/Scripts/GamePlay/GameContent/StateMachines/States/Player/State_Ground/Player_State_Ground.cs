using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UVector2 = UnityEngine.Vector2;

public class Player_State_Ground : Player_State_Base {
    public Player_State_Ground(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName)
        : base(logicPlayer, renderPlayer, stateMachine, stateName) {
        InputPlayer.Player.Movement.started += context => LogicPlayer.xInput = context.ReadValue<UVector2>();
        InputPlayer.Player.Movement.canceled += context => LogicPlayer.xInput = UVector2.zero;
        InputPlayer.Player.Jump.started += context => LogicPlayer.jumpPressed = true;
        InputPlayer.Player.Jump.canceled += context => LogicPlayer.jumpPressed = false;
    }


    protected override void OnEnter() { }

    public override void LogicFrameUpdate() {
        if (LogicPlayer.jumpPressed) {
            Debug.LogError("Jump.. ");
        }
    }

    public override void Exit() { }
}