using System;
using FixMath.NET;

public abstract class ITimerBehaviour {
    #region 属性和字段

    public bool timerFinish { get; set; }

    /// <summary>
    /// 行动完成回调
    /// </summary>
    protected Action _onTimerFinish = null;

    /// <summary>
    /// 行动更新回调
    /// </summary>
    protected Action _onTimerUpdate = null;

    #endregion

    #region public

    public abstract void OnLogicFrameUpdate(Fix64 deltaTime);

    /// <summary>
    /// 行动完成
    /// </summary>
    public abstract void OnTimerFinish();

    /// <summary>
    /// 主动完成计时器
    /// </summary>
    public abstract void Complete();

    #endregion
}