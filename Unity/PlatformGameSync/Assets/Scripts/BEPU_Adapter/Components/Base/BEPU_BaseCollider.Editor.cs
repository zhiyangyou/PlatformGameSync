#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(BEPU_BaseCollider))]
// [CanEditMultipleObjects]
public class BEPU_BaseColliderEditor<TCollider> : Editor where TCollider : BEPU_BaseCollider {
    #region 属性和字段

    public const string kStrFloatFormat = "F2";

    protected TCollider collider;

    private SerializedProperty centerProp;

    private SerializedProperty isTrigger;
    private SerializedProperty materialSo;

    private SerializedProperty entityType;

    private SerializedProperty mass;
    private SerializedProperty drag;
    private SerializedProperty angularDrag;
    private SerializedProperty useGravity;
    private SerializedProperty gravityScale;
    private SerializedProperty freezePos_X;
    private SerializedProperty freezePos_Y;
    private SerializedProperty freezePos_Z;
    private SerializedProperty freezeRotation_X;
    private SerializedProperty freezeRotation_Y;
    private SerializedProperty freezeRotation_Z;

    private Transform _curTransform = null;
    private ListenerTransformChanged _listenerTransformChanged;

    #endregion

    #region override

    protected void DoInit() {
        collider = target as TCollider;
        centerProp = serializedObject.FindProperty("center");
        isTrigger = serializedObject.FindProperty("isTrigger");
        materialSo = serializedObject.FindProperty("materialSo");
        entityType = serializedObject.FindProperty("entityType");

        mass = serializedObject.FindProperty("mass");
        drag = serializedObject.FindProperty("drag");
        angularDrag = serializedObject.FindProperty("angularDrag");
        useGravity = serializedObject.FindProperty("useGravity");
        gravityScale = serializedObject.FindProperty("gravityScale");
        freezePos_X = serializedObject.FindProperty("freezePos_X");
        freezePos_Y = serializedObject.FindProperty("freezePos_Y");
        freezePos_Z = serializedObject.FindProperty("freezePos_Z");
        freezeRotation_X = serializedObject.FindProperty("freezeRotation_X");
        freezeRotation_Y = serializedObject.FindProperty("freezeRotation_Y");
        freezeRotation_Z = serializedObject.FindProperty("freezeRotation_Z");

        _curTransform = collider.transform;
        _listenerTransformChanged = new ListenerTransformChanged(_curTransform, OnTransformChanged);
        collider.SyncAllAttrsToEntity();
    }


    protected void DoUninit() {
        _listenerTransformChanged?.Dispose();
        _listenerTransformChanged = null;
    }


    protected void DoOnInspectorGUI() {
        serializedObject.Update();
        DrawBaseAttr();
        DrawRigidBodyAttrs();
        DrawDebugAttrs();
        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawBaseAttr() {
        EditorGUILayout.PropertyField(isTrigger);
        EditorGUILayout.PropertyField(materialSo);
        EditorGUILayout.PropertyField(centerProp);
        EditorGUILayout.LabelField("FreezePos:");
        EditorGUILayout.PropertyField(freezePos_X);
        EditorGUILayout.PropertyField(freezePos_Y);
        EditorGUILayout.PropertyField(freezePos_Z);
        EditorGUILayout.LabelField("FreezeRotation:");

        EditorGUILayout.PropertyField(freezeRotation_X);
        EditorGUILayout.PropertyField(freezeRotation_Y);
        EditorGUILayout.PropertyField(freezeRotation_Z);

        EditorGUILayout.PropertyField(entityType);
    }

    protected virtual void DrawRigidBodyAttrs() {
        BEPU_EEntityType type = (BEPU_EEntityType)((int)entityType.enumValueIndex);
        if (type == BEPU_EEntityType.Dyanmic) {
            EditorGUILayout.LabelField("刚体属性");
            EditorGUILayout.PropertyField(mass);
            EditorGUILayout.PropertyField(drag);
            EditorGUILayout.PropertyField(angularDrag);
            EditorGUILayout.PropertyField(useGravity);
            if (useGravity.boolValue) {
                EditorGUILayout.PropertyField(gravityScale);
            }
        }
        else {
            EditorGUILayout.LabelField("None (没有刚体属性)");
        }
    }

    protected virtual void DrawDebugAttrs() {
        EditorGUI.BeginDisabledGroup(true);
        var entity = collider.entity;
        var mat = entity.Material;
        EditorGUILayout.LabelField($"Physics位置:({((float)collider.entity.Position.X).ToString(kStrFloatFormat)} , {((float)collider.entity.Position.Y).ToString(kStrFloatFormat)} , {((float)collider.entity.Position.Z).ToString(kStrFloatFormat)})");
        EditorGUILayout.LabelField($"GameObject位置:({_curTransform.position.x.ToString(kStrFloatFormat)} , {_curTransform.position.y.ToString(kStrFloatFormat)} , {(_curTransform.position.z).ToString(kStrFloatFormat)})");
        EditorGUILayout.LabelField($"动摩擦:{((float)mat.KineticFriction).ToString(kStrFloatFormat)} 静摩擦:{((float)mat.StaticFriction).ToString(kStrFloatFormat)} 弹性:{((float)mat.Bounciness).ToString(kStrFloatFormat)}");
        EditorGUI.EndDisabledGroup();
    }

    #endregion

    #region private

    private void OnTransformChanged() {
        collider.SyncAllAttrsToEntity();
    }

    #endregion
}
#endif