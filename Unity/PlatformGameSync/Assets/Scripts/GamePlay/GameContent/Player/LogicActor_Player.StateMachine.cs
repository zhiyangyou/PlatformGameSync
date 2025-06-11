using FixMath.NET;
using GamePlay.StateMachine;
using UVector2 = UnityEngine.Vector2;


public partial class LogicActor_Player : LogicActor {
    public StateMachine stateMachine { get; private set; }

    public Player_State_Idle StateIdle { get; private set; }
    public Player_State_Move StateMove { get; private set; }


    Fix64 _moveSpeed = (Fix64)10f;
    public UVector2 xInput = UVector2.zero;
    public bool jumpPressed = false;

    private void InitStateMachine() {
        stateMachine = new StateMachine();
        StateIdle = new(this, _renderPlayer, stateMachine, "Player-Idle");
        StateMove = new(this, _renderPlayer, stateMachine, "Player-Move");
        stateMachine.Init(StateIdle);
    }

    private void LogicFrameUpdate_StateMachine() {
        stateMachine.UpdateActiveState();
    }
}