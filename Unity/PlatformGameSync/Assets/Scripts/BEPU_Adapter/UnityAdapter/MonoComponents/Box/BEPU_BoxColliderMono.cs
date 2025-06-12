using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;


[ExecuteInEditMode]
public partial class BEPU_BoxColliderMono : BEPU_BaseColliderMono {
    [SerializeField] private Vector3 size = Vector3.one;


    public BEPU_BaseColliderLogic _colliderLogic = null;

    public override BEPU_BaseColliderLogic colliderLogic => _colliderLogic ??= new BEPU_BoxColliderLogic(ColliderName, this, base.SyncEntityPosAndRotationToRenderer);

    protected override string ColliderName => $"{gameObject.name}_BoxCollider";

    protected override void SyncExtendAttrsToEntity() {
        SyncExtendAttrsToEntity(colliderLogic as BEPU_BoxColliderLogic, this.size, transform);
    }

    public static void SyncExtendAttrsToEntity(BEPU_BoxColliderLogic boxCollider, Vector3 size, Transform transform) {
        var boxShape = (boxCollider.entityShape) as BoxShape;
        boxShape.Width = (Fix64)(size.x * transform.lossyScale.x);
        boxShape.Height = (Fix64)(size.y * transform.lossyScale.y);
        boxShape.Length = (Fix64)(size.z * transform.lossyScale.z);
    }


    protected override void OnRelease() { }
}