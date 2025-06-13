// BepuInterpolatedTransform.cs

// using UnityEngine;

using BEPUphysics.Entities;
using FixMath.NET;
using UnityEngine;
// using UnityEngine;
using Quaternion = BEPUutilities.Quaternion;
using Vector3 = BEPUutilities.Vector3; // Or whatever namespace Entity is in
using FVector3 = BEPUutilities.Vector3;
using Space = BEPUphysics.Space; // Or whatever namespace Entity is in


/// <summary>
/// 根据当前Entity的状态, 推测下一次的位置和旋转姿态
/// </summary>
public class LerpMethod_Extrapolate : ILerpMethod {
    private struct RigidbodyState {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 angularVelocity;
        public Fix64 physicsTime;
    }

    private RigidbodyState previousState;
    private RigidbodyState currentState;
    private bool hasPreviousState;

    private Fix64 _physicsTotalTime;
    private Fix64 _renderTotalTime;

    private Space _space;
    private Entity _entity;

    // 在物理更新时调用，记录状态
    public void RecordState(Entity rb, Fix64 physicsTime) {
        if (!hasPreviousState) {
            previousState = new RigidbodyState {
                position = rb.Position,
                rotation = rb.Orientation,
                velocity = rb.LinearVelocity,
                angularVelocity = rb.AngularVelocity,
                physicsTime = physicsTime
            };
            hasPreviousState = true;
            return;
        }

        // 将当前状态移到前一状态
        previousState = currentState;

        // 记录新状态
        currentState = new RigidbodyState {
            position = rb.Position,
            rotation = rb.Orientation,
            velocity = rb.LinearVelocity,
            angularVelocity = rb.AngularVelocity,
            physicsTime = physicsTime
        };
    }

    // 在渲染帧中调用，获取插值后的姿态
    public void GetExtrapolatedPose(Fix64 renderTime, out Vector3 position, out Quaternion rotation) {
        if (!hasPreviousState) {
            position = currentState.position;
            rotation = currentState.rotation;
            return;
        }

        // 计算从上次物理更新到当前渲染时间的时间差
        Fix64 timeSinceLastPhysicsUpdate = renderTime - currentState.physicsTime;
        // 外推位置 = 当前位置 + 速度 * 时间差
        position = currentState.position + currentState.velocity * timeSinceLastPhysicsUpdate;

        // 外推旋转 = 当前旋转 * 角速度旋转
        Quaternion angularRotation = (currentState.angularVelocity * Fix64.Rad2Deg * timeSinceLastPhysicsUpdate).ToQuaternion();
        rotation = currentState.rotation * angularRotation;
    }

    public void Init(Entity entity, BEPU_BaseColliderLogic baseCollider, Fix64 physicsTimeStep) {
        _physicsTotalTime = Fix64.Zero;
        _renderTotalTime = Fix64.Zero;
        _space = entity.Space;
        _entity = entity;
        if (_entity == null) {
            BEPU_Logger.LogError("_entity为空");
        }
    }

    public void BeforeWorldUpdate() { }

    public void AfterWorldUpdate() {
        RecordState(_entity, _physicsTotalTime);
        _physicsTotalTime += _space.TimeStepSettings.TimeStepDuration;
    }

    public (FVector3 interPos, Quaternion interRotation) UpdateLearp(Fix64 deltaTime) {
        _renderTotalTime += deltaTime;
        GetExtrapolatedPose(_renderTotalTime, out var pos, out var rot);
        return (pos, rot);
    }
}