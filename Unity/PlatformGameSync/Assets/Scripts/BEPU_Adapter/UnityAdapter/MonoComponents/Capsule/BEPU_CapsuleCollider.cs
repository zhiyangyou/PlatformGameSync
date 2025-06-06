using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_CapsuleCollider : BEPU_BaseCollider {
    #region 属性和字段

    [SerializeField] private float radiu = 0.5f;
    [SerializeField] private float length = 1f;

    public float Radiu {
        get => radiu;
        set {
            radiu = value;
            SyncAllAttrsToEntity();
        }
    }

    public float Length {
        get => length;
        set {
            length = value;
            SyncAllAttrsToEntity();
        }
    }

    private CapsuleShape _capsuleShape;

    public CapsuleShape capsuleShape => _capsuleShape ??= new CapsuleShape((Fix64)length, (Fix64)radiu);

    protected override void SyncAttrsToEntity() {
        capsuleShape.Radius = (Fix64)RealRadiu;
        capsuleShape.Length = (Fix64)RealLength;
    }

    protected override ConvexShape entityShape => capsuleShape;
    public Fix64 RealRadiu => (Fix64)(radiu * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z));
    public Fix64 RealLength => (Fix64)(length * transform.lossyScale.y);

    #endregion
}