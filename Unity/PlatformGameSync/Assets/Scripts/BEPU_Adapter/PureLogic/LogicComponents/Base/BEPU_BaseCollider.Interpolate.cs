// 处理插值的事情


using UnityEngine;

public partial class BEPU_BaseColliderLogic {
    private ILerpMethod _lerpper;

    public void InitInterpolateState() {
        _lerpper = new LerpMethod_Interpolate();
        _lerpper.Init(this.entity, this, BEPU_PhysicsUpdater.PhysicsTimeStep);
    }

    public void DoPositionInterpolateUpdate() // Unity's Update, runs every rendered frame
    {
        if (_lerpper == null) return;
        var (interPos, interRotation) = _lerpper.UpdateLearp();
        _syncEntityPosAndRotationToRenderer?.Invoke(interPos, interRotation);
    }

    public void OnBeforeUpdate() {
        if (_lerpper == null) return;
        _lerpper.StoreCurState();
    }

    public void OnAfterUpdate() {
        if (_lerpper == null) return;
        _lerpper.StoreNextSTate();
    }
}