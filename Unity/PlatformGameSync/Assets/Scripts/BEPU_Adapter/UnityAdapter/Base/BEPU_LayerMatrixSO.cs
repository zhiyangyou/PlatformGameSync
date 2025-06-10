using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BEPU_LayerMatrix", menuName = "BEPU/BEPU_LayerMatrix")]
public class BEPU_LayerMatrixSO : ScriptableObject {
    [SerializeField] public bool[] matrix = new bool[(int)BEPU_LayerDefine.LayerCount * (int)BEPU_LayerDefine.LayerCount];
    public int Length => matrix.Length;

    private static int LayerCount => (int)BEPU_LayerDefine.LayerCount;

    public BEPU_LayerMatrix AsLayerMatrix() {
        var ret = new BEPU_LayerMatrix();
        for (int layerA = 0; layerA < LayerCount; layerA++) {
            for (int layerB = 0; layerB < LayerCount; layerB++) {
                ret.Set((BEPU_LayerDefine)layerA, (BEPU_LayerDefine)layerB, GetValue((BEPU_LayerDefine)layerA, (BEPU_LayerDefine)layerB));
            }
        }
        return ret;
    }

    private bool GetValue(bool[] arr, int layerCount, BEPU_LayerDefine ElayerA, BEPU_LayerDefine ElayerB) {
        var layerA = (int)ElayerA;
        var layerB = (int)ElayerB;
        // 边界检查
        if (layerA < 0
            || layerA >= layerCount
            || layerB < 0
            || layerB >= layerCount) {
            Debug.LogError($"Layer index out of bounds. {layerA} {layerB}");
            return false;
        }
        var index = layerA * layerCount + layerB;
        if (index < 0 || index >= arr.Length) {
            Debug.LogError($"layerA:{layerA} layerB:{layerB} LayerCount:{layerCount}");
            return false;
        }
        return arr[layerA * layerCount + layerB];
    }

    /// <summary>
    /// 获取两个层之间是否可以交互
    /// </summary>
    /// <param name="layerA">层 A 的索引</param>
    /// <param name="layerB">层 B 的索引</param>
    /// <returns>如果可以交互，返回 true</returns>
    public bool GetValue(BEPU_LayerDefine ElayerA, BEPU_LayerDefine ElayerB) {
        return GetValue(matrix, LayerCount, ElayerA, ElayerB);
    }

    public void SetValue(bool[] arr, int layerCount, BEPU_LayerDefine ElayerA, BEPU_LayerDefine ElayerB, bool value) {
        var layerA = (int)ElayerA;
        var layerB = (int)ElayerB;
        if (layerA < 0 || layerA >= layerCount || layerB < 0 || layerB >= layerCount) {
            Debug.LogError("Layer index out of bounds.");
            return;
        }
        // 交互是相互的，所以 (A, B) 和 (B, A) 的状态应该一致
        arr[layerA * layerCount + layerB] = value;
        arr[layerB * layerCount + layerA] = value;
    }


    /// <summary>
    /// 设置两个层之间的交互状态 (具有对称性)
    /// </summary>
    /// <param name="layerA">层 A 的索引</param>
    /// <param name="layerB">层 B 的索引</param>
    /// <param name="value">是否可以交互</param>
    public void SetValue(BEPU_LayerDefine ElayerA, BEPU_LayerDefine ElayerB, bool value) {
        SetValue(matrix, LayerCount, ElayerA, ElayerB, value);
    }

    public void FixData(int newLayerCount) {
        var newLen = newLayerCount * newLayerCount;
        if (newLen != this.matrix.Length) {
            var oldData = this.matrix;
            var newData = new bool[newLen];
            for (int i = 0; i < newData.Length; i++) {
                newData[i] = true;
            }
            this.matrix = newData;

            var oldLayerCount = (int)Mathf.Sqrt(oldData.Length);

            var forCount = Math.Min(oldLayerCount, newLayerCount);
            for (int layerA = 0; layerA < forCount; layerA++) {
                for (int layerB = 0; layerB < forCount; layerB++) {
                    var oldValue = GetValue(oldData, oldLayerCount, (BEPU_LayerDefine)layerA, (BEPU_LayerDefine)layerB);
                    SetValue(this.matrix, newLayerCount, (BEPU_LayerDefine)layerA, (BEPU_LayerDefine)layerB, oldValue);
                }
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        else {
            Debug.LogError("数据长度匹配， 不需要修正");
        }
    }
}