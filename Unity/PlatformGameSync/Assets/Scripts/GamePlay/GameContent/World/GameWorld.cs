using System;
using FixMath.NET;
using GamePlay;
using GameScripts;
using UnityEngine;

namespace WorldSpace.GameWorld {
    public class GameWorld : World {
        #region 属性和字段

        private Fix64 _accLogicRealTimeS;

        private Fix64 _nextLogicFrameTimeS;

        private Fix64 _lastUpdateTime;

        private Fix64 LogicFrameIntervalS => GameConstConfigs.FrameIntervalS;

        public InputSystem2 inputSystem2 { get; private set; }
        public NextFrameTimer nextFrameTimer { get; private set; }

        public LogicTimerManager LogicTimerManager { get; private set; }


        public static int LogicFrameCount = 0;

        #endregion

        #region life-cycle

        public override void OnCreate() {
            LogicTimerManager = new();
            inputSystem2 = new();
            nextFrameTimer = new(() => LogicFrameCount);
            LogicFrameCount = 0;
            _accLogicRealTimeS = Fix64.Zero;
            _nextLogicFrameTimeS = Fix64.Zero;
            _lastUpdateTime = Fix64.Zero;
            UIModule.Instance.PopUpWindow<LoadingGameWindow>();
        }


        public override void OnUpdate() {
            inputSystem2.InputUpdate();
            _accLogicRealTimeS += (Fix64)Time.deltaTime;

            // 当前逻辑帧时间大于下一个逻辑帧时间, 需要更新逻辑帧
            // 另外作用: 追帧 && 保证所有设备的逻辑帧的帧数的一致性
            while (_accLogicRealTimeS > _nextLogicFrameTimeS) {
                OnLigicFrameUpdate(LogicFrameIntervalS);
                LogicFrameCount++;
                _nextLogicFrameTimeS += LogicFrameIntervalS;
                // _logicDeltaTimeS = _accLogicRealTimeS - _lastUpdateTime;
                _lastUpdateTime = _accLogicRealTimeS;
                // Debug.LogError($"{LogicFrameCount} : {(Time.realtimeSinceStartup * 1000):F0} {(_logicDeltaTimeS * 1000):F0}");
            }
        }

        public override void OnDestroy() {
            LogicTimerManager.Dispose();
            LogicTimerManager = null;
            inputSystem2.ClearLisntner();
            inputSystem2 = null;
            nextFrameTimer.Dispose();
            nextFrameTimer = null;
            base.OnDestroy();
        }

        public override void OnDestroyPostProcess(object args) {
            base.OnDestroyPostProcess(args);
        }

        public void OnLigicFrameUpdate(Fix64 deltaTime) {
            // 更新物理
            nextFrameTimer.LogicFrameUpdate();
            LogicTimerManager.OnLogicFrameUpdate(deltaTime);
            BEPU_PhysicsManagerUnity.Instance.UpdatePhysicsWorld(GameConstConfigs.FrameIntervalS);
            foreach (var logic in mLogicBehaviourDic.Values) {
                try {
                    logic.OnLogicFrameUpdate(deltaTime);
                }
                catch (Exception e) {
                    Debug.LogError($"{logic.GetType().Name} 执行OnLogicFrameUpdate时报错 ");
                    Debug.LogException(e);
                }
            }
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
}