// BepuInterpolatedTransform.cs

// using UnityEngine;

using BEPUphysics.Entities;
using FixMath.NET;
// using UnityEngine;
using Quaternion = BEPUutilities.Quaternion;
using FVector3 = BEPUutilities.Vector3; // Or whatever namespace Entity is in


/// <summary>
/// 渲染会始终滞后于真实的物体物理的位置, 渲染会一直处于追赶真实的物理位置的状态
/// </summary>
public class LerpMethod_Interpolate : ILerpMethod {
    private Fix64 _physicsTimeStep = Fix64.Zero;
    private Fix64 _accumulator = Fix64.Zero;
    private Entity PhysicsEntity { get; set; }
    private BEPU_BaseColliderLogic _collider { get; set; }

    // Stores the transform from the PREVIOUS physics update (fixed-point)
    private BEPUutilities.Vector3 _previousPositionFP;
    private BEPUutilities.Quaternion _previousOrientationFP;

    // Stores the transform from the CURRENT physics update (TARGET state) (fixed-point)
    private BEPUutilities.Vector3 _targetPositionFP;
    private BEPUutilities.Quaternion _targetOrientationFP;

    // Called once after the entity is created and linked
    public Fix64 LerpAccumulator => _accumulator;


    public void Init(Entity entity, BEPU_BaseColliderLogic baseCollider, Fix64 physicsTimeStep) {
        _physicsTimeStep = physicsTimeStep;
        _collider = baseCollider;
        PhysicsEntity = entity;
        if (PhysicsEntity == null) {
            BEPU_Logger.LogError("PhysicsEntity is null on InitializeStates ");
            return;
        }

        _targetPositionFP = PhysicsEntity.Position;
        _targetOrientationFP = PhysicsEntity.Orientation;
        _previousPositionFP = _targetPositionFP;
        _previousOrientationFP = _targetOrientationFP;
    }


    public void BeforeWorldUpdate() {
        if (PhysicsEntity == null) return;
        _accumulator = Fix64.Zero;
        _previousPositionFP = _targetPositionFP;
        _previousOrientationFP = _targetOrientationFP;
    }

    public void AfterWorldUpdate() {
        if (PhysicsEntity == null) return;
        _targetPositionFP = PhysicsEntity.Position;
        _targetOrientationFP = PhysicsEntity.Orientation;
    }

    public (FVector3 interPos, Quaternion interRotation) UpdateLearp(Fix64 deltaTime) {
        _accumulator += deltaTime;
        Fix64 alpha = _accumulator / _physicsTimeStep;
        var newAlpha = Fix64.Clamp01(alpha);
        alpha = (Fix64)newAlpha;
        return DoLerp(alpha);
    }

    private (FVector3 interPos, BEPUutilities.Quaternion interRotation) DoLerp(Fix64 alpha) // alpha is a value from 0 to 1
    {
        if (PhysicsEntity == null) return (default, default);
        return (FVector3.Lerp(_previousPositionFP, _targetPositionFP, alpha),
            UnityEngine.Quaternion.Lerp(_previousOrientationFP.ToUnityQuaternion(), _targetOrientationFP.ToUnityQuaternion(), (float)alpha).ToFixedQuaternion());
    }
}