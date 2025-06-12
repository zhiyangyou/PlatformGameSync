using System;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUutilities;
using FixMath.NET;

public class BEPU_CapsuleColliderLogic : BEPU_BaseColliderLogic {
    #region 属性和字段

    public Fix64 Radiu = Fix64.HalfOne;
    public Fix64 Length = Fix64.One;

    #endregion

    public BEPU_CapsuleColliderLogic(string name, object renderObj, Action<Vector3, Quaternion> syncEntityPosAndRotationToRenderer) :
        base(name, renderObj, new CapsuleShape(Fix64.One, Fix64.One), syncEntityPosAndRotationToRenderer) { }


    protected override void SyncExtendAttrsToEntity() {
        ((CapsuleShape)entityShape).Radius = Radiu;
        ((CapsuleShape)entityShape).Length = Length;
    }
}