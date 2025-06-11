using FixMath.NET;
using GamePlay;
using UnityEngine;
using Vector3 = BEPUutilities.Vector3;

public partial class LogicActor_Player {
    Fix64 _moveSpeed = (Fix64)10f;

    public void DoMove(Vector3 input) {
        if (GameConstConfigs.UseLocalFrame) {
            SetVelocity(input * _moveSpeed);
        }
        else {
            Debug.LogError($"尚未实现网络移动");
        }
    }

    void SetVelocity(Vector3 v) {
        this.BaseColliderLogic.entity.LinearVelocity = v;
    }
}