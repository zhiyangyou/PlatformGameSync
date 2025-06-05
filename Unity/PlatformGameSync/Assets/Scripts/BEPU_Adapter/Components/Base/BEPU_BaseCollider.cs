using System;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;
using Material = BEPUphysics.Materials.Material;

public abstract class BEPU_BaseCollider : MonoBehaviour {
    #region 属性和字段

    [SerializeField] private Vector3 center = Vector3.zero;
    [SerializeField] protected bool isTrigger;
    [SerializeField] protected BEPU_PhysicMaterialSO materialSo;

    // 刚体属性
    [SerializeField] protected BEPU_EEntityType entityType = BEPU_EEntityType.Dyanmic;
    [SerializeField] protected float mass = 1f;
    [SerializeField] protected float drag = 0f; // 移动时空气阻力
    [SerializeField] protected float angularDrag = 0f; // 旋转时的阻力
    [SerializeField] protected bool useGravity = true; // 使用重力
    [SerializeField] protected bool freezePos_X = false;
    [SerializeField] protected bool freezePos_Y = false;
    [SerializeField] protected bool freezePos_Z = false;
    [SerializeField] protected bool freezeRotation_X = false;
    [SerializeField] protected bool freezeRotation_Y = false;
    [SerializeField] protected bool freezeRotation_Z = false;

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
            SyncAttrsToEntity();
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
        SyncAttrsToEntity();
    }


    public virtual void SyncAllAttrsToEntity() {
        (materialSo ? materialSo.Data : DefaultAttr.DefaultMaterial).SyncToBEPUMat(material);
        entity.CollisionInformation.CollisionRules.Personal = isTrigger ? CollisionRule.NoSolver : _defaultCollisionRule;
        // Debug.LogError($"entityType :{entityType} {this.gameObject.name}");
        switch (entityType) {
            case BEPU_EEntityType.Kinematic:
                entity.BecomeKinematic();
                useGravity = false;
                break;
            case BEPU_EEntityType.Dyanmic:
                useGravity = true;
                var defaultMass = this.mass == 0f ? 1f : this.mass;
                entity.BecomeDynamic((Fix64)defaultMass, entity.AutoLocalInertiaTensor((Fix64)mass));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        entity.Mass = (Fix64)mass;
        entity.LinearDamping = (Fix64)drag;
        entity.AngularDamping = (Fix64)angularDrag;
        entity.Gravity = useGravity ? null : BEPUutilities.Vector3.Zero; // null代表使用默认的重力加速度值

        entity.freezePos_X = this.freezePos_X;
        entity.freezePos_Y = this.freezePos_Y;
        entity.freezePos_Z = this.freezePos_Z;
        entity.freezeRotation_X = this.freezeRotation_X;
        entity.freezeRotation_Y = this.freezeRotation_Y;
        entity.freezeRotation_Z = this.freezeRotation_Z;
        SyncAttrsToEntity();
        SyncPosAndRotation_ToPhysics();
    }

    protected virtual void SyncPosAndRotation_ToTransform() {
        if (Application.isPlaying) {
            transform.position = entity.Position.ToUnityVector3() + center;
            transform.rotation = entity.Orientation.ToUnityQuaternion();
        }
    }


    protected virtual void SyncPosAndRotation_ToPhysics() {
        entity.Position = (transform.position + center).ToFixedVector3();
        entity.Orientation = transform.rotation.ToFixedQuaternion();
    }

    void LateUpdate() {
        SyncPosAndRotation_ToTransform();
    }

    #endregion

    #region abstract

    protected abstract void SyncAttrsToEntity();
    protected abstract ConvexShape entityShape { get; }

    #endregion
}