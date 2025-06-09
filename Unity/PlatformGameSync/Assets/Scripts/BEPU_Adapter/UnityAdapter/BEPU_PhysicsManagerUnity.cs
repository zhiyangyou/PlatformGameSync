using System;
using UnityEngine;


public class BEPU_PhysicsManagerUnity : BEPU_PhysicsManagerLogic<BEPU_PhysicsManagerUnity> {
    public override void ExtentInit() {
        InitLayerMatrix();
        InitLogger();
    }

    private void InitLogger() {
        BEPU_Logger.OnLog = OnLog;
        BEPU_Logger.OnLogError = OnLogError;
        BEPU_Logger.OnLogException = OnLogException;
    }

    private void OnLogException(Exception obj) {
        Debug.LogException(obj);
    }

    private void OnLogError(object obj) {
        Debug.LogError(obj);
    }

    private void OnLog(object obj) {
        Debug.Log(obj);
    }

    private void InitLayerMatrix() {
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