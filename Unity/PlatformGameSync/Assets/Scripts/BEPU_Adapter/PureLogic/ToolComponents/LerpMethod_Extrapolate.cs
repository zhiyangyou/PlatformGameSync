// BepuInterpolatedTransform.cs

// using UnityEngine;

using BEPUphysics.Entities;
using FixMath.NET;
// using UnityEngine;
using Quaternion = BEPUutilities.Quaternion;
using FVector3 = BEPUutilities.Vector3; // Or whatever namespace Entity is in


/// <summary>
/// 根据当前Entity的状态, 推测下一次的位置和旋转姿态
/// </summary>
public class LerpMethod_Extrapolate : ILerpMethod {
    public void Init(Entity entity, BEPU_BaseColliderLogic baseCollider, Fix64 physicsTimeStep) { }
    public void BeforeWorldUpdate() { }
    public void AfterNextSTate() { }
    public (FVector3 interPos, Quaternion interRotation) UpdateLearp(Fix64 deltaTime) { }
}