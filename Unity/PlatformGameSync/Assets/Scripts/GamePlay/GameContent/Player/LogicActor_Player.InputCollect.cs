using FixMath.NET;
using UnityEngine;
using WorldSpace.GameWorld;


public partial class LogicActor_Player {
    public bool jumpPressed;
    public bool dashPressed;
    public Fix64 xInput; // 1:前进  0不动  -1:后退
    public Fix64 yInput; // 1:上   0:不动  -1:下

    public FrameInput Input_W;
    public FrameInput Input_A;
    public FrameInput Input_S;
    public FrameInput Input_D;
    public FrameInput Input_Shift;
    public FrameInput Input_Space;

    private void InitInputSystem() {
        inputSystem2 = WorldManager.GetWorld<GameWorld>().inputSystem2;

        Input_W = new FrameInput(false, KeyCode.W, inputSystem2, _nextFrameTimer);
        Input_A = new FrameInput(false, KeyCode.A, inputSystem2, _nextFrameTimer);
        Input_S = new FrameInput(false, KeyCode.S, inputSystem2, _nextFrameTimer);
        Input_D = new FrameInput(false, KeyCode.D, inputSystem2, _nextFrameTimer);
        Input_Shift = new FrameInput(true, KeyCode.LeftShift, inputSystem2, _nextFrameTimer);
        Input_Space = new FrameInput(true, KeyCode.Space, inputSystem2, _nextFrameTimer);


        Input_W.onDown = () => { yInput += Fix64.One; };
        Input_W.onUp = () => { yInput -= Fix64.One; };

        Input_S.onDown = () => { yInput -= Fix64.One; };
        Input_S.onUp = () => { yInput += Fix64.One; };

        Input_A.onDown = () => { xInput -= Fix64.One; };
        Input_A.onUp = () => { xInput += Fix64.One; };

        Input_D.onDown = () => { xInput += Fix64.One; };
        Input_D.onUp = () => { xInput -= Fix64.One; };

        Input_Shift.onDown = () => { dashPressed = true; };
        Input_Shift.onUp = () => { dashPressed = false; };

        Input_Space.onDown = () => { jumpPressed = true; };
        Input_Space.onUp = () => { jumpPressed = false; };
    }

    void DisposeInputActions() {
        Input_W.Dispose();
        Input_A.Dispose();
        Input_S.Dispose();
        Input_D.Dispose();
        Input_Space.Dispose();
    }
}