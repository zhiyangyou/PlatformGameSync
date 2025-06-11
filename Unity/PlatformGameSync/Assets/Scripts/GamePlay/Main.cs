using System;
using System.Threading.Tasks;
using GameScripts;
using UnityEngine;
using WorldSpace.GameWorld;
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
            LoadSceneManager.Instance.LoadSceneAsync(SceneNames.GamePlayScene, OnLoadSceneComplete);
        }

        private void OnLoadSceneComplete() {
            WorldManager.CreateWorld<GameWorld>();
            UIModule.Instance.HideWindow<LoadingGameWindow>();
            UIModule.Instance.PopUpWindow<BattleHUDWindow>();
            UIModule.Instance.PopUpWindow<BattleDebugStateWindow>();
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