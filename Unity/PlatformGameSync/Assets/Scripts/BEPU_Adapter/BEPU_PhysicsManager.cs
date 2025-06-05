using BEPUphysics.Constraints.SingleEntity;
using UnityEngine;
using FixMath.NET;
using Space = BEPUphysics.Space;
using Vector3 = BEPUutilities.Vector3;

public class BEPU_PhysicsManager : GoSingleton<BEPU_PhysicsManager> {
    private static readonly Vector3 gravity = new((Fix64)0, (Fix64)(-9.81f), (Fix64)0);
    private Space _bepuSpace { get; set; }

    public override void Init() {
        _bepuSpace = new Space();
        _bepuSpace.ForceUpdater.Gravity = gravity;
    }

    public void UpdatePhysicsWorld() {
        _bepuSpace.Update();
    }

    public void AddEntity(BEPU_BaseCollider collider) {
        // var lockAxisPosConstraint = new LockAxisPosConstraint(collider.entity);
        // var lockAxisRotationConstraint = new LockAxisRotationConstraint(collider.entity);
        // _bepuSpace.Add(lockAxisPosConstraint);
        // _bepuSpace.Add(lockAxisRotationConstraint);
        _bepuSpace.Add(collider.entity);
    }


    /// <summary>
    /// 设置冻结轴向, 用于3D物理引擎模拟2D物理引擎
    /// </summary>
    /// <param name="freezePosX"></param>
    /// <param name="freezePosY"></param>
    /// <param name="freezePosZ"></param>
    // public void SetFreezePos(bool freezePosX, bool freezePosY, bool freezePosZ) {
    //     
    //     Vector3 localAnchor = Vector3.Zero; // Constrain the entity's center of mass
    //     Vector3 planeNormal = Vector3.UnitZ; // The plane is an XY plane, normal points along Z
    //
    //     PointOnPlaneConstraint zConstraint = new PointOnPlaneConstraint(
    //         entity,
    //         localAnchor,
    //         planeNormal,
    //         targetZ
    //     );
    //
    //     // Add the constraint to the space.
    //     space.Add(zConstraint);
    //     _bepuSpace.Add();
    // }
}