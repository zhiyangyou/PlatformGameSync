using System;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUutilities;
using FixMath.NET;

public class BEPU_BoxColliderLogic : BEPU_BaseColliderLogic {
    #region 属性和字段

    public Vector3 size = new Vector3(Fix64.One, Fix64.One, Fix64.One);

    #endregion


    public BEPU_BoxColliderLogic(string name, object renderObj, Action<Vector3, Quaternion> syncEntityPosAndRotationToRenderer) :
        base(name, renderObj,new BoxShape(Fix64.One, Fix64.One, Fix64.One), syncEntityPosAndRotationToRenderer) { }


    protected override void SyncExtendAttrsToEntity() {
        ((BoxShape)entityShape).Width = size.X;
        ((BoxShape)entityShape).Height = size.Y;
        ((BoxShape)entityShape).Length = size.Z;
    }
}