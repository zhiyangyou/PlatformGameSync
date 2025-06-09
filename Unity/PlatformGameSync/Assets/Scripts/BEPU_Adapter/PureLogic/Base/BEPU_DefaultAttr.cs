using BEPUutilities;
using FixMath.NET;

public static class BEPU_DefaultAttr {
    // public static  BEPU_PhysicMaterial _defaultMaterial = null;
    //
    // public static  BEPU_PhysicMaterial DefaultMaterial {
    //     get {
    //         if (_defaultMaterial == null) {
    //             _defaultMaterial = new BEPU_PhysicMaterial();
    //             _defaultMaterial.Bounciness = 0f;
    //             _defaultMaterial.KineticFriction = 0f;
    //             _defaultMaterial.StaticFriction = 0f;
    //         }
    //         return _defaultMaterial;
    //     }
    // }

    public static readonly Vector3 DefaultGravity = new((Fix64)0, (Fix64)(-9.81f), (Fix64)0);
}