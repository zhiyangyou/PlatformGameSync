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

    private static readonly FVector3 gravity = new((Fix64)0, (Fix64)(-9.81f), (Fix64)0);


    [Header("Scene Objects")] public GameObject dynamicCubePrefab; // Assign a Unity Cube Prefab here
    public Transform groundTransform; // Assign a Unity Plane/Cube for visual ground

    [Header("Simulation Settings")] public int numberOfCubes = 10;
    public float cubeSpawnHeight = 10f;

    private Space BepuSpace { get; set; }

    private List<PhysicsSync> _physicsSyncs = new List<PhysicsSync>();

    #endregion

    #region life-cycle

    private void Awake() {
        BepuSpace = new Space();
        BepuSpace.ForceUpdater.Gravity = gravity;
        CreateGround();
        CreateDynamicCubes();
    }


    void FixedUpdate() {
        BepuSpace.Update();
    }

    private void OnDestroy() {
        _physicsSyncs.Clear();
    }

    #endregion

    #region private

    void CreateGround() {
        if (groundTransform == null) {
            Debug.LogError("Ground Transform not assigned in BepuPhysicsV1Manager!");
            return;
        }

        var groundScale = groundTransform.localScale;
        var groundShape = new BoxShape(
            (Fix64)(groundScale.x),
            (Fix64)(groundScale.y),
            (Fix64)(groundScale.z)
        );

        // An Entity with no mass specified, or mass 0, is treated as kinematic/infinite mass.
        var groundEntity = new Entity(groundShape);
        groundEntity.Position = groundTransform.position.ToFixedVector3();
        groundEntity.Orientation = groundTransform.rotation.ToFixedQuaternion();
        // groundEntity.BecomeKinematic(); // 静止物体
        var syncObj = groundTransform.gameObject.AddComponent<PhysicsSync>();
        _physicsSyncs.Add(syncObj);
        BepuSpace.Add(groundEntity);
    }

    void CreateDynamicCubes() {
        if (dynamicCubePrefab == null) {
            Debug.LogError("Dynamic Cube Prefab not assigned in BepuPhysicsV1Manager!");
            return;
        }

        var boxShape = new BoxShape(Fix64.One, Fix64.One, Fix64.One); // 1x1x1 cube
        Fix64 mass = Fix64.One; // Mass of 1

        for (int i = 0; i < numberOfCubes; i++) {
            var bepuCube = new Entity(boxShape, mass);

            UVector3 initialPosition = new(Random.Range(-3f, 3f), cubeSpawnHeight + Random.Range(0f, 2f), Random.Range(-3f, 3f));
            UQuaternion initRotatuon = new UQuaternion((Random.Range(-3f, 3f)), Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            bepuCube.Position = initialPosition.ToFixedVector3();
            bepuCube.Orientation = initRotatuon.ToFixedQuaternion();
            BepuSpace.Add(bepuCube);

            GameObject unityCube = Instantiate(dynamicCubePrefab, initialPosition, initRotatuon);
            unityCube.name = $"BepuCube_{i}";
            PhysicsSync syncComponent = unityCube.AddComponent<PhysicsSync>();
            syncComponent.BepuEntity = bepuCube;

            _physicsSyncs.Add(syncComponent);
        }
    }

    #endregion
}