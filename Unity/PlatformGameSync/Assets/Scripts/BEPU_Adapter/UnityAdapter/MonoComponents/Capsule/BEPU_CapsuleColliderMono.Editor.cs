#if UNITY_EDITOR

using BEPUphysics.CollisionShapes.ConvexShapes;
using UnityEditor;

[CustomEditor(typeof(BEPU_CapsuleColliderMono))]
[CanEditMultipleObjects]
public class BEPU_CapsuleColliderEditor : BEPU_BaseColliderEditor<BEPU_CapsuleColliderMono> {
    #region 属性和字段

    private SerializedProperty radiuProp;
    private SerializedProperty lengthProp;

    private CapsuleShape _capsuleShape;

    #endregion

    void OnEnable() {
        base.DoInit();
        radiuProp = serializedObject.FindProperty("radiu");
        lengthProp = serializedObject.FindProperty("length");
        _capsuleShape = ((BEPU_CapsuleColliderMono)target).colliderLogic.entityShape as CapsuleShape;
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


        EditorGUILayout.LabelField($"物理半径:({((float)_capsuleShape.Radius).ToString(kStrFloatFormat)})");
        EditorGUILayout.LabelField($"物理高度:({((float)_capsuleShape.Length).ToString(kStrFloatFormat)})");
    }


    public override void OnInspectorGUI() {
        DoOnInspectorGUI();
    }
}

#endif