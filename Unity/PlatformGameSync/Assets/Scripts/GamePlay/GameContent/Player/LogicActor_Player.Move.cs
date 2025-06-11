using FixMath.NET;
using GamePlay;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;

public partial class LogicActor_Player {
    Fix64 _moveSpeed = (Fix64)10f;

    public void DoMove(FVector3 input) {
        if (GameConstConfigs.UseLocalFrame) {
            SetVelocity(input * _moveSpeed);
        }
        else {
            Debug.LogError($"尚未实现网络移动");
        }
    }

    private void HandleFlip(FVector3 v) {
        // 2025年6月11日20:17:23
        // LogicAxis_X
    }
    
    void SetVelocity(FVector3 v) {
        this.BaseColliderLogic.entity.LinearVelocity = v;
        
    }
}