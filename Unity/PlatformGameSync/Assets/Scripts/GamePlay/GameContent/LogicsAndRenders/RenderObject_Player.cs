using UnityEngine;

public class RenderObject_Player : RenderObject {
    [SerializeField] private Vector3 size = Vector3.one;

    public override void SyncExtendAttrsToEntity() {
        if (base.baseColliderType != null) {
            BEPU_BoxColliderMono.SyncBoxAttrsToEntity(baseColliderLogic as BEPU_BoxColliderLogic,
                size, transform);
        }
    }
}