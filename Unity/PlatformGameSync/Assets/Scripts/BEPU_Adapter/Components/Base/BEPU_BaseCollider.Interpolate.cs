// 处理插值的事情


public abstract partial class BEPU_BaseCollider {
    private ILerpMethod _lerpper;

    void InitInterpolateState() {
        _lerpper = new LerpMethod_Interpolate();
        _lerpper.Init(this.entity, this, BEPU_PhysicsUpdater.PhysicsTimeStep);
    }

    void LateUpdate() // Unity's Update, runs every rendered frame
    {
        if (_lerpper == null) return;
        var (interPos, interRotation) = _lerpper.UpdateLearp();
        SyncPosAndRotation_ToTransform(interPos.ToUnityVector3(), interRotation.ToUnityQuaternion());
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