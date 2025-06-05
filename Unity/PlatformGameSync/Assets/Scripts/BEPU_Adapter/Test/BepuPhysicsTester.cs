using UnityEngine;
using System.Collections.Generic;
using FixMath.NET;
using UVector3 = UnityEngine.Vector3;
using UQuaternion = UnityEngine.Quaternion;

public class BepuPhysicsTester : MonoBehaviour {
    #region 属性和字段

    [Header("Scene Objects")] public GameObject dynamicCubePrefab; // Assign a Unity Cube Prefab here
    public GameObject sphereObj; // Assign a Unity Cube Prefab here
    public Transform groundTransform; // Assign a Unity Plane/Cube for visual ground

    [Header("Simulation Settings")] public int numberOfCubes = 10;
    public float cubeSpawnHeight = 10f;


    private BEPU_PhysicsManager _physicsWorld = null;

    #endregion

    #region life-cycle

    private void Awake() {
        _physicsWorld = BEPU_PhysicsManager.Instance;
        CreateGround();
        CreateDynamicObjs<BEPU_BoxCollider>(dynamicCubePrefab);
        CreateDynamicObjs<BEPU_SphereCollider>(sphereObj);
    }


    void FixedUpdate() {
        _physicsWorld.UpdatePhysicsWorld();
    }

    private void OnDestroy() {
        _physicsWorld = null;
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
        _physicsWorld.AddEntity(collider);
    }

    void CreateDynamicObjs<T>(GameObject goTemplate) where T : BEPU_BaseCollider {
        if (goTemplate == null) {
            Debug.LogError($"CreateDynamicObjs tempObj is null ");
            return;
        }
        for (int i = 0; i < numberOfCubes; i++) {
            UVector3 initPos = new(Random.Range(-3f, 3f), cubeSpawnHeight + Random.Range(0f, 2f), Random.Range(-3f, 3f));
            UQuaternion initRotation = new UQuaternion((Random.Range(-3f, 3f)), Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            GameObject goObj = Instantiate(goTemplate, initPos, initRotation);
            goObj.name = $"{goTemplate.name}_{i}";
            T collider = goObj.GetOrAddComponnet<T>();
            collider.Mass = Random.Range(1f, 5f);
            _physicsWorld.AddEntity(collider);
        }
    }

    #endregion
}