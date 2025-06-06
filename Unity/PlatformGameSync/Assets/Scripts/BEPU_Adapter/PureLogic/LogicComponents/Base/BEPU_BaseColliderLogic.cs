using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;

public interface IColliderUpdater {
    public void OnBeforeUpdate();
    public void OnAfterUpdate();
}

public partial class BEPU_BaseColliderLogic : IColliderUpdater {
    #region 属性和字段

    private Vector3 center = Vector3.zero;
    protected bool isTrigger;
    protected BEPU_EEntityType entityType = BEPU_EEntityType.Dyanmic;
    protected float mass = 1f;
    protected float drag = 0f; // 移动时空气阻力
    protected float angularDrag = 0f; // 旋转时的阻力
    protected bool useGravity = true; // 使用重力
    protected float gravityScale = 1f; // 使用重力
    protected bool freezePos_X = false;
    protected bool freezePos_Y = false;
    protected bool freezePos_Z = false;
    protected bool freezeRotation_X = false;
    protected bool freezeRotation_Y = false;
    protected bool freezeRotation_Z = false;

    private CollisionRule _defaultCollisionRule = CollisionRule.Defer;


    private Material _material;

    public Material material {
        get {
            if (_material == null) {
                _material = new Material();
                (materialSo ? materialSo.Data : DefaultAttr.DefaultMaterial).SyncToBEPUMat(_material);
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

    public Vector3 Center {
        get => center;
        set {
            center = value;
            SyncAllAttrsToEntity();
        }
    }

    public bool IsTrigger {
        get => isTrigger;
        set {
            isTrigger = value;
            SyncAllAttrsToEntity();
        }
    }

    public BEPU_PhysicMaterialSO MaterialSo {
        get => materialSo;
        set {
            materialSo = value;
            SyncAllAttrsToEntity();
        }
    }

    public BEPU_EEntityType EntityType {
        get => entityType;
        set {
            entityType = value;
            SyncAllAttrsToEntity();
        }
    }

    public float Mass {
        get => mass;
        set {
            mass = value;
            SyncAllAttrsToEntity();
        }
    }

    public float Drag {
        get => drag;
        set {
            drag = value;
            SyncAllAttrsToEntity();
        }
    }

    public float AngularDrag {
        get => angularDrag;
        set {
            angularDrag = value;
            SyncAllAttrsToEntity();
        }
    }

    public bool UseGravity {
        get => useGravity;
        set {
            useGravity = value;
            SyncAllAttrsToEntity();
        }
    }

    #endregion

    #region protected

    private void Awake() {
        SyncAllAttrsToEntity();
        if (Application.isPlaying) {
            BEPU_PhysicsManager.Instance.AddEntity(this);
        }
        InitInterpolateState();
    }


    public virtual void SyncAllAttrsToEntity() {
        (materialSo ? materialSo.Data : DefaultAttr.DefaultMaterial).SyncToBEPUMat(material);
        entity.CollisionInformation.CollisionRules.Personal = isTrigger ? CollisionRule.NoSolver : _defaultCollisionRule;
        // Debug.LogError($"entityType :{entityType} {this.gameObject.name}");
        switch (entityType) {
            case BEPU_EEntityType.Kinematic:
                // Debug.LogError("暂时没玩明白这个是什么意思--.  暂时不处理");
                entity.BecomeKinematic();
                break;
            case BEPU_EEntityType.Dyanmic:
                var defaultMass = this.mass == 0f ? 1f : this.mass;
                entity.BecomeDynamic((Fix64)defaultMass, entity.AutoLocalInertiaTensor((Fix64)mass));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        entity.Mass = (Fix64)mass;
        entity.LinearDamping = (Fix64)drag;
        entity.AngularDamping = (Fix64)angularDrag;
        entity.Gravity = useGravity
            ? (BEPU_PhysicsManager.Instance.SpaceGravity * (Fix64)gravityScale)
            : BEPUutilities.Vector3.Zero; // null代表使用默认的重力加速度值
        entity.freezePos_X = this.freezePos_X;
        entity.freezePos_Y = this.freezePos_Y;
        entity.freezePos_Z = this.freezePos_Z;
        entity.freezeRotation_X = this.freezeRotation_X;
        entity.freezeRotation_Y = this.freezeRotation_Y;
        entity.freezeRotation_Z = this.freezeRotation_Z;
        SyncAttrsToEntity();
        SyncPosAndRotation_ToPhysics();
    }

    public virtual void SyncPosAndRotation_ToTransform() {
        SyncPosAndRotation_ToTransform(entity.Position.ToUnityVector3(), entity.Orientation.ToUnityQuaternion());
    }

    public virtual void SyncPosAndRotation_ToTransform(Vector3 entityPos, Quaternion EntityRot) {
        if (Application.isPlaying) {
            transform.position = entityPos - center;
            transform.rotation = EntityRot;
        }
    }


    public virtual void SyncPosAndRotation_ToPhysics() {
        entity.Position = (transform.position + center).ToFixedVector3();
        entity.Orientation = transform.rotation.ToFixedQuaternion();
    }


#if UNITY_EDITOR

    private void OnValidate() {
        SyncAllAttrsToEntity();
    }
#endif


    private void OnDestroy() {
        if (Application.isPlaying) {
            BEPU_PhysicsManager.Instance.RemoveEntity(this);
        }
        OnRelease();
    }

    #endregion

    #region virtual

    protected virtual void OnRelease() { }
    protected abstract void SyncAttrsToEntity();
    protected abstract ConvexShape entityShape { get; }

    #endregion

    public void OnBeforeUpdate() {
        
    }
    public void OnAfterUpdate() {
        
    }
}