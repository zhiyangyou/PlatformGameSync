using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEditor;
using UnityEngine;

public partial class BEPU_SphereColliderMono {
    private void OnDrawGizmos() {
        DrawGizmo(colliderLogic);
    }

    public static void DrawGizmo(BEPU_BaseColliderLogic baseCollider) {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.green;
        var center = baseCollider.entity.Position.ToUnityVector3();
        var shape = baseCollider.entityShape as SphereShape;
        Gizmos.DrawWireSphere(center, (float)shape.Radius);
        Gizmos.color = oldColor;
    }
}