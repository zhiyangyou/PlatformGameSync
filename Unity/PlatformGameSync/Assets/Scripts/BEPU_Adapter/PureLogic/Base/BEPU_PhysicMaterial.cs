using System;
using FixMath.NET;


[Serializable]
public class BEPU_PhysicMaterial {
    public Fix64 Bounciness;
    public Fix64 KineticFriction;
    public Fix64 StaticFriction;

    public Fix64 FBounciness => Bounciness;
    public Fix64 FKineticFriction => KineticFriction;
    public Fix64 FStaticFriction => StaticFriction;

    public void SyncToBEPUMat(BEPUphysics.Materials.Material mat) {
        mat.Bounciness = this.FBounciness;
        mat.KineticFriction = this.FKineticFriction;
        mat.StaticFriction = this.FStaticFriction;
    }
}