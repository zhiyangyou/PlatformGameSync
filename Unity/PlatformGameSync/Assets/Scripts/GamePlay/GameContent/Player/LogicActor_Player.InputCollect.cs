using UnityEngine;
using WorldSpace.GameWorld;
using UVector2 = UnityEngine.Vector2;

public partial class LogicActor_Player {
    public UVector2 xInput = UVector2.zero;
    public bool jumpPressed = false;

    // 数据采集帧数
    protected int _logicFrameCount_xInput = 0;
    protected int _logicFrameCount_jumpPressed = 0;

    private void InitInputSystem() {
        InputSystem = new InputSystem_Player();
        InputSystem.Enable();

        InputSystem.Player.Movement.started += context => {
            xInput = context.ReadValue<UVector2>();
            _logicFrameCount_xInput = GameWorld.LogicFrameCount;
        };
        InputSystem.Player.Movement.canceled += context => {
            if (GameWorld.LogicFrameCount == _logicFrameCount_xInput) {
                Debug.LogError("TODO xInput 采集丢失, 等下一帧再进行xInput还原操作");
                xInput = UVector2.zero;
            }
            else {
                xInput = UVector2.zero;
            }
        };
        InputSystem.Player.Jump.started += context => {
            jumpPressed = true;
            _logicFrameCount_jumpPressed = GameWorld.LogicFrameCount;
        };
        InputSystem.Player.Jump.canceled += context => {
            if (GameWorld.LogicFrameCount == _logicFrameCount_jumpPressed) {
                Debug.LogError("TODO jumpPressed 采集丢失, 等下一帧再进行jumpPressed还原操作");
                jumpPressed = false;
            }
            else {
                jumpPressed = false;
            }
        };
    }
}