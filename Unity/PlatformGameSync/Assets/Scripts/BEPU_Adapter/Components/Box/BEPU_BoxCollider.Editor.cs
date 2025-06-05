#if UNITY_EDITOR


using UnityEditor;

[CustomEditor(typeof(BEPU_BoxCollider))]
[CanEditMultipleObjects]
public class BEPU_BoxCollider_Editor : BEPU_BaseColliderEditor {
    #region 属性和字段

    private SerializedProperty sizeProp;
    private BEPU_BoxCollider _boxCollider = null;

    #endregion

    void OnEnable() {
        base.DoInit();
        sizeProp = serializedObject.FindProperty("size");
        _boxCollider = target as BEPU_BoxCollider;
    }

    void OnDisable() {
        base.DoUninit();
    }

    protected override void DrawBaseAttr() {
        base.DrawBaseAttr();
        EditorGUILayout.PropertyField(sizeProp);
    }


    protected override void DrawRigidBodyAttrs() {
        base.DrawRigidBodyAttrs();
    }

    protected override void DrawDebugAttrs() {
        base.DrawDebugAttrs();

        var shape = _boxCollider.boxShape;
        EditorGUILayout.LabelField($"长宽高:({((float)shape.Width).ToString(kStrFloatFormat)} , {((float)shape.Height).ToString(kStrFloatFormat)} , {((float)shape.Length).ToString(kStrFloatFormat)})");
    }


    public override void OnInspectorGUI() {
        DoOnInspectorGUI();
    }
}

#endif