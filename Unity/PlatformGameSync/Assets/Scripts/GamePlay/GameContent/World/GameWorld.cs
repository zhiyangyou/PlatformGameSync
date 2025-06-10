using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GameScripts;
using UnityEngine;

public class GameWorld : World {
    #region life-cycle

    public override void OnCreate() {
        UIModule.Instance.PopUpWindow<LoadingGameWindow>();
        LoadSceneManager.Instance.LoadSceneAsync(SceneNames.GamePlayScene, OnLoadSceneComplete);
    }

    private void OnLoadSceneComplete() {
        UIModule.Instance.HideWindow<LoadingGameWindow>();
        UIModule.Instance.PopUpWindow<BattleHUDWindow>();
        UIModule.Instance.PopUpWindow<BattleDebugStateWindow>();
    }

    public override void OnUpdate() {
        base.OnUpdate();
    }

    public override void OnDestroy() {
        base.OnDestroy();
    }

    public override void OnDestroyPostProcess(object args) {
        base.OnDestroyPostProcess(args);
    }

    #endregion

    #region override

    private static Type[] s_OrderCtrlTypes = new Type[] { };

    private static Type[] s_OrderDataTypes = new Type[]
        { };

    private static Type[] s_OrderMsgTypes = new Type[]
        { };


    public override Type[] GetLogicBehaviourExecution() {
        return s_OrderCtrlTypes;
    }

    public override Type[] GetDataBehaviourExecution() {
        return s_OrderDataTypes;
    }

    public override Type[] GetMsgBehaviourExecution() {
        return s_OrderMsgTypes;
    }

    public override WorldEnum WorldEnum => WorldEnum.GameWorld;

    #endregion
}