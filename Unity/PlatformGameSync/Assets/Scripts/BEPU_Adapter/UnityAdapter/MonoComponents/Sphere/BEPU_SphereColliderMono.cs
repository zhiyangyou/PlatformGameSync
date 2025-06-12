using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_SphereColliderMono : BEPU_BaseColliderMono {
    [SerializeField] private Fix64 radiu = Fix64.HalfOne;

    public BEPU_BaseColliderLogic _colliderLogic = null;
    public override BEPU_BaseColliderLogic colliderLogic => _colliderLogic ??= new BEPU_SphereColliderLogic(ColliderName, this, base.SyncEntityPosAndRotationToRenderer);

    protected override string ColliderName => $"{gameObject.name}_ShpereCollider";

    public override void SyncExtendAttrsToEntity() {
        // 不能直接使用lossyScale这种序列化的浮点数作为定点物理世界的输入
        // var scaleRadiu = Mathf.Max(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z);
        // var realRadiu = (Fix64)(scaleRadiu * radiu);
        SyncExtendAttrsToEntity(colliderLogic, radiu);
    }

    public static void SyncExtendAttrsToEntity(BEPU_BaseColliderLogic colliderLogic, Fix64 radiu) {
        var sphereShape = (colliderLogic.entityShape) as SphereShape;
        sphereShape.Radius = radiu;
    }

    protected override void OnRelease() { }
}