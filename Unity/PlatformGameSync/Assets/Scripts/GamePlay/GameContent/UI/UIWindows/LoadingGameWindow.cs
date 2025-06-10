/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/6/10 16:01:43
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using UnityEngine.UI;
using UnityEngine;
using ZMUIFrameWork;

public class LoadingGameWindow : WindowBase {
    public LoadingGameWindowUIComponent uiCompt = new LoadingGameWindowUIComponent();

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        uiCompt.InitComponent(this);
        base.OnAwake();
    }

    //物体显示时执行
    public override void OnShow() {
        UIEventControl.AddEvent(UIEventEnum.LoadingScene_Start, OnEvent_LoadingStart);
        UIEventControl.AddEvent(UIEventEnum.LoadingScene_Progress, OnEvent_LoadingProgress);
        UIEventControl.AddEvent(UIEventEnum.LoadingScene_End, OnEvent_Complete);
        base.OnShow();
    }

    private void OnEvent_LoadingStart(object data) {
        uiCompt.txtLoadingProgressText.text = $"开始加载场景";
    }

    private void OnEvent_Complete(object data) {
        uiCompt.txtLoadingProgressText.text = $"场景结束";
        // this.HideWindow();
    }

    private void OnEvent_LoadingProgress(object data) {
        float progress = (float)data;
        uiCompt.txtLoadingProgressText.text = $"场景加载中 {progress * 100}% ...";
    }

    //物体隐藏时执行
    public override void OnHide() {
        UIEventControl.RemoveEvent(UIEventEnum.LoadingScene_Start, OnEvent_LoadingStart);
        UIEventControl.RemoveEvent(UIEventEnum.LoadingScene_Progress, OnEvent_LoadingProgress);
        UIEventControl.RemoveEvent(UIEventEnum.LoadingScene_End, OnEvent_Complete);
        base.OnHide();
    }

    //物体销毁时执行
    public override void OnDestroy() {
        base.OnDestroy();
    }

    #endregion

    #region API Function

    #endregion
}