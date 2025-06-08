#if UNITY_EDITOR

using BEPUphysics.CollisionShapes.ConvexShapes;
using UnityEditor;

[CustomEditor(typeof(BEPU_SphereColliderMono))]
[CanEditMultipleObjects]
public class BEPU_SphereColliderEditor : BEPU_BaseColliderEditor<BEPU_SphereColliderMono> {
    #region 属性和字段

    private SerializedProperty radiuProp;

    private SphereShape _sphereShape;

    #endregion

    void OnEnable() {
        base.DoInit();
        radiuProp = serializedObject.FindProperty("radiu");
        _sphereShape = (this.target as BEPU_SphereColliderMono).colliderLogic.entityShape as SphereShape;
    }

    void OnDisable() {
        base.DoUninit();
    }

    protected override void DrawBaseAttr() {
        base.DrawBaseAttr();
        EditorGUILayout.PropertyField(radiuProp);
    }


    protected override void DrawDebugAttrs() {
        base.DrawDebugAttrs();
        EditorGUILayout.LabelField($"物理半径:({((float)_sphereShape.Radius).ToString(kStrFloatFormat)})");
    }


    public override void OnInspectorGUI() {
        DoOnInspectorGUI();
    }
}

#endif