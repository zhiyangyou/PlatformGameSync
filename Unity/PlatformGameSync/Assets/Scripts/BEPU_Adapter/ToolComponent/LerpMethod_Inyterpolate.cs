// BepuInterpolatedTransform.cs

using UnityEngine;
using BEPUphysics.Entities;
using FixMath.NET; // Or whatever namespace Entity is in


/// <summary>
/// 渲染会始终滞后于真实的物体物理的位置, 一直处于追赶的状态
/// </summary>
public class LerpMethod_Inyterpolate {
    public Entity PhysicsEntity { get; set; }
    BEPU_BaseCollider _collider { get; set; }

    // Stores the transform from the PREVIOUS physics update (fixed-point)
    private BEPUutilities.Vector3 _previousPositionFP;
    private BEPUutilities.Quaternion _previousOrientationFP;

    // Stores the transform from the CURRENT physics update (TARGET state) (fixed-point)
    private BEPUutilities.Vector3 _targetPositionFP;
    private BEPUutilities.Quaternion _targetOrientationFP;

    // Called once after the entity is created and linked
    public void Init(Entity entity, BEPU_BaseCollider baseCollider) {
        _collider = baseCollider;
        PhysicsEntity = entity;
        if (PhysicsEntity == null) {
            Debug.LogError("PhysicsEntity is null on InitializeStates ");
            return;
        }

        _targetPositionFP = PhysicsEntity.Position;
        _targetOrientationFP = PhysicsEntity.Orientation;
        _previousPositionFP = _targetPositionFP;
        _previousOrientationFP = _targetOrientationFP;

        _collider.SyncPosAndRotation_ToTransform();
    }

    public void StoreCurState() {
        if (PhysicsEntity == null) return;
        _previousPositionFP = _targetPositionFP;
        _previousOrientationFP = _targetOrientationFP;
        // if (_collider.gameObject.name == "Player") {
        //     Debug.LogError($"Frame:{Time.frameCount} store >>>>> previous:{_previousPositionFP}");
        // }
    }

    public void StoreNextSTate() {
        if (PhysicsEntity == null) return;
        _targetPositionFP = PhysicsEntity.Position;
        _targetOrientationFP = PhysicsEntity.Orientation;
        // if (_collider.gameObject.name == "Player") {
        //     Debug.LogError($"Frame:{Time.frameCount} next <<<<< target:{_targetPositionFP}");
        // }
    }

    public (BEPUutilities.Vector3 interPos, BEPUutilities.Quaternion interRotation) DoLerp(Fix64 alpha) // alpha is a value from 0 to 1
    {
        if (PhysicsEntity == null) return (default, default);

        var retPos = BEPUutilities.Vector3.Lerp(_previousPositionFP, _targetPositionFP, alpha);
        BEPUutilities.Quaternion.Slerp(ref _previousOrientationFP, ref _targetOrientationFP, alpha, out var retRot);
        // if (_collider.gameObject.name == "Player") {
        //     var strAlpha = ((float)alpha).ToString("F2");
        //     Debug.LogError($"Frame:{Time.frameCount} alpha:{strAlpha} p1:{_previousPositionFP} p2:{_targetPositionFP} retPos:{retPos}");
        // }
        return (retPos, retRot);
    }
}