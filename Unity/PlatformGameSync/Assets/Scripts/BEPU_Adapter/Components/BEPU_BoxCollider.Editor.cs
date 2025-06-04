#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BEPU_BoxCollider))]
[CanEditMultipleObjects]
public class BEPU_BoxCollider_Editor : Editor {
    #region 属性和字段

    private bool _editMode = false;
    const string kStrFloatFormat = "F2";

    #endregion

    #region life-cycle

    private const float HandleSize = 0.05f;
    private BEPU_BoxCollider _collider;

    private SerializedProperty centerProp;
    private SerializedProperty sizeProp;
    private SerializedProperty isTrigger;
    private SerializedProperty materialSo;

    private SerializedProperty entityType;

    private SerializedProperty mass;
    private SerializedProperty drag;
    private SerializedProperty angularDrag;
    private SerializedProperty useGravity;
    private SerializedProperty freezePos_X;
    private SerializedProperty freezePos_Y;
    private SerializedProperty freezePos_Z;
    private SerializedProperty freezeRotation_X;
    private SerializedProperty freezeRotation_Y;
    private SerializedProperty freezeRotation_Z;

    private Transform _curTransform = null;
    private ListenerTransformChanged _listenerTransformChanged;

    private void OnEnable() {
        _collider = target as BEPU_BoxCollider;
        centerProp = serializedObject.FindProperty("center");
        sizeProp = serializedObject.FindProperty("size");
        isTrigger = serializedObject.FindProperty("isTrigger");
        materialSo = serializedObject.FindProperty("materialSo");
        entityType = serializedObject.FindProperty("entityType");

        mass = serializedObject.FindProperty("mass");
        drag = serializedObject.FindProperty("drag");
        angularDrag = serializedObject.FindProperty("angularDrag");
        useGravity = serializedObject.FindProperty("useGravity");
        freezePos_X = serializedObject.FindProperty("freezePos_X");
        freezePos_Y = serializedObject.FindProperty("freezePos_Y");
        freezePos_Z = serializedObject.FindProperty("freezePos_Z");
        freezeRotation_X = serializedObject.FindProperty("freezeRotation_X");
        freezeRotation_Y = serializedObject.FindProperty("freezeRotation_Y");
        freezeRotation_Z = serializedObject.FindProperty("freezeRotation_Z");

        _curTransform = _collider.transform;
        _listenerTransformChanged = new ListenerTransformChanged(_curTransform, OnTransformChanged);
    }

    private void OnTransformChanged() {
        _collider.SyncAttrsToEntity();
    }

    private void OnDisable() {
        _listenerTransformChanged.Dispose();
        _listenerTransformChanged = null;
    }


    public override void OnInspectorGUI() {
        serializedObject.Update();

        // 基础属性
        EditorGUILayout.PropertyField(isTrigger);
        EditorGUILayout.PropertyField(materialSo);
        EditorGUILayout.PropertyField(centerProp);
        EditorGUILayout.PropertyField(sizeProp);
        EditorGUILayout.PropertyField(entityType);


        DrawRigidBodyAttrs();
        // debug属性
        DrawDebugAttrs();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawRigidBodyAttrs() {
        BEPU_EEntityType type = (BEPU_EEntityType)((int)entityType.enumValueIndex);
        if (type == BEPU_EEntityType.Dyanmic) {
            EditorGUILayout.LabelField("刚体属性");

            EditorGUILayout.PropertyField(mass);
            EditorGUILayout.PropertyField(drag);
            EditorGUILayout.PropertyField(angularDrag);
            EditorGUILayout.PropertyField(useGravity);

            EditorGUILayout.LabelField("FreezePos:");
            EditorGUILayout.PropertyField(freezePos_X);
            EditorGUILayout.PropertyField(freezePos_Y);
            EditorGUILayout.PropertyField(freezePos_Z);
            EditorGUILayout.LabelField("FreezeRotation:");
            
            EditorGUILayout.PropertyField(freezeRotation_X);
            EditorGUILayout.PropertyField(freezeRotation_Y);
            EditorGUILayout.PropertyField(freezeRotation_Z);
        }
        else {
            EditorGUILayout.LabelField("None (没有刚体属性)");
        }
    }

    private void DrawDebugAttrs() {
        EditorGUI.BeginDisabledGroup(true);

        var entity = _collider.entity;
        var shape = _collider.boxShape;
        var mat = entity.Material;
        EditorGUILayout.LabelField($"长宽高:({((float)shape.Width).ToString(kStrFloatFormat)} , {((float)shape.Height).ToString(kStrFloatFormat)} , {((float)shape.Length).ToString(kStrFloatFormat)})");
        EditorGUILayout.LabelField($"Physics位置:({((float)_collider.entity.Position.X).ToString(kStrFloatFormat)} , {((float)_collider.entity.Position.Y).ToString(kStrFloatFormat)} , {((float)_collider.entity.Position.Z).ToString(kStrFloatFormat)})");
        EditorGUILayout.LabelField($"GameObject位置:({_curTransform.position.x.ToString(kStrFloatFormat)} , {_curTransform.position.y.ToString(kStrFloatFormat)} , {(_curTransform.position.z).ToString(kStrFloatFormat)})");
        EditorGUILayout.LabelField($"动摩擦:{((float)mat.KineticFriction).ToString(kStrFloatFormat)} 静摩擦:{((float)mat.StaticFriction).ToString(kStrFloatFormat)} 弹性:{((float)mat.Bounciness).ToString(kStrFloatFormat)}");
        EditorGUI.EndDisabledGroup();
    }

    #endregion
}
#endif