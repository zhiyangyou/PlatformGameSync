using GamePlay.StateMachine;
using PlasticGui;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_IdleState : Player_StateBase {
    #region 属性和字段

    #endregion


    public Player_IdleState(LogicActor_Player logicPlayer, RenderObject_Player renderPlayer, StateMachine stateMachine, string stateName) : base(logicPlayer, renderPlayer, stateMachine, stateName) { }

    public override void Enter() {
        Animator.SetBool(kStrBool_Idle, true);
    }

    public override void Update() {
        if (this.xInput.x != 0f) {
            this._stateMachine.ChangeState(LogicPlayer.state_Move);
        }
    }

    public override void Exit() {
        Animator.SetBool(kStrBool_Idle, false);
    }
}