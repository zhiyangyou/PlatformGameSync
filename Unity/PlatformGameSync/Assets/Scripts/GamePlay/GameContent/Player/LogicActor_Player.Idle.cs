using FixMath.NET;
using GamePlay;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;

public partial class LogicActor_Player {
    public void DoIdle() {
        if (GameConstConfigs.UseLocalFrame) {
            SetXVelocity(Fix64.Zero);
        }
        else {
            Debug.LogError($"尚未实现网络的Idle");
        }
    }
}