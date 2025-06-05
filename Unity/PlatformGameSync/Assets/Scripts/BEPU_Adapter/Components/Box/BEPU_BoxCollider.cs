using UnityEngine;
using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;


[ExecuteInEditMode]
public partial class BEPU_BoxCollider : BEPU_BaseCollider {
    #region 属性和字段

    [SerializeField] private Vector3 size = Vector3.one;

    public Vector3 Size {
        get { return size; }
        set {
            size = value;
            SyncAttrsToEntity();
        }
    }


    private BoxShape _boxShape;

    public BoxShape boxShape => _boxShape ??= new BoxShape(Width, Height, Length);


    protected override void SyncAttrsToEntity() {
        boxShape.Width = this.Width;
        boxShape.Height = this.Height;
        boxShape.Length = this.Length;
    }

    protected override ConvexShape entityShape => boxShape;

    public Fix64 Width => (Fix64)(size.x * transform.lossyScale.x);
    public Fix64 Height => (Fix64)(size.y * transform.lossyScale.y);
    public Fix64 Length => (Fix64)(size.z * transform.lossyScale.z);

    #endregion
}