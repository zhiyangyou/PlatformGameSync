using System;
using UnityEngine;
using WorldSpace.GameWorld;

public class FrameInput : IDisposable {
    private int StartFrameCount;
    private bool onlyKeyOneFrame;
    private NextFrameTimer _nextFrameTimer;
    private int _uniqueKey;
    private InputSystem2 _inputSystem2;
    public Action onDown;
    public Action onUp;
    public KeyCode keyCode { get; private set; }

    public FrameInput(
        bool onlyKeyOneFrame,
        KeyCode keyCode,
        InputSystem2 inputSystem2,
        NextFrameTimer nextFrameTimer) {
        this.onlyKeyOneFrame = onlyKeyOneFrame;
        this.keyCode = keyCode;
        _nextFrameTimer = nextFrameTimer;
        _uniqueKey = (int)keyCode;
        _inputSystem2 = inputSystem2;
        if (onlyKeyOneFrame) {
            _inputSystem2.OnKeyDown += this.OnDown_OneFrame;
            _inputSystem2.OnKeyUp += this.OnUp_OneFrame;
        }
        else {
            _inputSystem2.OnKeyDown += this.OnDown_Keep;
            _inputSystem2.OnKeyUp += this.OnUp_Keep;
        }
    }


    public void OnDown_OneFrame(KeyCode code) {
        if (this.keyCode != code) {
            return;
        }
        StartFrameCount = GameWorld.LogicFrameCount;
        _nextFrameTimer.ClearKey(_uniqueKey);
        onDown.Invoke();
        _nextFrameTimer.CallOnNextFrame(_uniqueKey, () => { onUp.Invoke(); });
    }

    public void OnUp_OneFrame(KeyCode code) {
        // nothing
    }

    public void OnDown_Keep(KeyCode code) {
        if (this.keyCode != code) {
            return;
        }
        StartFrameCount = GameWorld.LogicFrameCount;
        _nextFrameTimer.ClearKey(_uniqueKey);
        onDown.Invoke();
    }

    public void OnUp_Keep(KeyCode code) {
        if (this.keyCode != code) {
            return;
        }
        if (GameWorld.LogicFrameCount == StartFrameCount) {
            _nextFrameTimer.CallOnNextFrame(_uniqueKey, () => { onUp.Invoke(); });
        }
        else {
            onUp.Invoke();
        }
    }

    public void Dispose() {
        if (onlyKeyOneFrame) {
            _inputSystem2.OnKeyDown -= this.OnDown_OneFrame;
            _inputSystem2.OnKeyUp -= this.OnUp_OneFrame;
        }
        else {
            _inputSystem2.OnKeyDown -= this.OnDown_Keep;
            _inputSystem2.OnKeyUp -= this.OnUp_Keep;
        }
    }
}