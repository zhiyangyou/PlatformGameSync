using UnityEngine;


public class BEPU_PhysicsManagerUnity : BEPU_PhysicsManagerLogic<BEPU_PhysicsManagerUnity> {
    public override void ExtentInit() {
        var matrixSoConfigPath = "BEPU_Settings/BEPU_LayerMatrix";
        var so = Resources.Load<BEPU_LayerMatrixSO>(matrixSoConfigPath);
        if (so != null) {
            Debug.Log("配置LayerMatrix");
            this.SetLayerMatrix(so.AsLayerMatrix());
        }
        else {
            Debug.LogError($"加载不到资源文件: {matrixSoConfigPath}");
        }
    }
}