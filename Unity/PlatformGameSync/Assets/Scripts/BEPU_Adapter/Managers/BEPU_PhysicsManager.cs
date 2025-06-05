using FixMath.NET;
using UnityEngine;
using Space = BEPUphysics.Space;
using Vector3 = BEPUutilities.Vector3;

public class BEPU_PhysicsManager : GoSingleton<BEPU_PhysicsManager> {
    private static readonly Vector3 gravity = new((Fix64)0, (Fix64)(-9.81f), (Fix64)0);
    private Space _bepuSpace { get; set; }

    public bool NeedAutoUpdate = true;

    public override void Init() {
        Debug.Log("Editor下 BEPU 物理引擎Space被创建");
        _bepuSpace = new Space();
        _bepuSpace.ForceUpdater.Gravity = gravity;
    }

    public void UpdatePhysicsWorld() {
        _bepuSpace.Update();
    }

    public void AddEntity(BEPU_BaseCollider collider) {
        _bepuSpace.Add(collider.entity);
    }

    private void FixedUpdate() {
        if (NeedAutoUpdate
#if UNITY_EDITOR
            && Application.isPlaying
#endif
           ) {
            UpdatePhysicsWorld();
        }
    }
}