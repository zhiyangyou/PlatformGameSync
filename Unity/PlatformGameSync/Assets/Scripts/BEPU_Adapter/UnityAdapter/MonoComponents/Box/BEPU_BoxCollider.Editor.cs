#if UNITY_EDITOR


using UnityEditor;

[CustomEditor(typeof(BEPU_BoxCollider))]
[CanEditMultipleObjects]
public class BEPU_BoxCollider_Editor : BEPU_BaseColliderEditor<BEPU_BoxCollider> {
    #region 属性和字段

    private SerializedProperty sizeProp;

    #endregion

    void OnEnable() {
        base.DoInit();
        sizeProp = serializedObject.FindProperty("size");
    }

    void OnDisable() {
        base.DoUninit();
    }

    protected override void DrawBaseAttr() {
        base.DrawBaseAttr();
        EditorGUILayout.PropertyField(sizeProp);
    }


    protected override void DrawDebugAttrs() {
        base.DrawDebugAttrs();

        var shape = collider.boxShape;
        EditorGUILayout.LabelField($"长宽高:({((float)shape.Width).ToString(kStrFloatFormat)} , {((float)shape.Height).ToString(kStrFloatFormat)} , {((float)shape.Length).ToString(kStrFloatFormat)})");
    }


    public override void OnInspectorGUI() {
        DoOnInspectorGUI();
    }
}

#endif