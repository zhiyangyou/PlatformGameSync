using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_SphereColliderMono {
    private void OnDrawGizmos() {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.green;
        var center = this.colliderLogic.entity.Position.ToUnityVector3();
        var shape = this.colliderLogic.entityShape as SphereShape;
        Gizmos.DrawWireSphere(center, (float)shape.Radius);
        Gizmos.color = oldColor;
    }
}