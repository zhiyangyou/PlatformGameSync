
/// <summary>
/// 层级定义， 只能顺序向下递增， 如果出现删除中间的情况，要重新配置并生成
/// </summary>
public enum BEPU_LayerDefaine : int {
    Default = 0,
    Player = 1, // 玩家
    Enemy = 2, // 敌人
    Envirement = 3, // 环境
    Trigger = 4, // 触发器 
    PlayerBullet = 5, // 玩家子弹 
    EnemyBullet = 6, // 敌人子弹
    LayerCount,
}