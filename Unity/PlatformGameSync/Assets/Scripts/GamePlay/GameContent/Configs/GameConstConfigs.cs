using FixMath.NET;

namespace GamePlay {
    public static class GameConstConfigs {
        public static Fix64 FrameIntervalS => BEPU_PhysicsUpdater.PhysicsTimeStep;  

        /// <summary>
        /// 最大预测的帧数, 不应该是一个固定数值, 而是一个根据目标帧数的动态数值
        /// </summary>
        public static readonly long MaxPreMoveCount = 5;

        public static readonly bool UseLocalFrame = true;
    }
}