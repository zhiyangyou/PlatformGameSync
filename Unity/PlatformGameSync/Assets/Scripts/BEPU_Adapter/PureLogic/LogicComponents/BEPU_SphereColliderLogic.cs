using System;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUutilities;
using FixMath.NET;

public class BEPU_SphereColliderLogic : BEPU_BaseColliderLogic {
    #region 属性和字段

    public Fix64 Radiu = (Fix64)0.5f;

    #endregion

    public BEPU_SphereColliderLogic(string name, object renderObj, Action<Vector3, Quaternion> syncEntityPosAndRotationToRenderer) :
        base(name, renderObj, new SphereShape(Fix64.One), syncEntityPosAndRotationToRenderer) { }


    protected override void SyncExtendAttrsToEntity() {
        ((SphereShape)entityShape).Radius = Radiu;
    }
}