using BEPUphysics.Entities;
using FixMath.NET;

public interface ILerpMethod {
    public void Init(Entity entity, BEPU_BaseColliderLogic baseCollider, Fix64 physicsTimeStep);
    public void BeforeWorldUpdate();
    public void AfterNextSTate();
    public (BEPUutilities.Vector3 interPos, BEPUutilities.Quaternion interRotation) UpdateLearp(Fix64 deltaTime);
}