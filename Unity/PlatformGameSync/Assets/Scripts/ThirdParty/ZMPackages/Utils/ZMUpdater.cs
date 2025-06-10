using System;

public class ZMUpdater : ZM.ZMAsset.MonoSingleton<ZMUpdater> {
    private Action _onUpdate;

    public void AddUpdateListener(Action onUpdate) {
        _onUpdate += onUpdate;
    }

    public void RemoveUpdateListener(Action onUpdate) {
        _onUpdate -= onUpdate;
    }

    private void Update() {
        _onUpdate?.Invoke();
    }
}