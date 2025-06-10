using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fantasy.Helper;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Fantasy.IdFactory
{
    /// <summary>
    /// 表示一个运行时的ID。
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WorldRuntimeIdStruct
    {
        // RuntimeId:23 + 8 + 8 + 25 =  64
        // +-------------------+--------------------------+-----------------------+--------------------------------------+
        // |  time(23) 最大60天 | SceneId(8) 最多255个Scene | WordId(8) 最多255个世界 | sequence(25) 每秒每个进程能生产33554431个
        // +-------------------+--------------------------+-----------------------+--------------------------------------+
        public uint Time { get; private set; }
        public uint SceneId { get; private set; }
        public byte WordId { get; private set; }
        public uint Sequence { get; private set; }

        public const uint MaskSequence = 0x1FFFFFF;
        public const uint MaskSceneId = 0xFF;
        public const uint MaskWordId = 0xFF;
        public const uint MaskTime = 0x7FFFFF;
        
        /// <summary>
        /// WorldRuntimeIdStruct（如果超过下面参数的设定该ID会失效）。
        /// </summary>
        /// <param name="time">time不能超过8388607</param>
        /// <param name="sceneId">sceneId不能超过255</param>
        /// <param name="wordId">wordId不能超过255</param>
        /// <param name="sequence">sequence不能超过33554431</param>
        public WorldRuntimeIdStruct(uint time, uint sceneId, byte wordId, uint sequence)
        {
            // 因为都是在配置表里拿到参数、所以这个不做边界判定、能节省一点点性能。
            Time = time;
            SceneId = sceneId;
            WordId = wordId;
            Sequence = sequence;
        }

        public static implicit operator long(WorldRuntimeIdStruct runtimeIdStruct)
        {
            ulong result = runtimeIdStruct.Sequence;
            result |= (ulong)runtimeIdStruct.WordId << 25;
            result |= (ulong)(runtimeIdStruct.SceneId % (runtimeIdStruct.WordId * 1000)) << 33;
            result |= (ulong)runtimeIdStruct.Time << 41;
            return (long)result;
        }

        public static implicit operator WorldRuntimeIdStruct(long runtimeId)
        {
            var result = (ulong)runtimeId;
            var runtimeIdStruct = new WorldRuntimeIdStruct
            {
                Sequence = (uint)(result & MaskSequence)
            };
            result >>= 25;
            runtimeIdStruct.WordId = (byte)(result & MaskWordId);
            result >>= 8;
            runtimeIdStruct.SceneId = (uint)(result & MaskSceneId) + (uint)runtimeIdStruct.WordId * 1000;
            result >>= 8;
            runtimeIdStruct.Time = (uint)(result & MaskTime);
            return runtimeIdStruct;
        }
    }

    public sealed class WorldRuntimeIdFactory : IRuntimeIdFactory
    {
        private readonly uint _sceneId;
        private readonly byte _worldId;
        
        private uint _lastTime;
        private uint _lastSequence;
        private readonly long _epochNow;
        private readonly long _epoch1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000;
        
        private WorldRuntimeIdFactory() { }

        public WorldRuntimeIdFactory(uint sceneId, byte worldId) : this(TimeHelper.Now, sceneId, worldId) { }

        public WorldRuntimeIdFactory(long epochNow, uint sceneId, byte worldId)
        {
            switch (sceneId)
            {
                case > 255255:
                {
                    throw new NotSupportedException($"sceneId:{sceneId} cannot be greater than 255255");
                }
                case < 1001:
                {
                    throw new NotSupportedException($"sceneId:{sceneId} cannot be less than 1001");
                }
                default:
                {
                    _sceneId = sceneId;
                    _worldId = worldId;
                    _epochNow = epochNow - _epoch1970;
                    break;
                }
            }
        }

        public long Create
        {
            get
            {
                var time = (uint)((TimeHelper.Now - _epochNow) / 1000);
                
                if (time > _lastTime)
                {
                    _lastTime = time;
                    _lastSequence = 0;
                }
                else if (++_lastSequence > WorldRuntimeIdStruct.MaskSequence - 1)
                {
                    _lastTime++;
                    _lastSequence = 0;
                }

                return new WorldRuntimeIdStruct(time, _sceneId, _worldId, _lastSequence);
            }
        }
    }

    public sealed class WorldRuntimeIdFactoryTool : IIdFactoryTool
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetTime(ref long runtimeId)
        {
            var result = (ulong)runtimeId >> 41;
            return (uint)(result & WorldRuntimeIdStruct.MaskTime);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetSceneId(ref long runtimeId)
        {
            var result = (ulong)runtimeId >> 25;
            var worldId = (uint)(result & WorldRuntimeIdStruct.MaskWordId) * 1000;
            result >>= 8;
            return (uint)(result & WorldRuntimeIdStruct.MaskSceneId) + worldId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetWorldId(ref long runtimeId)
        {
            var result = (ulong)runtimeId >> 25;
            return (byte)(result & WorldRuntimeIdStruct.MaskWordId);
        }
    }
}