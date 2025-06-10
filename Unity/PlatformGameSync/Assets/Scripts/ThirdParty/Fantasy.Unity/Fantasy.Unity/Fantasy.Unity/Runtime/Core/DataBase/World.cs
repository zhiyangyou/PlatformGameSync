#pragma warning disable CS8603 // Possible null reference return.
#if FANTASY_NET
using Fantasy.Platform.Net;

namespace Fantasy.DataBase
{
    /// <summary>
    /// 表示一个游戏世界。
    /// </summary>
    public sealed class World : IDisposable
    {
        /// <summary>
        /// 获取游戏世界的唯一标识。
        /// </summary>
        public byte Id { get; private init; }
        /// <summary>
        /// 获取游戏世界的数据库接口。
        /// </summary>
        public IDataBase DataBase { get; private init; }
        /// <summary>
        /// 获取游戏世界的配置信息。
        /// </summary>
        public WorldConfig Config => WorldConfigData.Instance.Get(Id);

        /// <summary>
        /// 使用指定的配置信息创建一个游戏世界实例。
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="worldConfigId"></param>
        private World(Scene scene, byte worldConfigId)
        {
            Id = worldConfigId;
            var worldConfig = Config;
            var dbType = worldConfig.DbType.ToLower();

            switch (dbType)
            {
                case "mongodb":
                {
                    DataBase = new MongoDataBase();
                    DataBase.Initialize(scene, worldConfig.DbConnection, worldConfig.DbName);
                    break;
                }
                default:
                {
                    throw new Exception("No supported database");
                }
            }
        }

        /// <summary>
        /// 创建一个指定唯一标识的游戏世界实例。
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="id">游戏世界的唯一标识。</param>
        /// <returns>游戏世界实例。</returns>
        internal static World Create(Scene scene, byte id)
        {
            if (!WorldConfigData.Instance.TryGet(id, out var worldConfigData))
            {
                return null;
            }

            return string.IsNullOrEmpty(worldConfigData.DbConnection) ? null : new World(scene, id);
        }

        /// <summary>
        /// 释放游戏世界资源。
        /// </summary>
        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}

#endif