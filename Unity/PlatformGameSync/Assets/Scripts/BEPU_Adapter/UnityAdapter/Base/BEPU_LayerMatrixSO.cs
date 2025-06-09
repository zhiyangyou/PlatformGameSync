using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BEPU_LayerMatrix", menuName = "BEPU/BEPU_LayerMatrix")]
public class BEPU_LayerMatrixSO : ScriptableObject {
    [SerializeField] public bool[] matrix = new bool[(int)BEPU_LayerDefaine.LayerCount * (int)BEPU_LayerDefaine.LayerCount];
    public int Length => matrix.Length;

    private static int LayerCount => (int)BEPU_LayerDefaine.LayerCount;

    private bool GetValue(bool[] arr, BEPU_LayerDefaine ElayerA, BEPU_LayerDefaine ElayerB) {
        var layerA = (int)ElayerA;
        var layerB = (int)ElayerB;
        // 边界检查
        if (layerA < 0
            || layerA >= LayerCount
            || layerB < 0
            || layerB >= LayerCount) {
            Debug.LogError($"Layer index out of bounds. {layerA} {layerB}");
            return false;
        }
        return arr[layerA * LayerCount + layerB];
    }

    /// <summary>
    /// 获取两个层之间是否可以交互
    /// </summary>
    /// <param name="layerA">层 A 的索引</param>
    /// <param name="layerB">层 B 的索引</param>
    /// <returns>如果可以交互，返回 true</returns>
    public bool GetValue(BEPU_LayerDefaine ElayerA, BEPU_LayerDefaine ElayerB) {
        return GetValue(matrix, ElayerA, ElayerB);
    }

    public void SetValue(bool[] arr, BEPU_LayerDefaine ElayerA, BEPU_LayerDefaine ElayerB, bool value) {
        var layerA = (int)ElayerA;
        var layerB = (int)ElayerB;
        if (layerA < 0 || layerA >= LayerCount || layerB < 0 || layerB >= LayerCount) {
            Debug.LogError("Layer index out of bounds.");
            return;
        }
        // 交互是相互的，所以 (A, B) 和 (B, A) 的状态应该一致
        arr[layerA * LayerCount + layerB] = value;
        arr[layerB * LayerCount + layerA] = value;
    }


    /// <summary>
    /// 设置两个层之间的交互状态 (具有对称性)
    /// </summary>
    /// <param name="layerA">层 A 的索引</param>
    /// <param name="layerB">层 B 的索引</param>
    /// <param name="value">是否可以交互</param>
    public void SetValue(BEPU_LayerDefaine ElayerA, BEPU_LayerDefaine ElayerB, bool value) {
        SetValue(matrix, ElayerA, ElayerB, value);
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
            for (int layerA = 0; layerA < oldLayerCount; layerA++) {
                for (int layerB = 0; layerB < oldLayerCount; layerB++) {
                    var oldValue = GetValue(oldData, (BEPU_LayerDefaine)layerA, (BEPU_LayerDefaine)layerB);
                    SetValue(this.matrix, (BEPU_LayerDefaine)layerA, (BEPU_LayerDefaine)layerB, oldValue);
                }
            }
        }
        else {
            Debug.LogError("数据长度匹配， 不需要修正");
        }
    }
}