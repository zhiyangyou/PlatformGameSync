using System;
using FixMath.NET;
using UnityEngine;

public class BEPU_PhysicsUpdater : MonoBehaviour {
    private void Awake() {
        if (Application.isPlaying) {
            this.gameObject.name = "BEPUPhysicsUpdater";
        }
    } 

    public static Fix64 PhysicsTimeStep {
        get {
            if (Application.isPlaying) {
                return (Fix64)Time.fixedDeltaTime;
            }
            else {
                return (Fix64)0.066f;
            }
        }
    }

    private void FixedUpdate() {
        if (
#if UNITY_EDITOR
            Application.isPlaying
#endif
        ) {
            BEPU_PhysicsManager.Instance.UpdatePhysicsWorld(PhysicsTimeStep);
        }
    }
}