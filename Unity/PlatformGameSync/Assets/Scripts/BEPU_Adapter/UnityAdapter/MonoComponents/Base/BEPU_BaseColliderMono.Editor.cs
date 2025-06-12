#if UNITY_EDITOR


using UnityEditor;
using UnityEngine;

public partial class BEPU_BaseColliderMono {
    private SerializedObject _serializedMono;

    private SerializedObject serializedMono {
        get {
            if (_serializedMono == null) {
                _serializedMono = new SerializedObject(this);
            }
            return _serializedMono;
        }
    }

    private void SyncInitPosAndRot() {
        if (!Application.isPlaying) {
            serializedMono.Update();
            serializedMono.FindProperty(nameof(entityInitPos)).boxedValue = transform.position.ToFixedVector3();
            serializedMono.FindProperty(nameof(entityInitRotation)).boxedValue = transform.rotation.ToFixedQuaternion().ToEulerAngles();
            // entityInitPos = transform.position.ToFixedVector3();
            // entityInitRotation = transform.rotation.ToFixedQuaternion().ToEulerAngles();
            serializedMono.ApplyModifiedProperties();
        }
    }

    private void OnGUI() {
        SyncInitPosAndRot();
    }
}

#endif