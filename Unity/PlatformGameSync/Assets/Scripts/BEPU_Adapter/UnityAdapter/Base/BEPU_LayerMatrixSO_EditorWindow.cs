using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LayerMatrixEditor : EditorWindow {
    private BEPU_LayerMatrixSO targetData; // 当前正在编辑的数据资源
    private Vector2 scrollPosition;

    // 添加一个菜单项，用于打开我们的编辑器窗口
    [MenuItem("Tools/Layer Matrix Editor")]
    public static void ShowWindow() {
        // 获取或创建一个新的编辑器窗口实例
        ShowWindow(null);
    }

    public static void ShowWindow(BEPU_LayerMatrixSO so) {
        // 获取或创建一个新的编辑器窗口实例
        var window = GetWindow<LayerMatrixEditor>("Layer Matrix");
        window.targetData = so;
    }

    // 获取所有已定义的层名称
    List<string> _layerNames = new List<string>();
    List<int> _layerNumbers = new List<int>();

    private void OnEnable() {
        for (int i = 0; i < (int)BEPU_LayerDefine.LayerCount; i++) {
            _layerNumbers.Add(i);
            _layerNames.Add(((BEPU_LayerDefine)(i)).ToString());
        }
    }

    private void OnGUI() {
        // 标题
        GUILayout.Label("Layer Interaction Matrix Editor", EditorStyles.boldLabel);

        // 允许用户拖拽或选择一个 BEPU_LayerMatrixSO 资源
        targetData = (BEPU_LayerMatrixSO)EditorGUILayout.ObjectField(
            "Matrix Data Asset",
            targetData,
            typeof(BEPU_LayerMatrixSO),
            false);


        EditorGUILayout.Space();

        // 使用滚动视图，以防层太多超出窗口

        if (targetData != null) {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawColumnHeaders(_layerNames);
            for (int i = 0; i < _layerNumbers.Count; i++) {
                DrawMatrixRow(i, _layerNames, _layerNumbers);
            }
            EditorGUILayout.EndScrollView();
        }
        else {
            EditorGUILayout.LabelField("Please select a Layer Matrix Asset.");
        }

        // 如果GUI有任何变化，标记资源为"dirty"以便保存
        if (GUI.changed) {
            EditorUtility.SetDirty(targetData);
        }

        var rectSaveBtn = EditorGUILayout.GetControlRect();
        if (GUI.Button(rectSaveBtn, "Save")) {
            AssetDatabase.SaveAssetIfDirty(targetData);
        }
    }

    private void DrawColumnHeaders(List<string> layerNames) {
        EditorGUILayout.BeginHorizontal();
        // 左上角的空白区域
        GUILayout.Label("", GUILayout.Width(100));
        float columnWidth = 24f;
        for (int i = 0; i < layerNames.Count; i++) {
            // 这是关键的修正：
            // 我们获取一个矩形，并使用 GUILayout.Width() 强制其宽度
            Rect labelRect = GUILayoutUtility.GetRect(columnWidth, 125, GUILayout.Width(columnWidth));

            // 保存当前GUI状态
            Matrix4x4 matrix = GUI.matrix;

            // 旋转
            GUIUtility.RotateAroundPivot(-80, new Vector2(labelRect.x, labelRect.y));

            // 在旋转后的坐标系中绘制标签
            // 我们稍微调整一下绘制矩形，让文字更居中
            Rect textRect = new Rect(labelRect.x - 105, labelRect.y + 20, labelRect.height, labelRect.width);
            GUI.Label(textRect, layerNames[i], EditorStyles.label);

            // 恢复GUI状态
            GUI.matrix = matrix;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawMatrixRow(int rowIndex, List<string> layerNames, List<int> layerNumbers) {
        EditorGUILayout.BeginHorizontal();

        // 绘制行标题
        GUILayout.Label(layerNames[rowIndex], GUILayout.Width(110));

        int layerA = layerNumbers[rowIndex];

        // 绘制该行的所有复选框
        for (int colIndex = 0; colIndex < layerNumbers.Count; colIndex++) {
            int layerB = layerNumbers[colIndex];
            {
                bool disableEdit = colIndex < rowIndex;
                EditorGUI.BeginDisabledGroup(disableEdit);
                bool currentValue = targetData.GetValue((BEPU_LayerDefine)layerA, (BEPU_LayerDefine)layerB);

                // 开始检查变化
                EditorGUI.BeginChangeCheck();

                bool newValue = EditorGUILayout.Toggle(currentValue, GUILayout.Width(20));

                // 如果用户点击了复选框
                if (EditorGUI.EndChangeCheck()) {
                    // 更新数据
                    targetData.SetValue((BEPU_LayerDefine)layerA, (BEPU_LayerDefine)layerB, newValue);
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        EditorGUILayout.EndHorizontal();
    }
}