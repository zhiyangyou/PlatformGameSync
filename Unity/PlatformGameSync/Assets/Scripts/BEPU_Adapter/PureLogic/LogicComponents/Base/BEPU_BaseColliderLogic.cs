using System;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Materials;
using BEPUutilities;
using FixMath.NET;

public interface IColliderUpdater {
    public void OnBeforeUpdate();
    public void OnAfterUpdate();
}

public abstract partial class BEPU_BaseColliderLogic : IColliderUpdater, IDisposable {
    #region 属性和字段

    private Action<Vector3, Quaternion> _syncEntityPosAndRotationToRenderer;
    public bool isTrigger;
    public BEPU_EEntityType entityType = BEPU_EEntityType.Dyanmic;
    public bool useGravity = true; // 使用重力
    public float gravityScale = 1f; // 使用重力
    private CollisionRule _defaultCollisionRule = CollisionRule.Defer;
    public string name { get; private set; }
    public ConvexShape entityShape { get; private set; }

    private Material _material;

    private Material material {
        get {
            if (_material == null) {
                _material = new Material();
                _material.Bounciness = Fix64.Zero;
                _material.KineticFriction = Fix64.Zero;
                _material.StaticFriction = Fix64.Zero;
            }
            return _material;
        }
    }


    private BEPU_CustomEntity _entity;


    public BEPU_CustomEntity entity => _entity;

    #endregion

    #region public

    private BEPU_BaseColliderLogic() { }

    public BEPU_BaseColliderLogic(
        string name,
        ConvexShape shape,
        Action<Vector3, Quaternion> syncEntityPosAndRotationToRenderer) {
        entityShape = shape;
        this.name = name;
        _entity = new(shape);
        _entity.Material = material;
        _defaultCollisionRule = _entity.CollisionInformation.CollisionRules.Personal;
        _syncEntityPosAndRotationToRenderer = syncEntityPosAndRotationToRenderer;
        SyncAttrsToEntity();
    }

    public virtual void SyncAttrsToEntity() {
        entity.CollisionInformation.CollisionRules.Personal = isTrigger ? CollisionRule.NoSolver : _defaultCollisionRule;
        switch (entityType) {
            case BEPU_EEntityType.Kinematic:
                // Debug.LogError("暂时没玩明白这个是什么意思--.  暂时不处理");
                entity.BecomeKinematic();
                break;
            case BEPU_EEntityType.Dyanmic:
                var entityMass = entity.Mass == Fix64.Zero ? Fix64.One : entity.Mass;
                entity.BecomeDynamic(entityMass, entity.AutoLocalInertiaTensor(entityMass));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        entity.Gravity = useGravity
            ? (BEPU_PhysicsManager.Instance.SpaceGravity * (Fix64)gravityScale)
            : BEPUutilities.Vector3.Zero; // null代表使用默认的重力加速度值

        SyncExtendAttrsToEntity();
    }

    public virtual void Dispose() {
        BEPU_PhysicsManager.Instance.RemoveEntity(this);
    }

    #endregion

    #region virtual

    protected abstract void SyncExtendAttrsToEntity();

    #endregion
}