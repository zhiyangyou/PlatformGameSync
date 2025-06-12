using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using WorldSpace.GameWorld;

public class NextFrameTimer : IDisposable {
    private Func<int> _frameCountGetter;

    class CallInfo {
        public Action ac = null;
        public int CallFrameCount = 0;
        private CallInfo() { }

        public CallInfo(Action ac, int callFrameCount) {
            this.ac = ac;
            CallFrameCount = callFrameCount;
        }
    }

    public NextFrameTimer(Func<int> frameCountGetter) {
        _frameCountGetter = frameCountGetter;
    }

    public void LogicFrameUpdate() {
        var curFrameCount = _frameCountGetter();
        List<int> removeKeys = BEPU_Adapter.Pool.ListPool<int>.Get();
        foreach (var kv in _dicNextFrameCall) { }
        foreach (var kv in _dicNextFrameCall) {
            var info = kv.Value;
            if (info.CallFrameCount == curFrameCount) {
                try {
                    info.ac.Invoke();
                }
                catch (Exception e) {
                    Debug.LogError("NextFrameTimer call 失败 发生异常");
                    Debug.LogException(e);
                }

                removeKeys.Add(kv.Key);
            }
        }
        for (int i = 0; i < removeKeys.Count; i++) {
            var key = removeKeys[i];
            _dicNextFrameCall.Remove(key);
        }

        BEPU_Adapter.Pool.ListPool<int>.Release(removeKeys);
    }

    private Dictionary<int, CallInfo> _dicNextFrameCall = new();

    /// <summary>
    /// 如果uniqueID重复, 则更新
    /// </summary>
    /// <param name="uniqueID"></param>
    /// <param name="ac"></param>
    public void CallOnNextFrame(int uniqueID, Action ac) {
        var curFrameCount = _frameCountGetter();
        var nextFrameCount = curFrameCount + 1;
        var newCallInfo = new CallInfo(ac, nextFrameCount);
        if (_dicNextFrameCall.ContainsKey(uniqueID)) {
            _dicNextFrameCall[uniqueID] = newCallInfo;
        }
        else {
            _dicNextFrameCall.Add(uniqueID, newCallInfo);
        }
    }


    public void ClearKey(int key) {
        _dicNextFrameCall.Remove(key);
    }


    public void Dispose() {
        _dicNextFrameCall.Clear();
        _frameCountGetter = null;
    }
}