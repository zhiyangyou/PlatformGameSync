using GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_StateBase : EntityState {
    protected const string kStrBool_Idle = "isIdle";
    protected const string kStrBool_Move = "isMove";

    protected LogicActor_Player LogicPlayer;
    protected RenderObject_Player RenderPlayer;
    protected InputSystem_Player InputPlayer;
    protected Animator Animator;

    protected Vector2 xInput = Vector2.zero;


    private void MovementOncanceled(InputAction.CallbackContext obj) {
        xInput = Vector2.zero;
    } 


    private void MovementOnstarted(InputAction.CallbackContext obj) {
        xInput = obj.ReadValue<Vector2>();
    }

    public Player_StateBase(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(stateMachine, stateName) {
        LogicPlayer = logicPlayer;
        RenderPlayer = renderPlayer;
        InputPlayer = logicPlayer.InputSystem;
        Animator = renderPlayer.Animator;

        this.InputPlayer.Player.Movement.started += MovementOnstarted;
        this.InputPlayer.Player.Movement.canceled += MovementOncanceled;
    }

    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}