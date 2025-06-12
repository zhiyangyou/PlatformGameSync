using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;


[ExecuteInEditMode]
public partial class BEPU_BoxColliderMono : BEPU_BaseColliderMono {
    [SerializeField] public FVector3 size = FVector3.One;


    public BEPU_BaseColliderLogic _colliderLogic = null;

    public override BEPU_BaseColliderLogic colliderLogic => _colliderLogic ??= new BEPU_BoxColliderLogic(ColliderName, this, base.SyncEntityPosAndRotationToRenderer);

    protected override string ColliderName => $"{gameObject.name}_BoxCollider";

    public override void SyncExtendAttrsToEntity() {
        SyncExtendAttrsToEntity(colliderLogic as BEPU_BoxColliderLogic, size);
    }

    public static void SyncExtendAttrsToEntity(BEPU_BoxColliderLogic boxCollider, FVector3 size) {
        var boxShape = (boxCollider.entityShape) as BoxShape;

        // 不能直接使用lossyScale这种序列化的浮点数作为定点物理世界的输入
        // boxShape.Width = (Fix64)(size.X * transform.lossyScale.x);
        // boxShape.Height = (Fix64)(size.Y * transform.lossyScale.y);
        // boxShape.Length = (Fix64)(size.Z * transform.lossyScale.z);

        boxShape.Width = size.X;
        boxShape.Height = size.Y;
        boxShape.Length = size.Z;
    }


    protected override void OnRelease() { }
}