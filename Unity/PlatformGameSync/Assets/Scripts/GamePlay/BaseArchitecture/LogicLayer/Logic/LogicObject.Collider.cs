using System;
using FixMath.NET;
using UnityEngine;
using Vector3 = BEPUutilities.Vector3;
using Quaternion = BEPUutilities.Quaternion;


public partial class LogicObject {
    public BEPU_ColliderType ColliderType = BEPU_ColliderType.Box;
    public Vector3 PhysicsEntryCenter = Vector3.Zero;
    public BEPU_BaseColliderLogic BaseColliderLogic { get; private set; }

    public void FlipYRotation(Fix64 flipYDegree) {
        var e1 = BaseColliderLogic.entity.Orientation.ToEulerAngles(); // TODO 应该实现一个Orientation转换欧拉角的方法, 而不是借用Unity的转换, 可能产生浮点精度误差
        e1.Y += flipYDegree;
    }


    private void LerpSyncTransform_EntityToLogic(Vector3 pos, Quaternion rot) {
        LogicPos = pos - PhysicsEntryCenter;
        LogicRotation = rot;
    }


    private void InitColliderLogic() {
        var name = $"RenderObjectWithCollider-LogicObjectNam";
        switch (ColliderType) {
            case BEPU_ColliderType.None:
                BaseColliderLogic = null;
                break;
            case BEPU_ColliderType.Box:
                BaseColliderLogic = new BEPU_BoxColliderLogic(name, this, LerpSyncTransform_EntityToLogic);
                break;
            case BEPU_ColliderType.Sphere:
                BaseColliderLogic = new BEPU_SphereColliderLogic(name, this, LerpSyncTransform_EntityToLogic);
                break;
            case BEPU_ColliderType.Capsule:
                BaseColliderLogic = new BEPU_CapsuleColliderLogic(name, this, LerpSyncTransform_EntityToLogic);
                break;
            default:
                throw new ArgumentOutOfRangeException($"InitColliderLogic 尚未不支持的碰撞体类型{ColliderType}");
        }
        // SyncTransform_Render2Entity();
    }
}