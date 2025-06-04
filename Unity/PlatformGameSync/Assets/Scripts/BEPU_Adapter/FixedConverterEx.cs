using FixMath.NET;
using UVector3 = UnityEngine.Vector3;
using FVector3 = BEPUutilities.Vector3;
using UVector2 = UnityEngine.Vector2;
using FVector2 = BEPUutilities.Vector2;
using UQuaternion = UnityEngine.Quaternion;
using FQuaternion = BEPUutilities.Quaternion;


public static class FixedConverterEx {
    // BEPUphysics v1 (Fix64) uses a specific internal representation.
    // Fix64.ToFloat() and Fix64.FromFloat() are the correct conversion methods.

    public static UVector3 ToUVector3(this FVector3 bepuVector) {
        return new UVector3(
            (float)bepuVector.X,
            (float)bepuVector.Y,
            (float)bepuVector.Z
        );
    }

    public static FVector3 ToFVector3(this UVector3 unityVector) {
        return new FVector3(
            (Fix64)(unityVector.x),
            (Fix64)(unityVector.y),
            (Fix64)(unityVector.z)
        );
    }

    public static UVector2 ToUVector2(this FVector2 bepuVector) {
        return new UVector2(
            (float)bepuVector.X,
            (float)bepuVector.Y
        );
    }

    public static FVector2 ToFVector2(this UVector2 unityVector) {
        return new FVector2(
            (Fix64)(unityVector.x),
            (Fix64)(unityVector.y)
        );
    }

    public static UQuaternion ToUQuaternion(this FQuaternion bepuQuaternion) {
        return new UQuaternion(
            (float)bepuQuaternion.X,
            (float)bepuQuaternion.Y,
            (float)bepuQuaternion.Z,
            (float)bepuQuaternion.W
        );
    }

    public static FQuaternion ToFQuaternion(this UQuaternion unityQuaternion) {
        return new FQuaternion(
            (Fix64)unityQuaternion.x,
            (Fix64)unityQuaternion.y,
            (Fix64)unityQuaternion.z,
            (Fix64)unityQuaternion.w
        );
    }
}