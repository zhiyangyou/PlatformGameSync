// 处理插值的事情


using FixMath.NET;
using UnityEngine;

public abstract partial class BEPU_BaseCollider {
    public Fix64 PhysicsTimeStep => BEPU_PhysicsUpdater.PhysicsTimeStep; // 66ms
    private Fix64 _accumulator = Fix64.Zero;

    private LerpMethod_Inyterpolate _bepuInterpolated;

    void InitInterpolateState() {
        _bepuInterpolated = new LerpMethod_Inyterpolate();
        _bepuInterpolated.Init(this.entity, this);
    }

    void LateUpdate() // Unity's Update, runs every rendered frame
    {
        if (_bepuInterpolated == null) return;
        _accumulator += (Fix64)Time.deltaTime;

        // Fixed physics steps
        // while (_accumulator >= PhysicsTimeStep) {
        //     // 1. Store previous states for all interpolated objects
        //     // _bepuInterpolated.StorePreviousState();
        //
        //
        //     // 2. Update BEPUphysics
        //     // BepuSpace.Update(PhysicsTimeStep); // Use your fixed timestep
        //
        //     // 3. Update target states for all interpolated objects
        //     // _bepuInterpolated.UpdateTargetState();
        //
        //     _accumulator -= PhysicsTimeStep;
        //     // _accumulator =(Fix64)1f;
        // }

        // 4. Interpolate for rendering
        Fix64 alpha = _accumulator / PhysicsTimeStep;
        var newAlpha = Mathf.Clamp01((float)alpha);
        alpha = (Fix64)newAlpha;
        (var interPos, var interRotation) = _bepuInterpolated.DoLerp(alpha);
        SyncPosAndRotation_ToTransform(interPos.ToUnityVector3(), interRotation.ToUnityQuaternion());
    }

    public void OnBeforeUpdate() {
        if (_bepuInterpolated == null) return;
        _bepuInterpolated.StoreCurState();
        _accumulator = (Fix64)0f;
    }

    public void OnAfterUpdate() {
        if (_bepuInterpolated == null) return;
        _bepuInterpolated.StoreNextSTate();
    }
}