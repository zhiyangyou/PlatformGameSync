using System;
using FixMath.NET;


[Serializable]
public class BEPU_PhysicMaterial {
    public Fix64 Bounciness;
    public Fix64 KineticFriction;
    public Fix64 StaticFriction;

    public void SyncToBEPUMat(BEPUphysics.Materials.Material mat) {
        mat.Bounciness = Bounciness;
        mat.KineticFriction = KineticFriction;
        mat.StaticFriction = StaticFriction;
    }
}