#if UNITY_EDITOR


using System;
using UnityEditor;
using UnityEngine;

public class ListenerTransformChanged : IDisposable {
    private Transform _curTransform;
    private bool _hasAddEventListener = false;
    private Action _onChanged = null;
    private ListenerTransformChanged() { }

    public ListenerTransformChanged(Transform curTransform, Action onChanged) {
        _curTransform = curTransform;
        _onChanged = onChanged;
        if (!_hasAddEventListener) {
            ObjectChangeEvents.changesPublished += ObjectChangeEventsOnchangesPublished;
            _hasAddEventListener = true;
        }
    }

    private void ObjectChangeEventsOnchangesPublished(ref ObjectChangeEventStream stream) {
        if (_curTransform == null) {
            return;
        }
        for (int i = 0; i < stream.length; i++) {
            if (stream.GetEventType(i) == ObjectChangeKind.ChangeGameObjectOrComponentProperties) {
                stream.GetChangeGameObjectOrComponentPropertiesEvent(i, out var changeEvent);
                Transform targetTransform = EditorUtility.InstanceIDToObject(changeEvent.instanceId) as Transform;
                if (_curTransform == targetTransform) {
                    _onChanged?.Invoke();
                    break;
                }
            }
        }
    }

    public void Dispose() {
        if (_hasAddEventListener) {
            ObjectChangeEvents.changesPublished -= ObjectChangeEventsOnchangesPublished;
            _hasAddEventListener = false;
        }
    }
}

#endif