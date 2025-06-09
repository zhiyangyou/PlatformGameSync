using System;
using System.Collections.Generic;
using BEPUphysics;
using BEPUphysics.CollisionRuleManagement;
using FixMath.NET;
using UnityEngine;
using Space = BEPUphysics.Space;
using Vector3 = BEPUutilities.Vector3;

public enum CollisionLayers {
    Player,
    Enemy,
    PlayerProjectile,
    Environment
}

public class BEPU_PhysicsManager : Singleton<BEPU_PhysicsManager> {
    public static readonly Vector3 DefaultGravity = new((Fix64)0, (Fix64)(-9.81f), (Fix64)0);
    public Space Space { get; private set; }

    public bool NeedAutoUpdate = true;

    private bool _hasInit = false;

    private HashSet<BEPU_BaseColliderLogic> _setAllColliders = new();
    public Vector3 SpaceGravity => Application.isPlaying ? Space.ForceUpdater.Gravity : DefaultGravity;

    public override void Init() {
        if (!_hasInit) {
            _hasInit = true;
            Debug.Log("Editor下 BEPU 物理引擎Space被创建");
            Space = new Space();
            Space.ForceUpdater.Gravity = DefaultGravity;
        }
    }


    public void UpdatePhysicsWorld(Fix64 dt) {
        foreach (var collider in _setAllColliders) {
            try {
                collider.OnBeforeUpdate();
            }
            catch (Exception e) {
                Debug.LogError("OnBeforeUpdate 回调报错");
                Debug.LogException(e);
            }
        }
        Space.Update(dt);
        foreach (var collider in _setAllColliders) {
            try {
                collider.OnAfterUpdate();
            }
            catch (Exception e) {
                Debug.LogError("OnBeforeUpdate 回调报错");
                Debug.LogException(e);
            }
        }
    }

    public void AddEntity(BEPU_BaseColliderLogic collider) {
        if (_setAllColliders.Add(collider)) {
            Space.Add(collider.entity);
        }
        else {
            Debug.LogError($"AddEntity 失败, 因为重复了");
        }
    }

    public void RemoveEntity(BEPU_BaseColliderLogic collider) {
        if (_setAllColliders.Remove(collider)) {
            Space.Remove(collider.entity);
        }
        else {
            Debug.LogError($"RemoveEntity 失败, Entity并没有加入到Space中 go:{collider.name}");
        }
    }


    public bool Raycast(
        Vector3 origin,
        Vector3 direction,
        Fix64 maxDistance,
        out RaycastHit hitInfo,
        uint layerMask
    ) {
        hitInfo = new RaycastHit();

        // 标准化方向向量
        Vector3 normalizedDir = direction;
        if (normalizedDir.LengthSquared() != 1) {
            normalizedDir.Normalize();
        }


        // 创建射线
        BEPUutilities.Ray ray = new BEPUutilities.Ray(origin, normalizedDir);
        RayCastResult rayHit;

        // 执行射线检测
        bool hit = Space.RayCast(ray, maxDistance, out rayHit);

        if (!hit) {
            return false;
        }

        // 转换到 Unity 风格的命中信息
        hitInfo.point = rayHit.HitData.Location;
        hitInfo.normal = rayHit.HitData.Normal;
        hitInfo.distance = rayHit.HitData.T;
        hitInfo.collider = rayHit.HitObject;
        return hitInfo.collider != null;
    }


    private void OnRelease() {
        this.Space = null;
    }
}