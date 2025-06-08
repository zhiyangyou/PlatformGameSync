using UnityEngine;
using System.Collections.Generic;
using FixMath.NET;
using UVector3 = UnityEngine.Vector3;
using UQuaternion = UnityEngine.Quaternion;

public class ManyBEPUObjs : MonoBehaviour {
    #region 属性和字段

    [Header("Scene Objects")] public GameObject dynamicCubePrefab; // Assign a Unity Cube Prefab here
    public GameObject sphereObj; // Assign a Unity Cube Prefab here
    public GameObject capsuleObj; // Assign a Unity Cube Prefab here
    public Transform groundTransform; // Assign a Unity Plane/Cube for visual ground

    [Header("Simulation Settings")] public int numberOfCubes = 10;
    public float cubeSpawnHeight = 10f;

    #endregion

    #region life-cycle

    private void Awake() {
        CreateDynamicObjs<BEPU_BoxColliderMono>(dynamicCubePrefab);
        CreateDynamicObjs<BEPU_SphereColliderMono>(sphereObj);
        CreateDynamicObjs<BEPU_CapsuleColliderMono>(capsuleObj);
    }

    #endregion

    #region private

    void CreateDynamicObjs<T>(GameObject goTemplate) where T : BEPU_BaseColliderMono {
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
            collider.entity.Mass = (Fix64)Random.Range(1f, 5f);
        }
    }

    #endregion
}