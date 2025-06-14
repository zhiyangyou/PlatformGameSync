using FixMath.NET;
using GamePlay.StateMachine;
using UVector2 = UnityEngine.Vector2;


public partial class LogicActor_Player : LogicActor {
    public StateMachine stateMachine { get; private set; }

    public Player_State_Idle StateIdle { get; private set; }
    public Player_State_Move StateMove { get; private set; }
    public Player_State_Jump StateJump { get; private set; }
    public Player_State_Fall StateFall { get; private set; }
    public Player_State_WallSlide StateWallSlide { get; private set; }
    public Player_State_WallJump StateWallJump { get; private set; }
    public Player_State_Dash StateDash { get; private set; }

    public const string kStrBool_Idle = "isIdle";
    public const string kStrBool_Move = "isMove";
    public const string kStrBool_JumpFall = "isJumpFall";
    public const string kStrBool_WallSlide = "isWallSilde";
    public const string kStrBool_Dash = "isDash";
    public const string kStrFloat_yVelocity = "yVelocity";


    private void InitStateMachine() {
        stateMachine = new StateMachine();
        StateIdle = new(this, _renderPlayer, stateMachine);
        StateMove = new(this, _renderPlayer, stateMachine);
        StateJump = new(this, _renderPlayer, stateMachine);
        StateFall = new(this, _renderPlayer, stateMachine);
        StateWallSlide = new(this, _renderPlayer, stateMachine);
        StateWallJump = new(this, _renderPlayer, stateMachine);
        StateDash = new(this, _renderPlayer, stateMachine);
        stateMachine.Init(StateIdle);
    }

    private void LogicFrameUpdate_StateMachine(Fix64 deltaTime) {
      
        stateMachine.Update(deltaTime);
    }
}