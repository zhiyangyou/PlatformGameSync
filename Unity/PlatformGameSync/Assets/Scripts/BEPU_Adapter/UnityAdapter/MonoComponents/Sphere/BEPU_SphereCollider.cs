using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_SphereCollider : BEPU_BaseCollider {
    #region 属性和字段

    [SerializeField] private float radiu = 0.5f;

    public float Radiu {
        get => radiu;
        set {
            radiu = value;
            SyncAllAttrsToEntity();
        }
    }

    private SphereShape _sphereShape;

    public SphereShape sphereShape => _sphereShape ??= new SphereShape((Fix64)radiu);


    protected override void SyncAttrsToEntity() {
        sphereShape.Radius = RealRadiu;
    }

    protected override ConvexShape entityShape => sphereShape;

    public Fix64 RealRadiu => (Fix64)(radiu * Mathf.Max(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z));

    #endregion
}