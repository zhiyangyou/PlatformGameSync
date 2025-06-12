using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_CapsuleColliderMono : BEPU_BaseColliderMono {
    [SerializeField] private Fix64 radiu = Fix64.HalfOne;
    [SerializeField] private Fix64 length = Fix64.One;


    public BEPU_BaseColliderLogic _colliderLogic = null;

    public override BEPU_BaseColliderLogic colliderLogic => _colliderLogic ??= new BEPU_CapsuleColliderLogic(ColliderName, this, base.SyncEntityPosAndRotationToRenderer);

    protected override string ColliderName => $"{gameObject.name}_CapsuleCollider";

    public override void SyncExtendAttrsToEntity() {
        //  不能直接使用lossyScale这种序列化的浮点数作为定点物理世界的输入
        // var scaleRadiu = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        SyncExtendAttrsToEntity(this.colliderLogic, radiu, length);
    }


    public static void SyncExtendAttrsToEntity(BEPU_BaseColliderLogic colliderLogic, Fix64 realRadius, Fix64 realLen) {
        var capsuleShape = (colliderLogic.entityShape) as CapsuleShape;
        capsuleShape.Radius = realRadius;
        capsuleShape.Length = realLen;
    }

    protected override void OnRelease() { }
}