#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public partial class RenderObject {
    private Mesh _gizmosBoxMesh = null;

    private Mesh GizmosBoxMesh {
        get {
            if (_gizmosBoxMesh == null) {
                _gizmosBoxMesh = new();
            }
            return _gizmosBoxMesh;
        }
    }

    private SerializedObject _serializedMono;

    private SerializedObject serializedMono {
        get {
            if (_serializedMono == null) {
                _serializedMono = new SerializedObject(this);
            }
            return _serializedMono;
        }
    }

    protected virtual void SyncRenderPosAndRotationToEntity() {
        if (baseColliderLogic == null) {
            return;
        }
        baseColliderLogic.entity.Position = entityInitPos + center;
        baseColliderLogic.entity.Orientation = entityInitRotation.ToQuaternion();
    }

    protected virtual void OnValidate() {
        SyncAllAttrsToEntity();
        if (!Application.isPlaying) {
            SyncRenderPosAndRotationToEntity();
        }
    }

    protected virtual void OnDrawGizmos() {
        if (baseColliderLogic != null) {
            switch (baseColliderType) {
                case BEPU_ColliderType.None:
                    break;
                case BEPU_ColliderType.Box:
                    BEPU_BoxColliderMono.DrawGizmos(baseColliderLogic, GizmosBoxMesh);
                    break;
                case BEPU_ColliderType.Sphere:
                    BEPU_SphereColliderMono.DrawGizmos(baseColliderLogic);
                    break;
                case BEPU_ColliderType.Capsule:
                    BEPU_CapsuleColliderMono.DrawGizmos(baseColliderLogic);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"不支持的碰撞体类型{this.LogicObject.ColliderType}");
            }
        }

        // SyncInitPosAndRot();
    }


    private void SyncInitPosAndRot() {
        if (!Application.isPlaying) {
            serializedMono.Update();
            serializedMono.FindProperty(nameof(entityInitPos)).boxedValue = transform.position.ToFixedVector3();
            serializedMono.FindProperty(nameof(entityInitRotation)).boxedValue = transform.rotation.ToFixedQuaternion().ToEulerAngles();
            serializedMono.ApplyModifiedProperties();
        }
    }
}
#endif