using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_CapsuleColliderMono : BEPU_BaseColliderMono {
    [SerializeField] private float radiu = 0.5f;
    [SerializeField] private float length = 1f;

    

    public BEPU_BaseColliderLogic _colliderLogic = null;

    public override BEPU_BaseColliderLogic colliderLogic => _colliderLogic ??= new BEPU_CapsuleColliderLogic(ColliderName, base.SyncEntityPosAndRotationToRenderer);

    protected override string ColliderName => $"{gameObject.name}_CapsuleCollider";

    protected override void SyncExtendAttrsToEntity() {
        var capsuleShape = (this.colliderLogic.entityShape) as CapsuleShape;
        var scaleRadiu = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        capsuleShape.Radius = (Fix64)(scaleRadiu * radiu);
        capsuleShape.Length = (Fix64)(transform.lossyScale.y * length);
    }

    protected override void OnRelease() { }
}