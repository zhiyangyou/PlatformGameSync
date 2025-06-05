using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Entities;
using BEPUutilities;
using FixMath.NET;

public class BEPU_CustomEntity : Entity {
    #region 属性和字段

    public bool freezePos_X = false;
    public bool freezePos_Y = false;
    public bool freezePos_Z = false;
    public bool freezeRotation_X = false;
    public bool freezeRotation_Y = false;
    public bool freezeRotation_Z = false;

    #endregion

    #region ctors

    protected BEPU_CustomEntity() { }
    public BEPU_CustomEntity(EntityCollidable collisionInformation) : base(collisionInformation) { }
    public BEPU_CustomEntity(EntityCollidable collisionInformation, Fix64 mass) : base(collisionInformation, mass) { }
    public BEPU_CustomEntity(EntityCollidable collisionInformation, Fix64 mass, Matrix3x3 inertiaTensor) : base(collisionInformation, mass, inertiaTensor) { }
    public BEPU_CustomEntity(EntityShape shape) : base(shape) { }
    public BEPU_CustomEntity(EntityShape shape, Fix64 mass) : base(shape, mass) { }
    public BEPU_CustomEntity(EntityShape shape, Fix64 mass, Matrix3x3 inertiaTensor) : base(shape, mass, inertiaTensor) { }

    #endregion

    #region override

    protected override Vector3 GetRotationIncrement(Fix64 dt) {
        var angularV = AngularVelocity;

        if (freezeRotation_X)
            angularV.X = Fix64.Zero;
        if (freezeRotation_Y)
            angularV.Y = Fix64.Zero;
        if (freezeRotation_Z)
            angularV.Z = Fix64.Zero;

        AngularVelocity = angularV;

        Vector3.Multiply(ref angularV, dt * F64.C0p5, out var increment);
        return increment;
    }

    protected override Vector3 GetPosIncrement(Fix64 dt) {
        var linearV = LinearVelocity;
        if (freezePos_X)
            linearV.X = Fix64.Zero;
        if (freezePos_Y)
            linearV.Y = Fix64.Zero;
        if (freezePos_Z)
            linearV.Z = Fix64.Zero;
        LinearVelocity = linearV;
        Vector3.Multiply(ref linearV, dt, out var increment);
        return increment;
    }
    
    

    #endregion
}