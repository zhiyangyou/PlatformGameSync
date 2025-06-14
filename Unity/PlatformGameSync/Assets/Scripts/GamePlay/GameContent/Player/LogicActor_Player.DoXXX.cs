using FixMath.NET;
using GamePlay;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;
using FVector2 = BEPUutilities.Vector2;

public partial class LogicActor_Player {
    #region 人物运动参数

    public Fix64 moveSpeedAirRate = (Fix64)0.7f; // 空中移动速度衰减
    public Fix64 wallSlideSpeedRate = (Fix64)0.2f; // 养着墙体下滑速度衰减
    public Fix64 moveSpeed = (Fix64)7f; // 移动速度
    public Fix64 jumpForce = (Fix64)10f; // 跳跃力
    public Fix64 groundRayCastLen = (Fix64)1.3f; // 地面检测射线长度
    public Fix64 wallRayCastLen = (Fix64)1.3f; // 爬墙检测射线长度
    public BEPU_LayerDefine whatIsGround = BEPU_LayerDefine.Envirement;
    public FVector2 wallJumpForce = new(Fix64.HalfOne, Fix64.HalfOne); // 跳墙方向和力度
    public Fix64 dashDuration = Fix64.One; // 冲刺持续时间
    public Fix64 dashSpeed = Fix64.One; // 冲刺速度

    #endregion

    #region 人物运动运行时状态值

    public Fix64 facingDir = Fix64.One; // 1=右 -1=左  
    public bool groundDetected = false; // 是否接触地面
    public bool wallDetected = false; // 是否接触地面

    #endregion


    #region private

    private void HandleFlip(Fix64 xVelocity) {
        var isRight = xVelocity > Fix64.Zero;
        var isLeft = xVelocity < Fix64.Zero;
        if (isRight) {
            SetY180(true, true);
            facingDir = Fix64.One;
        }
        if (isLeft) {
            SetY180(false, true);
            facingDir = Fix64.MinusOne;
        }
    }

    private void LogicFrameUpdate_HandleCollisionDetection() {
        // 地面检测
        {
            groundDetected = BEPU_PhysicsManagerUnity.Instance.RaycastAnyLayer(this.PhysicsEntity.Position, FVector3.Down, this.groundRayCastLen, whatIsGround);
        }

        // 墙体检测
        {
            wallDetected = BEPU_PhysicsManagerUnity.Instance.RaycastAnyLayer(PhysicsEntity.Position, FVector3.Right * this.facingDir, wallRayCastLen, whatIsGround);
        }
    }

    #endregion

    #region public

    public void Flip() {
        if (facingDir < Fix64.Zero) {
            SetY180(true, true);
            facingDir = Fix64.One;
        }
        else if (facingDir > Fix64.Zero) {
            SetY180(false, true);
            facingDir = Fix64.MinusOne;
        }
    }

    public void SetXVelocityByXInput() {
        SetXVelocityByXInput(Fix64.One);
    }

    public void SetXVelocityByXInput(Fix64 rate) {
        SetVelocity_X(moveSpeed * xInput * rate);
    }

    public void SetVelocity_X(Fix64 v) {
        var oldV = BaseColliderLogic.entity.LinearVelocity;
        oldV.X = v;
        BaseColliderLogic.entity.LinearVelocity = oldV;
        HandleFlip(v);
    }

    public void SetYVelocityByJumpForce() {
        SetVelocity_Y(jumpForce);
    }


    public void SetVelocity_Y(Fix64 v) {
        var oldV = BaseColliderLogic.entity.LinearVelocity;
        oldV.Y = v;
        BaseColliderLogic.entity.LinearVelocity = oldV;
    }

    #endregion
}