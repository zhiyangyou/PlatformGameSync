using FixMath.NET;
using GamePlay;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;

public partial class LogicActor_Player {
  

    public void DoMove(FVector3 input) {
        if (GameConstConfigs.UseLocalFrame) {
            SetXVelocity((input * _moveSpeed).X);
        }
        else {
            Debug.LogError($"尚未实现网络移动");
        }
    }

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

    void SetXVelocity(Fix64 v) {
        var oldV = BaseColliderLogic.entity.LinearVelocity;
        oldV.X = v;
        BaseColliderLogic.entity.LinearVelocity = oldV;
        HandleFlip(v);
    }
}