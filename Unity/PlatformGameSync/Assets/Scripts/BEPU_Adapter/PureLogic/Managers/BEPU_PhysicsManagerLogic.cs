using System;
using System.Collections.Generic;
using BEPUphysics;
using BEPUphysics.CollisionRuleManagement;
using Codice.CM.Common;
using FixMath.NET;
using Space = BEPUphysics.Space;
using Vector3 = BEPUutilities.Vector3;


public abstract class BEPU_PhysicsManagerLogic<T> : Singleton<T> where T : new() {
    public Space Space { get; private set; }

    public bool NeedAutoUpdate = true;

    private bool _hasInit = false;

    private HashSet<BEPU_BaseColliderLogic> _setAllColliders = new();
    private Dictionary<BEPU_LayerDefaine, CollisionGroup> _dicGroup = new();

    public Vector3 SpaceGravity {
        get {
            if (Space == null) {
                return BEPU_DefaultAttr.DefaultGravity;
            }
            else {
                return Space.ForceUpdater.Gravity;
            }
        }
    }

    public override void Init() {
        if (!_hasInit) {
            _hasInit = true;
#if UNITY_2022_3_OR_NEWER
            UnityEngine.Debug.Log("Editor下 BEPU 物理引擎Space被创建");
#endif
            Space = new Space();
            Space.ForceUpdater.Gravity = BEPU_DefaultAttr.DefaultGravity;

            var layerCount = (int)BEPU_LayerDefaine.LayerCount;
            for (int i = 0; i < layerCount; i++) {
                BEPU_LayerDefaine layer = (BEPU_LayerDefaine)i;
                _dicGroup.Add(layer, new CollisionGroup());
            }

            ExtentInit();
        }
    }

    public void SetLayerMatrix(BEPU_LayerMatrix matrix) {
        var layerCount = (int)BEPU_LayerDefaine.LayerCount;
        for (int i = 0; i < layerCount; i++) {
            BEPU_LayerDefaine layerA = (BEPU_LayerDefaine)i;
            for (int j = 0; j < layerCount; j++) {
                BEPU_LayerDefaine layerB = (BEPU_LayerDefaine)j;
                var groupA = _dicGroup[layerA];
                var groupB = _dicGroup[layerB];
                var needCollision = matrix.Get(layerA, layerB);
                CollisionRule rule = needCollision ? CollisionRule.Defer : CollisionRule.NoSolver;
                CollisionGroup.DefineCollisionRule(groupA, groupB, rule);
            }
        }
    }


    public void UpdatePhysicsWorld(Fix64 dt) {
        foreach (var collider in _setAllColliders) {
            try {
                collider.OnBeforeUpdate();
            }
            catch (Exception e) {
#if UNITY_2022_3_OR_NEWER

                UnityEngine.Debug.LogError("OnBeforeUpdate 回调报错");
                UnityEngine.Debug.LogException(e);
#endif
            }
        }
        Space.Update(dt);
        foreach (var collider in _setAllColliders) {
            try {
                collider.OnAfterUpdate();
            }
            catch (Exception e) {
#if UNITY_2022_3_OR_NEWER
                UnityEngine.Debug.LogError("OnBeforeUpdate 回调报错");
                UnityEngine.Debug.LogException(e);
#endif
            }
        }
    }

    public void AddEntity(BEPU_BaseColliderLogic collider) {
        if (_setAllColliders.Add(collider)) {
            Space.Add(collider.entity);
        }
        else {
#if UNITY_2022_3_OR_NEWER
            UnityEngine.Debug.LogError($"AddEntity 失败, 因为重复了");
#endif
        }
    }

    public void RemoveEntity(BEPU_BaseColliderLogic collider) {
        if (_setAllColliders.Remove(collider)) {
            Space.Remove(collider.entity);
        }
        else {
#if UNITY_2022_3_OR_NEWER
            UnityEngine.Debug.LogError($"RemoveEntity 失败, Entity并没有加入到Space中 go:{collider.name}");
#endif
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


    public virtual void OnRelease() {
        this.Space = null;
    }

    public abstract void ExtentInit();
}