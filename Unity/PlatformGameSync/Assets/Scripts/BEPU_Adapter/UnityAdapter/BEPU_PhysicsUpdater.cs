using System;
using FixMath.NET;
using UnityEngine;

public class BEPU_PhysicsUpdater : MonoBehaviour {
    private Fix64 _accLogicRealTimeS;

    private Fix64 _nextLogicFrameTimeS;

    private void Awake() {
        if (Application.isPlaying) {
            this.gameObject.name = "BEPUPhysicsUpdater";
        }
    }

    public static Fix64 PhysicsTimeStep {
        get {
            if (!Application.isPlaying) {
                return (Fix64)Time.fixedDeltaTime;
            }
            else {
                Fix64 fixedDeltaTime = Fix64.One / new Fix64((int)20); // 1秒15帧
                return fixedDeltaTime;
            }
        }
    }

    private void Update() {
        _accLogicRealTimeS += (Fix64)Time.deltaTime;
        while (_accLogicRealTimeS > _nextLogicFrameTimeS) {
            BEPU_PhysicsManagerUnity.Instance.UpdatePhysicsWorld(PhysicsTimeStep);
            _nextLogicFrameTimeS += PhysicsTimeStep;
        }
    }
}