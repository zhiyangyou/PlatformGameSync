#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(BEPU_SphereCollider))]
[CanEditMultipleObjects]
public class BEPU_SphereColliderEditor : BEPU_BaseColliderEditor<BEPU_SphereCollider> {
    #region 属性和字段

    private SerializedProperty radiuProp;

    #endregion

    void OnEnable() {
        base.DoInit();
        radiuProp = serializedObject.FindProperty("radiu");
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

        var shape = collider.sphereShape;
        EditorGUILayout.LabelField($"物理半径:({((float)shape.Radius).ToString(kStrFloatFormat)})");
    }


    public override void OnInspectorGUI() {
        DoOnInspectorGUI();
    }
}

#endif