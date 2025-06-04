using UnityEngine;
using BEPUphysics.Entities; // For Entity

public class PhysicsSync : MonoBehaviour {
    public Entity BepuEntity { get; set; }

    void LateUpdate() // Use LateUpdate to ensure physics has been processed
    {
        if (BepuEntity != null) {
            transform.position = BepuEntity.Position.ToUnityVector3();
            transform.rotation = BepuEntity.Orientation.ToUnityQuaternion();
        }
    }
}