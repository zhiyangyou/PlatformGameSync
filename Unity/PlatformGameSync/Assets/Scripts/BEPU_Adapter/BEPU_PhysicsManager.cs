using BEPUphysics;
using UnityEngine;
using System.Collections.Generic;
using BEPUphysics.Entities;
using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using Space = BEPUphysics.Space;
using FVector3 = BEPUutilities.Vector3;
using UVector3 = UnityEngine.Vector3;
using UQuaternion = UnityEngine.Quaternion;

public class BEPU_PhysicsManager : GoSingleton<BEPU_PhysicsManager> {
    private static readonly FVector3 gravity = new((Fix64)0, (Fix64)(-9.81f), (Fix64)0);
    private Space _bepuSpace { get; set; }

    public override void Init() {
        _bepuSpace = new Space();
        _bepuSpace.ForceUpdater.Gravity = gravity;
    }

    public void UpdatePhysicsWorld() {
        _bepuSpace.Update();
    }

    public void AddEntity(BEPU_BaseCollider collider) {
        _bepuSpace.Add(collider.entity);
    }
}