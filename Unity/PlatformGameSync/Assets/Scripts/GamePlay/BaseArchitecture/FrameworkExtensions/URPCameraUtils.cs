using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GamePlay {
    public static class URPCameraUtils {
        public const string kStrSceneBaseCameraName = "SceneBaseCamera";

        public static void SetCurSceneCameraAsBaseCamera() {
            var goCamera = GameObject.Find(kStrSceneBaseCameraName);
            if (goCamera == null) {
                Debug.LogError($"找不到场景中的Scene中的 {kStrSceneBaseCameraName}");
                return;
            }
            var sceneCam = goCamera.GetComponent<Camera>();
            var sceneCamData = sceneCam.GetComponent<UniversalAdditionalCameraData>();
            sceneCamData.renderType = CameraRenderType.Base;

            var uiCam = UIModule.Instance.Camera;
            var uiCamData = uiCam.GetComponent<UniversalAdditionalCameraData>();
            uiCamData.renderType = CameraRenderType.Overlay;
            uiCamData.cameraStack?.Clear();


            sceneCamData.cameraStack.Clear();
            sceneCamData.cameraStack.Add(uiCam);
        }

        public static void SetUICameraAsBase() {
            var uiCam = UIModule.Instance.Camera;
            var uiCamData = uiCam.GetComponent<UniversalAdditionalCameraData>();
            uiCamData.renderType = CameraRenderType.Base;
            uiCamData.cameraStack?.Clear();
        }
    }
}