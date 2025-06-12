using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_SphereColliderMono : BEPU_BaseColliderMono {
    [SerializeField] private float radiu = 0.5f;


    public float Radiu {
        get => radiu;
        set {
            radiu = value;
            SyncAllAttrsToEntity();
        }
    }

    public BEPU_BaseColliderLogic _colliderLogic = null;
    public override BEPU_BaseColliderLogic colliderLogic => _colliderLogic ??= new BEPU_SphereColliderLogic(ColliderName, this, base.SyncEntityPosAndRotationToRenderer);

    protected override string ColliderName => $"{gameObject.name}_ShpereCollider";

    protected override void SyncExtendAttrsToEntity() {
        var scaleRadiu = Mathf.Max(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z);
        var realRadiu = (Fix64)(scaleRadiu * radiu);
        SyncExtendAttrsToEntity(colliderLogic, realRadiu);
    }

    public static void SyncExtendAttrsToEntity(BEPU_BaseColliderLogic colliderLogic, Fix64 radiu) {
        var sphereShape = (colliderLogic.entityShape) as SphereShape;
        sphereShape.Radius = radiu;
    }

    protected override void OnRelease() { }
}