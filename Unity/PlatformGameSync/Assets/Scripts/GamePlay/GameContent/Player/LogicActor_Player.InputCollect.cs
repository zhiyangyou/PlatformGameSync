using System;
using UnityEngine;
using UnityEngine.InputSystem;
using WorldSpace.GameWorld;
using UVector2 = UnityEngine.Vector2;


public class FrameInput<T> : IDisposable where T : struct {
    public T Value;
    public int StartFrameCount;


    private NextFrameTimer _nextFrameTimer;
    private int _uniqueKey;
    private InputAction _inputAction;
    public Action<InputAction.CallbackContext> onStart;
    public Action<InputAction.CallbackContext> onCancel;

    public FrameInput(
        InputAction inputAction,
        NextFrameTimer nextFrameTimer,
        int uniqueKey) {
        _nextFrameTimer = nextFrameTimer;
        _uniqueKey = uniqueKey;
        _inputAction = inputAction;
        inputAction.started += this.OnStarted;
        inputAction.canceled += this.OnCancle;
    }

    public void OnStarted(InputAction.CallbackContext context) {
        StartFrameCount = GameWorld.LogicFrameCount;
        _nextFrameTimer.ClearKey(_uniqueKey);
        onStart.Invoke(context);
    }

    public void OnCancle(InputAction.CallbackContext context) {
        if (GameWorld.LogicFrameCount == StartFrameCount) {
            _nextFrameTimer.CallOnNextFrame(_uniqueKey, () => { onCancel.Invoke(context); });
        }
        else {
            onCancel.Invoke(context);
        }
    }

    public void Dispose() {
        _inputAction.started -= this.OnStarted;
        _inputAction.canceled -= this.OnCancle;
    }
}

public partial class LogicActor_Player {
    public FrameInput<bool> jumpPressed;
    public FrameInput<UVector2> xInput;
    
    private void InitInputSystem() {
        InputSystem = new InputSystem_Player();
        InputSystem.Enable();


        xInput = new(InputSystem.Player.Movement, _nextFrameTimer, NextFrameTimeUniqueKeys.XInputKey);
        xInput.onStart = context => { xInput.Value = context.ReadValue<UVector2>(); };
        xInput.onCancel = _ => { xInput.Value = UVector2.zero; };

        jumpPressed = new(InputSystem.Player.Jump, _nextFrameTimer, NextFrameTimeUniqueKeys.JumpKey);
        jumpPressed.onStart = _ => jumpPressed.Value = true;
        jumpPressed.onCancel = _ => jumpPressed.Value = false;
    }

    void DisposeInputActions() {
        jumpPressed.Dispose();
    }
}