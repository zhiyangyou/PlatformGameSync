#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(BEPU_CapsuleCollider))]
[CanEditMultipleObjects]
public class BEPU_CapsuleColliderEditor : BEPU_BaseColliderEditor<BEPU_CapsuleCollider> {
    #region 属性和字段

    private SerializedProperty radiuProp;
    private SerializedProperty lengthProp;

    #endregion

    void OnEnable() {
        base.DoInit();
        radiuProp = serializedObject.FindProperty("radiu");
        lengthProp = serializedObject.FindProperty("length");
    }

    void OnDisable() {
        base.DoUninit();
    }

    protected override void DrawBaseAttr() {
        base.DrawBaseAttr();
        EditorGUILayout.PropertyField(radiuProp);
        EditorGUILayout.PropertyField(lengthProp);
    }


    protected override void DrawDebugAttrs() {
        base.DrawDebugAttrs();

        var shape = collider.capsuleShape;
        EditorGUILayout.LabelField($"物理半径:({((float)shape.Radius).ToString(kStrFloatFormat)})");
        EditorGUILayout.LabelField($"物理高度:({((float)shape.Length).ToString(kStrFloatFormat)})");
    }


    public override void OnInspectorGUI() {
        DoOnInspectorGUI();
    }
}

#endif