using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_IdleState : Player_StateBase {
    public Player_IdleState(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(logicPlayer, renderPlayer, stateMachine, stateName) {
        this.InputPlayer.Player.Movement.started += MovementOnstarted;
        this.InputPlayer.Player.Movement.performed += MovementOnperformed;
        this.InputPlayer.Player.Movement.canceled += MovementOncanceled;
    }

    private void MovementOncanceled(InputAction.CallbackContext obj) {
        Debug.LogError($"cancle {obj.phase} {obj.valueType} {obj.ReadValue<Vector2>()}");
    }

    private void MovementOnperformed(InputAction.CallbackContext obj) {
        Debug.LogError($"perform {obj.phase} {obj.ReadValue<Vector2>()}");
    }

    private void MovementOnstarted(InputAction.CallbackContext obj) {
        Debug.LogError($"start {obj.phase} {obj.ReadValue<Vector2>()}");
    }

    public override void Enter() { }

    public override void Update() {

        
        
    }
    public override void Exit() { }
}