using System;
using FixMath.NET;


[Serializable]
public class BEPU_PhysicMaterial {
    public float Bounciness;
    public float KineticFriction;
    public float StaticFriction;

    public Fix64 FBounciness => (Fix64)Bounciness;
    public Fix64 FKineticFriction => (Fix64)KineticFriction;
    public Fix64 FStaticFriction => (Fix64)StaticFriction;

    public void SyncToBEPUMat(BEPUphysics.Materials.Material mat) {
        mat.Bounciness = this.FBounciness;
        mat.KineticFriction = this.FKineticFriction;
        mat.StaticFriction = this.FStaticFriction;
    }
}