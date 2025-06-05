using UnityEngine;
using System.Collections.Generic;
using BEPUphysics.Entities;
using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using Space = BEPUphysics.Space;
using FVector3 = BEPUutilities.Vector3;
using UVector3 = UnityEngine.Vector3;
using UQuaternion = UnityEngine.Quaternion;

public class BepuPhysicsV1Manager : MonoBehaviour {
    #region 属性和字段

    [Header("Scene Objects")] public GameObject dynamicCubePrefab; // Assign a Unity Cube Prefab here
    public Transform groundTransform; // Assign a Unity Plane/Cube for visual ground

    [Header("Simulation Settings")] public int numberOfCubes = 10;
    public float cubeSpawnHeight = 10f;


    private List<BEPU_BoxCollider> _physicsSyncs = new();
    private BEPU_PhysicsManager _physicsWorld = null;

    #endregion

    #region life-cycle

    private void Awake() {
        _physicsWorld = BEPU_PhysicsManager.Instance;
        CreateGround();
        CreateDynamicCubes();
    }


    void FixedUpdate() {
        _physicsWorld.UpdatePhysicsWorld();
    }

    private void OnDestroy() {
        _physicsWorld = null;
        _physicsSyncs.Clear();
    }

    #endregion

    #region private

    void CreateGround() {
        if (groundTransform == null) {
            Debug.LogError("Ground Transform not assigned in BepuPhysicsV1Manager!");
            return;
        }

        BEPU_BoxCollider collider = groundTransform.gameObject.GetOrAddComponnet<BEPU_BoxCollider>();

        collider.EntityType = BEPU_EEntityType.Kinematic;
        _physicsSyncs.Add(collider);
        _physicsWorld.AddEntity(collider);
    }

    void CreateDynamicCubes() {
        if (dynamicCubePrefab == null) {
            Debug.LogError("Dynamic Cube Prefab not assigned in BepuPhysicsV1Manager!");
            return;
        }
        Fix64 mass = Fix64.One; // Mass of 1
        Debug.LogError("TODO Mass刚体属性");
        for (int i = 0; i < numberOfCubes; i++) {
            UVector3 initPos = new(Random.Range(-3f, 3f), cubeSpawnHeight + Random.Range(0f, 2f), Random.Range(-3f, 3f));
            UQuaternion initRotation = new UQuaternion((Random.Range(-3f, 3f)), Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            GameObject goCube = Instantiate(dynamicCubePrefab, initPos, initRotation);
            goCube.name = $"BepuCube_{i}";
            BEPU_BoxCollider collider = goCube.GetOrAddComponnet<BEPU_BoxCollider>();
            _physicsWorld.AddEntity(collider);
            _physicsSyncs.Add(collider);
        }
    }

    #endregion
}