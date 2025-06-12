using FixMath.NET;
using GamePlay;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;

public partial class LogicActor_Player {
    public Fix64 moveSpeed = (Fix64)7f;
    public Fix64 jumpForce = (Fix64)10f;
    public Fix64 groundRayCastLen = (Fix64)1.3f;
    public BEPU_LayerDefine whatIsGround = BEPU_LayerDefine.Envirement;

    /// <summary>
    /// 是否解除到地面
    /// </summary>
    public bool groundDetected = false;


    private void HandleFlip(Fix64 xVelocity) {
        var isRight = xVelocity > Fix64.Zero;
        var isLeft = xVelocity < Fix64.Zero;
        if (isRight) {
            SetY180(true);
        }
        if (isLeft) {
            SetY180(false);
        }
    }

    private void LogicFrameUpdate_HandleCollisionDetection() {
        var halfW = BoxShape.HalfWidth;
        var p1 = this.PhysicsEntity.Position - FVector3.Left * halfW;
        var p2 = this.PhysicsEntity.Position - FVector3.Right * halfW;
        var ret1 = BEPU_PhysicsManagerUnity.Instance.RaycastAnyLayer(p1, FVector3.Down, this.groundRayCastLen, BEPU_LayerDefine.Envirement);
        var ret2 = BEPU_PhysicsManagerUnity.Instance.RaycastAnyLayer(p2, FVector3.Down, this.groundRayCastLen, BEPU_LayerDefine.Envirement);
        groundDetected = ret2 || ret1;
    }

    public void SetXVelocityByXInput() {
        SetXVelocity(moveSpeed * xInput.Value.X);
    }

    public void SetXVelocity(Fix64 v) {
        var oldV = BaseColliderLogic.entity.LinearVelocity;
        oldV.X = v;
        BaseColliderLogic.entity.LinearVelocity = oldV;
        HandleFlip(v);
    }

    public void SetYVelocityByJumpForce() {
        SetYVelocity(jumpForce);
    }


    public void SetYVelocity(Fix64 v) {
        var oldV = BaseColliderLogic.entity.LinearVelocity;
        oldV.Y = v;
        BaseColliderLogic.entity.LinearVelocity = oldV;
    }
}