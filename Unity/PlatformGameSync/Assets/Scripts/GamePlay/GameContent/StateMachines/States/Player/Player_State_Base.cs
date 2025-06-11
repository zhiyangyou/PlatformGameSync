using GamePlay.StateMachine;
using UnityEngine;

public class Player_State_Base : EntityState {
    protected const string kStrBool_Idle = "isIdle";
    protected const string kStrBool_Move = "isMove";

    protected LogicActor_Player LogicPlayer;
    protected RenderObject_Player RenderPlayer;
    protected InputSystem_Player InputPlayer;
    protected Animator Animator;

    public Player_State_Base(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(stateMachine, stateName) {
        LogicPlayer = logicPlayer;
        RenderPlayer = renderPlayer;
        InputPlayer = logicPlayer.InputSystem;
        Animator = renderPlayer.Animator;
    }


    protected override void OnEnter() { }
    public override void LogicFrameUpdate() { }
    public override void Exit() { }
}