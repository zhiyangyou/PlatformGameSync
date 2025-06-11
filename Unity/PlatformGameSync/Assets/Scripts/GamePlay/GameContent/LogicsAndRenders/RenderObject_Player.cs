using UnityEngine;

public class RenderObject_Player : RenderObject {
    [SerializeField] private Vector3 size = Vector3.one;
    [SerializeField] private Animator animator;
    public Animator Animator => animator;

    public override void SyncExtendAttrsToEntity() {
        if (base.baseColliderType != null) {
            BEPU_BoxColliderMono.SyncBoxAttrsToEntity(baseColliderLogic as BEPU_BoxColliderLogic,
                size, transform);
        }
    }
}