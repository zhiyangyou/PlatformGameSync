using System;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;
using Space = BEPUphysics.Space;
using Vector3 = BEPUutilities.Vector3;

public class BEPU_PhysicsManager : Singleton<BEPU_PhysicsManager> {
    public static readonly Vector3 DefaultGravity = new((Fix64)0, (Fix64)(-9.81f), (Fix64)0);
    private Space _bepuSpace { get; set; }

    public bool NeedAutoUpdate = true;

    private bool _hasInit = false;

    private HashSet<BEPU_BaseColliderLogic> _setAllColliders = new();
    public Vector3 SpaceGravity => Application.isPlaying ? _bepuSpace.ForceUpdater.Gravity : DefaultGravity;

    public override void Init() {
        if (!_hasInit) {
            _hasInit = true;
            Debug.Log("Editor下 BEPU 物理引擎Space被创建");
            _bepuSpace = new Space();
            _bepuSpace.ForceUpdater.Gravity = DefaultGravity;
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
        _bepuSpace.Update(dt);
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
            _bepuSpace.Add(collider.entity);
        }
        else {
            Debug.LogError($"AddEntity 失败, 因为重复了");
        }
    }

    public void RemoveEntity(BEPU_BaseColliderLogic collider) {
        if (_setAllColliders.Remove(collider)) {
            _bepuSpace.Remove(collider.entity);
        }
        else {
            Debug.LogError($"RemoveEntity 失败, Entity并没有加入到Space中 go:{collider.name}");
        }
    }


    private void OnRelease() {
        this._bepuSpace = null;
    }
}