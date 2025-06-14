using System;
using FixMath.NET;


/// <summary>
/// 控制角色移动
/// </summary>
public class LogicTimer : ITimerBehaviour {
    #region 属性字段

    private Fix64 _delayTimeS = 0;
    private int _loopCount = 0;

    private Fix64 _accLogicFrameTimeS = 0;
    private Fix64 _totalTimeS = 0; // 总运行时间 (固定值)

    #endregion


    #region public

    public LogicTimer(Fix64 delayTimeS, Action onFinish, int loopCount) {
        // if (loopCount <= 0) {
        //     throw new ArgumentException($"loopCount 不能小等于0 {nameof(loopCount)}:{loopCount}");
        // }
        _delayTimeS = delayTimeS;
        _loopCount = loopCount;
        _onTimerFinish = onFinish;
        _totalTimeS = loopCount * _delayTimeS;
        timerFinish = false;
    }


    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate(Fix64 deltaTime) {
        _accLogicFrameTimeS += deltaTime;
        if (_accLogicFrameTimeS >= _delayTimeS) {
            _onTimerFinish?.Invoke();
            _accLogicFrameTimeS -= _delayTimeS;
            _totalTimeS -= _delayTimeS;
            if (_loopCount >= 0) {
                if (_loopCount <= 1 || _totalTimeS <= 0) {
                    timerFinish = true;
                    _onTimerFinish = null;
                }
            }
        }
    }

    public override void OnTimerFinish() { }

    public override void Complete() {
        timerFinish = true;
    }

    #endregion
}