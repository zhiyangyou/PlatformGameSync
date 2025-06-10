using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZM.ZMAsset;

namespace GameScripts {
    public class LoadSceneManager : MonoSingleton<LoadSceneManager> {
        public void LoadSceneAsync(string sceneName, Action onLoadComplete, Action<float> onLoadingProgress = null) {
            StartCoroutine(AsyncLoadScene(sceneName, onLoadComplete, onLoadingProgress));
        }

        IEnumerator AsyncLoadScene(string sceneName, Action onLoadComplete, Action<float> onLoadingProgress = null) {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            UIEventControl.DispensEvent(UIEventEnum.LoadingScene_Start);

            float curProgress = 0f;
            float maxProgress = 100f;
            while (curProgress < 90) {
                curProgress = asyncOperation.progress * 100f;
                var percent = curProgress / 100f;
                UIEventControl.DispensEvent(UIEventEnum.LoadingScene_Progress, percent);
                onLoadingProgress?.Invoke(percent);
                yield return null;
                yield return null;
                yield return null;
            }

            bool hasOnePercent = false;
            while (curProgress < maxProgress) {
                curProgress++;
                var percent = curProgress / 100f;
                UIEventControl.DispensEvent(UIEventEnum.LoadingScene_Progress, curProgress / 100f);
                onLoadingProgress?.Invoke(percent);
                hasOnePercent = percent >= 1f;
                yield return null;
            }
            asyncOperation.allowSceneActivation = true;
            yield return null; // asyncOperation.allowSceneActivation =true,先激活当前场景, 然后等待一阵,再调用complete回调方可. 不然当前当前激活的场景依旧是上一个
            UIEventControl.DispensEvent(UIEventEnum.LoadingScene_End);
            if (!hasOnePercent) {
                onLoadingProgress?.Invoke(1f);
            }
            onLoadComplete?.Invoke();
        }
    }
}