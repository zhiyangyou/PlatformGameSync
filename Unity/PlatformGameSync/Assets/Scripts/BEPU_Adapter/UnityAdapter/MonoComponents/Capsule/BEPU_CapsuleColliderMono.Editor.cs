#if UNITY_EDITOR

using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEditor;
using UnityEngine;

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

    protected override void DoAutoSize() {
        
        var scale = curTransform.lossyScale;
      
        var scaleRadiu = (Fix64)Mathf.Max(scale.x, scale.z);
        var length = (Fix64)scale.y;
        radiuProp.boxedValue = scaleRadiu * Fix64.HalfOne;
        lengthProp.boxedValue = length;
        collider.SyncExtendAttrsToEntity();
    }


    public override void OnInspectorGUI() {
        DoOnInspectorGUI();
    }
}

#endif