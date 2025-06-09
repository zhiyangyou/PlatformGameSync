using System;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BEPU_LayerMatrixSO))]
public class BEPU_LayerMatrixSO_Inspector : Editor {
    private BEPU_LayerMatrixSO _so;

    private void OnEnable() {
        _so = target as BEPU_LayerMatrixSO;
    }

    private void OnDisable() { }


    public override void OnInspectorGUI() {
        var rectBtn = EditorGUILayout.GetControlRect();
        if (GUI.Button(rectBtn, "编辑")) {
            LayerMatrixEditor.ShowWindow(_so);
        }

        var layerCount = (int)BEPU_LayerDefine.LayerCount;
        var needLen = layerCount * layerCount;
        if (needLen != _so.Length) {
            Debug.LogError($"枚举发生变更，修正配置数据！newLayerCount{layerCount} oldLayerCount:{(int)Mathf.Sqrt(_so.Length)}");
            _so.FixData(layerCount);
        }

        EditorGUI.BeginDisabledGroup(true);

        for (int layerA = 0; layerA < layerCount; layerA++) {
            for (int layerB = 0; layerB < layerCount; layerB++) {
                var value = _so.GetValue((BEPU_LayerDefine)layerA, (BEPU_LayerDefine)layerB);
                var label = $"{((BEPU_LayerDefine)layerA).ToString()} - {((BEPU_LayerDefine)layerB).ToString()}";
                EditorGUILayout.Toggle(label, value);
            }
        }
        // base.OnInspectorGUI();
        EditorGUI.EndDisabledGroup();
    }
}