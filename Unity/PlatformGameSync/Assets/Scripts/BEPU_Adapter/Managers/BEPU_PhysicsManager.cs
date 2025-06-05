using System;
using FixMath.NET;
using UnityEngine;
using Space = BEPUphysics.Space;
using Vector3 = BEPUutilities.Vector3;

public class BEPU_PhysicsManager : GoSingleton<BEPU_PhysicsManager> {
    public static readonly Vector3 DefaultGravity = new((Fix64)0, (Fix64)(-9.81f), (Fix64)0);
    private Space _bepuSpace { get; set; }

    public bool NeedAutoUpdate = true;

    private bool _hasInit = false;

    public Vector3 SpaceGravity => Application.isPlaying ? _bepuSpace.ForceUpdater.Gravity : DefaultGravity;

    public override void Init() {
        if (!_hasInit) {
            _hasInit = true;
            Debug.Log("Editor下 BEPU 物理引擎Space被创建");
            _bepuSpace = new Space();
            _bepuSpace.ForceUpdater.Gravity = DefaultGravity;
            // _bepuSpace.SimulationSettings.CollisionDetection.AllowContactGenerationBetweenKinematics = true;
            // _bepuSpace.SimulationSettings.CollisionDetection.UseFullCCDResolution = true;
        }
    }


    public void UpdatePhysicsWorld(float dt) {
        _bepuSpace.Update((Fix64)dt);
    }

    public void AddEntity(BEPU_BaseCollider collider) {
        _bepuSpace.Add(collider.entity);
    }

#if true
    private void FixedUpdate() {
        if (NeedAutoUpdate
#if UNITY_EDITOR
            && Application.isPlaying
#endif
           ) {
            UpdatePhysicsWorld(Time.fixedDeltaTime);
        }
    }

#else
    private void Update() {
        if (NeedAutoUpdate
#if UNITY_EDITOR
            && Application.isPlaying
#endif
           ) {
            UpdatePhysicsWorld(Time.deltaTime);
        }
    }
#endif


    private void OnDestroy() {
        this._bepuSpace = null;
    }
}