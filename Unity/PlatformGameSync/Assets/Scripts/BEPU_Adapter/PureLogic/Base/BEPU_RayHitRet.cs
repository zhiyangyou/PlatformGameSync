using BEPUphysics.BroadPhaseEntries;
using BEPUutilities;
using FixMath.NET;

/// <summary>
/// 类似 Unity 的 RaycastHit 结构
/// </summary>
public struct RaycastHit {
    public Vector3 point;
    public Vector3 normal;
    public Fix64 distance;
    public BroadPhaseEntry collider; // 实际应为 BEPUphysics 中的碰撞对象

    // 添加常用属性（可选）
    public Vector3 barycentricCoordinate { get; set; }
    public int triangleIndex { get; set; }
}