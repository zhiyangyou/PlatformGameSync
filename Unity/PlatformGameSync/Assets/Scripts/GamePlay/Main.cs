using System;
using System.Threading.Tasks;
using UnityEngine;
using ZM.ZMAsset;

namespace GamePlay {
    public class Main : MonoBehaviour {
        private void Awake() {
            InitScreenSettings();
            DontDestroyOnLoad(gameObject);
            InitLoadingStateCallback();
            ZMAsset.InitFrameWork();
            UIModule.Instance.Initialize();
            InitAssetBundle();
        }

        private void Start() {
            // InitNetworkManager();
        }

        private void InitAssetBundle() {
            HotUpdateManager.Instance.HotAndUnPackAssets(BundleModuleEnum.Game, OnUnPackAssetComplete);
        }

        private void OnUnPackAssetComplete() {
            StartGame();
        }

        private void StartGame() {
            WorldManager.CreateWorld<GameWorld>();
        }

        private void InitScreenSettings() {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }


        private async Task InitNetworkManager() {
            await NetworkManager.Instance.Initlization();
        }

        private void InitLoadingStateCallback() {
            HotUpdateManager.Instance.OnLoadingState += what => {
                if (what == LoadWhat.Config) {
                    // ConfigCenter.Instance.InitGameCfg();
                }
            };
        }
    }
}