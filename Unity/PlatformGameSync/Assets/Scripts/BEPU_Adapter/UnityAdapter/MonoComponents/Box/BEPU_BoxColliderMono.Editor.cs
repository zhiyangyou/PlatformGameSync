#if UNITY_EDITOR


using BEPUphysics.CollisionShapes.ConvexShapes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BEPU_BoxColliderMono))]
[CanEditMultipleObjects]
public class BEPU_BoxColliderEditor : BEPU_BaseColliderEditor<BEPU_BoxColliderMono> {
    #region 属性和字段

    private SerializedProperty sizeProp;

    private BEPU_CustomEntity _entity;
    private BoxShape _boxShape;

    #endregion

    void OnEnable() {
        base.DoInit();
        sizeProp = serializedObject.FindProperty("size");
        _entity = ((BEPU_BoxColliderMono)target).entity;
        _boxShape = ((BEPU_BoxColliderMono)target).colliderLogic.entityShape as BoxShape;
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
        EditorGUILayout.LabelField($"长宽高:({((float)_boxShape.Width).ToString(kStrFloatFormat)} , {((float)_boxShape.Height).ToString(kStrFloatFormat)} , {((float)_boxShape.Length).ToString(kStrFloatFormat)})");
    }

    protected override void DoAutoSize() {
        var scale = curTransform.lossyScale;
        sizeProp.boxedValue = scale.ToFixedVector3();
        collider.SyncExtendAttrsToEntity();
    }


    public override void OnInspectorGUI() {
        DoOnInspectorGUI();
    }
}

#endif