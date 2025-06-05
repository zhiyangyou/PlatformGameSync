using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;

public partial class BEPU_SphereCollider {
    private void OnDrawGizmos() {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.green;
        var center = entity.Position.ToUnityVector3();
        Gizmos.DrawWireSphere(center, (float)RealRadiu);
        Gizmos.color = oldColor;
    }
}