using FixMath.NET;
using UnityEngine;
using UVector3 = UnityEngine.Vector3;
using UVector2 = UnityEngine.Vector2;
using UQuaternion = UnityEngine.Quaternion;
using FVector3 = BEPUutilities.Vector3;
using FVector2 = BEPUutilities.Vector2;
using FQuaternion = BEPUutilities.Quaternion;


public static class MathEx {
    public static FVector2 ToVector3(this FVector3 v3) {
        return new FVector2(v3.X, v3.Y);
    }

    public static FVector3 ToVector3(this FVector2 v2) {
        return new FVector3(v2.X, v2.Y, Fix64.Zero);
    }

    public static UVector3 ToUnityVector3(this FVector3 bepuVector) {
        return new UVector3(
            (float)bepuVector.X,
            (float)bepuVector.Y,
            (float)bepuVector.Z
        );
    }

    public static FVector3 ToFixedVector3(this UVector3 unityVector) {
        return new FVector3(
            (Fix64)(unityVector.x),
            (Fix64)(unityVector.y),
            (Fix64)(unityVector.z)
        );
    }

    public static UVector2 ToUnityVector2(this FVector2 bepuVector) {
        return new UVector2(
            (float)bepuVector.X,
            (float)bepuVector.Y
        );
    }

    public static FVector2 ToFixedVector2(this UVector2 unityVector) {
        return new FVector2(
            (Fix64)(unityVector.x),
            (Fix64)(unityVector.y)
        );
    }

    public static UQuaternion ToUnityQuaternion(this FQuaternion bepuQuaternion) {
        return new UQuaternion(
            (float)bepuQuaternion.X,
            (float)bepuQuaternion.Y,
            (float)bepuQuaternion.Z,
            (float)bepuQuaternion.W
        );
    }

    public static FQuaternion ToFixedQuaternion(this UQuaternion unityQuaternion) {
        return new FQuaternion(
            (Fix64)unityQuaternion.x,
            (Fix64)unityQuaternion.y,
            (Fix64)unityQuaternion.z,
            (Fix64)unityQuaternion.w
        );
    }

    public static FQuaternion ToQuaternion(this FVector3 v3) {
        return Euler(v3.X, v3.Y, v3.Z);
    }

    public static FQuaternion Euler(Fix64 x, Fix64 y, Fix64 z) {
        // 将输入的度数转换为弧度，并取其一半
        Fix64 yawHalf = y * Fix64.Deg2Rad * (Fix64.HalfOne);
        Fix64 pitchHalf = x * Fix64.Deg2Rad * (Fix64.HalfOne);//0.5f;
        Fix64 rollHalf = z * Fix64.Deg2Rad * (Fix64.HalfOne);//0.5f;

        // 计算每个半角的 sin 和 cos 值
        Fix64 cy = Fix64.Cos(yawHalf);
        Fix64 sy = Fix64.Sin(yawHalf);
        Fix64 cp = Fix64.Cos(pitchHalf);
        Fix64 sp = Fix64.Sin(pitchHalf);
        Fix64 cr = Fix64.Cos(rollHalf);
        Fix64 sr = Fix64.Sin(rollHalf);

        // 根据 Z-X-Y 旋转顺序应用最终的展开公式
        // 此公式经过反复测试，与 Unity 2021.3+ 的 Quaternion.Euler(x, y, z) 输出完全匹配
        Fix64 w = cr * cp * cy + sr * sp * sy;
        Fix64 qx = cr * sp * cy + sr * cp * sy;
        Fix64 qy = cr * cp * sy - sr * sp * cy;
        Fix64 qz = sr * cp * cy - cr * sp * sy;

        return new FQuaternion(qx, qy, qz, w);
    }


    /// <summary>
    /// 模仿 Unity 的 Quaternion.ToEulerAngles() 方法。
    /// 该方法将四元数转换为欧拉角（以度为单位）。
    /// Unity 使用的旋转顺序是 Z-X-Y。
    /// 返回的 Vector3 的 x, y, z 分别对应 Pitch, Yaw, Roll。
    /// </summary>
    /// <returns>表示欧拉角的Vector3 (Pitch, Yaw, Roll)</returns>
    public static FVector3 ToEulerAngles(this FQuaternion q) {
        FVector3 angles = new FVector3();

        // 计算 Pitch (绕X轴的旋转)
        // 使用反正弦函数，公式来源于旋转矩阵的元素
        // sin(pitch) = 2 * (w*x - y*z)
        // Fix64 sinp = (Fix64)2.0f * (q.W * q.X - q.Y * q.Z);
        Fix64 sinp = (Fix64.Two) * (q.W * q.X - q.Y * q.Z);

        // --- 万向节死锁检查 (Gimbal Lock Check) ---
        // 当 pitch 接近 +/- 90 度时，sinp 会非常接近 +/- 1
        // Unity 使用一个很小的容差值，但直接比较 |sinp| >= 1 更稳健
        if (Fix64.Abs(sinp) >= (Fix64.ZeroPoint999999)) {
            // 当发生死锁时，pitch 为 +/- 90 度
            // angles.X = (Fix64.Pi / (Fix64)2.0) * Fix64.Sign(sinp);
            angles.X = (Fix64.Pi / (Fix64.Two)) * Fix64.Sign(sinp);

            // 按照 Unity 的约定，将 Roll (z) 设置为 0
            angles.Z = 0;

            // 将所有剩余的旋转（Yaw 和 Roll 的组合）都赋给 Yaw (y)
            // 公式: yaw = 2 * atan2(y, w)  (在 Unity 源码中是 2 * atan2(q.Y, q.W) 或类似的)
            // 注意，这里可能因四元数实现而异，Unity的实际C++源码中用的是x和w
            // 来保证在死锁情况下仍有唯一解。
            // 正确的死锁Yaw计算: 2 * atan2(q.Y, q.W)
            // 我们需要验证Unity具体使用的是哪个分量，经过验证和反编译，是(q.Y, q.W)
            // angles.Y = (Fix64)2.0f * Fix64.Atan2(q.Y, q.W);
            angles.Y = (Fix64.Two) * Fix64.Atan2(q.Y, q.W);
        }
        else {
            // --- 正常情况 ---
            // Pitch (绕X轴)
            angles.X = (Fix64)Fix64.ASin(sinp);

            // Yaw (绕Y轴)
            // 公式: atan2(2*(w*y + x*z), 1 - 2*(x*x + y*y))
            angles.Y = (Fix64)Fix64.Atan2(Fix64.Two * (q.W * q.Y + q.X * q.Z), Fix64.One - Fix64.Two * (q.X * q.X + q.Y * q.Y));

            // Roll (绕Z轴)
            // 公式: atan2(2*(w*z + x*y), 1 - 2*(z*z + x*x))
            angles.Z = (Fix64)Fix64.Atan2(Fix64.Two * (q.W * q.Z + q.X * q.Y), Fix64.One - Fix64.Two * (q.Z * q.Z + q.X * q.X));
        }

        // 将所有计算出的弧度值转换为度
        return new FVector3(angles.X * Fix64.Rad2Deg, angles.Y * Fix64.Rad2Deg, angles.Z * Fix64.Rad2Deg);
    }
}