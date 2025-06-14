using System;
using System.Collections.Generic;
using FixMath.NET;


public class LogicTimerManager : IDisposable {
    #region 属性和字段

    private List<ITimerBehaviour> _listTimers = new();

    #endregion

    #region public

    public LogicTimer DelayCallOnce(Fix64 delayTimeS, Action onTimerFinish) {
        return DelayCall(delayTimeS, onTimerFinish, 1);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="delayTimeS"></param>
    /// <param name="onTimerFinish"></param>
    /// <param name="loopCount">负数代表永久</param>
    /// <returns></returns>
    public LogicTimer DelayCall(Fix64 delayTimeS, Action onTimerFinish, int loopCount) {
        LogicTimer timer = new LogicTimer(delayTimeS, onTimerFinish, loopCount);
        _listTimers.Add(timer);
        return timer;
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate(Fix64 deltaTime) {
        // 移除已经完成的
        for (int i = _listTimers.Count - 1; i >= 0; i--) {
            var timer = _listTimers[i];
            if (timer.timerFinish) {
                RemoveTimer(timer);
            }
        }

        // 更新逻辑帧
        foreach (var timerBehaviour in _listTimers) {
            timerBehaviour.OnLogicFrameUpdate(deltaTime);
        }
    }

    public void RemoveTimer(ITimerBehaviour actionBehaviour) {
        _listTimers.Remove(actionBehaviour);
    }

    #endregion

    public void Dispose() {
        _listTimers.Clear();
    }
}