using System;
using UnityEngine;

/// <summary>
/// 用于将LogicActor_Player的内部参数暂时出来
/// </summary>
public class PlayerArgsDebugPanel : MonoBehaviour {
    public float moveSpeed = (float)7f;
    public float jumpForce = (float)10f;
    public float groundRayCastLen = (float)1.3f;
    public BEPU_LayerDefine whatIsGround = BEPU_LayerDefine.Envirement;
    public bool groundDetected = false;

    private RenderObject_Player _renderPlayer = null;

    RenderObject_Player renderPlayer {
        get {
            if (_renderPlayer == null) {
                _renderPlayer = this.GetComponent<RenderObject_Player>();
            }
            return _renderPlayer;
        }
    }

    private LogicActor_Player _logicPlayer = null;

    LogicActor_Player logicPlayer {
        get {
            if (_logicPlayer == null && renderPlayer != null) {
                _logicPlayer = renderPlayer.LogicObject as LogicActor_Player;
            }
            return _logicPlayer;
        }
    }

    private void Update() {
        if (logicPlayer == null) {
            return;
        }
        this.moveSpeed = (float)logicPlayer.moveSpeed;
        this.jumpForce = (float)logicPlayer.jumpForce;
        this.groundRayCastLen = (float)logicPlayer.groundRayCastLen;
        this.whatIsGround = logicPlayer.whatIsGround;
        this.groundDetected = logicPlayer.groundDetected;
    }
}