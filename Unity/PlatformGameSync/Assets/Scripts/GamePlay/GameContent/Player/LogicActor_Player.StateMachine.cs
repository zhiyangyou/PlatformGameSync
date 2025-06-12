using FixMath.NET;
using GamePlay.StateMachine;
using UVector2 = UnityEngine.Vector2;


public partial class LogicActor_Player : LogicActor {
    public StateMachine stateMachine { get; private set; }

    public Player_State_Idle StateIdle { get; private set; }
    public Player_State_Move StateMove { get; private set; }
    public Player_State_Jump StateJump { get; private set; }
    public Player_State_Fall StateFall { get; private set; }

    public const string kStrBool_Idle = "isIdle";
    public const string kStrBool_Move = "isMove";
    public const string kStrBool_JumpFall = "isJumpFall";
    public const string kStrFloat_yVelocity = "yVelocity";


    private void InitStateMachine() {
        stateMachine = new StateMachine();
        StateIdle = new(this, _renderPlayer, stateMachine, "Player-Idle");
        StateMove = new(this, _renderPlayer, stateMachine, "Player-Move");
        StateJump = new(this, _renderPlayer, stateMachine, "Player-Jump");
        StateFall = new(this, _renderPlayer, stateMachine, "Player-Fall");
        stateMachine.Init(StateIdle);
    }

    private void LogicFrameUpdate_StateMachine() {
        LogicFrameUpdate_HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }
}