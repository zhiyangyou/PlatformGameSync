using BEPUutilities;
using FixMath.NET;

public static class VectorEx {
    public static Vector2 ToVector2(this Vector3 v3) {
        return new Vector2(v3.X, v3.Y);
    }

    public static Vector3 ToVector2(this Vector2 v2) {
        return new Vector3(v2.X, v2.Y, Fix64.Zero);
    }
}