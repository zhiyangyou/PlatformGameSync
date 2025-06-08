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
    public override BEPU_BaseColliderLogic colliderLogic => _colliderLogic ??= new BEPU_SphereColliderLogic(ColliderName, base.SyncEntityPosAndRotationToRenderer);

    protected override string ColliderName => $"{gameObject.name}_ShpereCollider";

    protected override void SyncExtendAttrsToEntity() {
        var sphereShape = (colliderLogic.entityShape) as SphereShape;
        var scaleRadiu = Mathf.Max(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z);
        sphereShape.Radius = (Fix64)(scaleRadiu * radiu);
    }

    protected override void OnRelease() { }
}