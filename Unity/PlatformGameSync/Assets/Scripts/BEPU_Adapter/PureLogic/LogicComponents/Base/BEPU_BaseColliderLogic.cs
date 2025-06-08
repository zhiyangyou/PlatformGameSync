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

public partial class BEPU_BaseColliderLogic : IColliderUpdater, IDisposable {
    #region 属性和字段

    private Action _syncRenderPosAndRotationToEntity;
    private Action<Vector3, Quaternion> _syncEntityPosAndRotationToRenderer;
    protected bool isTrigger;
    protected BEPU_EEntityType entityType = BEPU_EEntityType.Dyanmic;
    protected bool useGravity = true; // 使用重力
    protected float gravityScale = 1f; // 使用重力
    private CollisionRule _defaultCollisionRule = CollisionRule.Defer;
    public string name { get; private set; }

    private Material _material;

    public Material material {
        get {
            if (_material == null) {
                _material = new Material();
                _material.Bounciness = (Fix64.Zero);
                _material.KineticFriction = (Fix64.Zero);
                _material.StaticFriction = (Fix64.Zero);
            }
            return _material;
        }
    }


    private BEPU_CustomEntity _entity;


    public BEPU_CustomEntity entity {
        get {
            if (_entity == null) {
                _entity = new(entityShape);
                // _entity.PositionUpdateMode = PositionUpdateMode.Continuous;
                _entity.Material = material;
                _defaultCollisionRule = _entity.CollisionInformation.CollisionRules.Personal;
            }
            return _entity;
        }
    }

    #endregion

    #region public

    private BEPU_BaseColliderLogic() { }

    public BEPU_BaseColliderLogic(string name, Action syncRenderPosAndRotationToEntity, Action<Vector3, Quaternion> syncEntityPosAndRotationToRenderer) {
        this.name = name;
        _syncRenderPosAndRotationToEntity = syncRenderPosAndRotationToEntity;
        _syncEntityPosAndRotationToRenderer = syncEntityPosAndRotationToRenderer;
        SyncAllAttrsToEntity();
        BEPU_PhysicsManager.Instance.AddEntity(this);
        InitInterpolateState();
    }

    public virtual void SyncAllAttrsToEntity() {
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

        SyncOtherAttrsToEntity();
        _syncRenderPosAndRotationToEntity?.Invoke();
    }

    public void Dispose() {
        BEPU_PhysicsManager.Instance.RemoveEntity(this);
    }

    // public virtual void SyncPosAndRotation_ToTransform() {
    //     SyncPosAndRotation_ToTransform(entity.Position.ToUnityVector3(), entity.Orientation.ToUnityQuaternion());
    // }
    //
    // public virtual void SyncPosAndRotation_ToTransform(Vector3 entityPos, Quaternion EntityRot) {
    //     if (Application.isPlaying) {
    //         transform.position = entityPos - center;
    //         transform.rotation = EntityRot;
    //     }
    // }
    //
    //
    // public virtual void SyncPosAndRotation_ToPhysics() {
    //     entity.Position = (transform.position + center).ToFixedVector3();
    //     entity.Orientation = transform.rotation.ToFixedQuaternion();
    // }


// #if UNITY_EDITOR
//
//     private void OnValidate() {
//         SyncAllAttrsToEntity();
//     }
// #endif

    #endregion

    #region abstract

    protected abstract void SyncOtherAttrsToEntity();
    protected abstract ConvexShape entityShape { get; }

    #endregion
}