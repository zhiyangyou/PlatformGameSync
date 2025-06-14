// 处理插值的事情


using System;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_BaseColliderLogic {
    private ILerpMethod _lerpper;

    public void InitInterpolateState(BEPU_LerpMethod method) {
        _lerpper = LerpMethodFactory.NewMethod(method);
        _lerpper.Init(this.entity, this, BEPU_PhysicsUpdater.PhysicsTimeStep);
    }

    public void DoPositionInterpolateUpdate(Fix64 renderDeltaTime) // Unity's Update, runs every rendered frame
    {
        if (_lerpper == null) return;
        var (interPos, interRotation) = _lerpper.UpdateLearp(renderDeltaTime);
        _syncEntityPosAndRotationToRenderer?.Invoke(interPos, interRotation);
    }

    public void OnBeforeUpdate() {
        if (_lerpper == null) return;
        _lerpper.BeforeWorldUpdate();
    }

    public void OnAfterUpdate() {
        if (_lerpper == null) return;
        _lerpper.AfterWorldUpdate();
    }
}