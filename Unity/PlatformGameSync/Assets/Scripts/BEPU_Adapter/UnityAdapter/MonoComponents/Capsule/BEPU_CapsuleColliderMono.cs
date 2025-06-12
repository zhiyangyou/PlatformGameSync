using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_CapsuleColliderMono : BEPU_BaseColliderMono {
    [SerializeField] private float radiu = 0.5f;
    [SerializeField] private float length = 1f;


    public BEPU_BaseColliderLogic _colliderLogic = null;

    public override BEPU_BaseColliderLogic colliderLogic => _colliderLogic ??= new BEPU_CapsuleColliderLogic(ColliderName, this, base.SyncEntityPosAndRotationToRenderer);

    protected override string ColliderName => $"{gameObject.name}_CapsuleCollider";

    protected override void SyncExtendAttrsToEntity() {
        var scaleRadiu = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        var realRadiu = (Fix64)(scaleRadiu * radiu);
        var realLen = (Fix64)(transform.lossyScale.y * length);
        SyncExtendAttrsToEntity(this.colliderLogic, realRadiu, realLen);
    }


    public static void SyncExtendAttrsToEntity(BEPU_BaseColliderLogic colliderLogic, Fix64 realRadius, Fix64 realLen) {
        var capsuleShape = (colliderLogic.entityShape) as CapsuleShape;
        capsuleShape.Radius = realRadius;
        capsuleShape.Length = realLen;
    }

    protected override void OnRelease() { }
}