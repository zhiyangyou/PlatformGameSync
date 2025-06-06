using BEPUphysics.Entities;
using FixMath.NET;

public interface ILerpMethod {
    public void Init(Entity entity, BEPU_BaseCollider baseCollider);
    public void StoreCurState();
    public void StoreNextSTate();
    public (BEPUutilities.Vector3 interPos, BEPUutilities.Quaternion interRotation) UpdateLearp();
    public (BEPUutilities.Vector3 interPos, BEPUutilities.Quaternion interRotation) DoLerp(Fix64 alpha);
}