using BEPUphysics.Entities;
using FixMath.NET;


public enum BEPU_LerpMethod {
    Interpolate,
    Extrapolate,
}
public interface ILerpMethod {
    public void Init(Entity entity, BEPU_BaseColliderLogic baseCollider, Fix64 physicsTimeStep);
    public void BeforeWorldUpdate();
    public void AfterWorldUpdate();
    public (BEPUutilities.Vector3 interPos, BEPUutilities.Quaternion interRotation) UpdateLearp(Fix64 deltaTime);
}