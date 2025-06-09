using System;
using System.Collections.Generic;
using BEPUphysics.CollisionRuleManagement;
using FixMath.NET;
using Space = BEPUphysics.Space;
using Vector3 = BEPUutilities.Vector3;


public abstract partial class BEPU_PhysicsManagerLogic<T> : Singleton<T> where T : new() {
    public Space Space { get; private set; }

    public bool NeedAutoUpdate = true;

    private bool _hasInit = false;

    private HashSet<BEPU_BaseColliderLogic> _setAllColliders = new();


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

            BEPU_Logger.Log("Editor下 BEPU 物理引擎Space被创建");

            Space = new Space();
            Space.ForceUpdater.Gravity = BEPU_DefaultAttr.DefaultGravity;

            var layerCount = (int)BEPU_LayerDefine.LayerCount;
            for (int i = 0; i < layerCount; i++) {
                BEPU_LayerDefine layer = (BEPU_LayerDefine)i;
                _dicGroup.Add(layer, new CollisionGroup());
            }

            ExtentInit();
        }
    }

    public void SetLayerMatrix(BEPU_LayerMatrix matrix) {
        var layerCount = (int)BEPU_LayerDefine.LayerCount;
        for (int i = 0; i < layerCount; i++) {
            BEPU_LayerDefine layerA = (BEPU_LayerDefine)i;
            for (int j = 0; j < layerCount; j++) {
                BEPU_LayerDefine layerB = (BEPU_LayerDefine)j;
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
                BEPU_Logger.LogError("OnBeforeUpdate 回调报错");
                BEPU_Logger.LogException(e);
            }
        }
        Space.Update(dt);
        foreach (var collider in _setAllColliders) {
            try {
                collider.OnAfterUpdate();
            }
            catch (Exception e) {
                BEPU_Logger.LogError("OnBeforeUpdate 回调报错");
                BEPU_Logger.LogException(e);
            }
        }
    }

    public void AddEntity(BEPU_BaseColliderLogic collider) {
        if (_setAllColliders.Add(collider)) {
            Space.Add(collider.entity);
        }
        else {
            BEPU_Logger.LogError($"AddEntity 失败, 因为重复了");
        }
    }

    public void RemoveEntity(BEPU_BaseColliderLogic collider) {
        if (_setAllColliders.Remove(collider)) {
            Space.Remove(collider.entity);
        }
        else {
            BEPU_Logger.LogError($"RemoveEntity 失败, Entity并没有加入到Space中 go:{collider.name}");
        }
    }




    public virtual void OnRelease() {
        this.Space = null;
    }

    public abstract void ExtentInit();
}