using BEPUphysics.Entities;
using FixMath.NET;

public interface ILerpMethod {
    public Fix64 LerpAccumulator { get; }
    public void Init(Entity entity, BEPU_BaseColliderLogic baseCollider, Fix64 physicsTimeStep);
    public void StoreCurState();
    public void StoreNextSTate();
    public (BEPUutilities.Vector3 interPos, BEPUutilities.Quaternion interRotation) UpdateLearp();
}