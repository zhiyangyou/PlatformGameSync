using System;
using BEPUphysics;
using BEPUphysics.CollisionRuleManagement;
using UnityEngine;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities;
using FixMath.NET;
using Material = BEPUphysics.Materials.Material;
using Space = BEPUphysics.Space;


[ExecuteInEditMode]
public partial class BEPU_BoxCollider : BEPU_BaseCollider {
    #region 属性和字段

    // collider 属性
    [SerializeField] private bool isTrigger;
    [SerializeField] private Vector3 center = Vector3.zero;
    [SerializeField] private Vector3 size = Vector3.one;
    [SerializeField] private BEPU_PhysicMaterialSO materialSo;


    // 刚体属性
    [SerializeField] private BEPU_EEntityType entityType = BEPU_EEntityType.Dyanmic;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float drag = 0f; // 移动时空气阻力
    [SerializeField] private float angularDrag = 0f; // 旋转时的阻力
    [SerializeField] private bool useGravity = true; // 使用重力
    [SerializeField] private bool freezePos_X = false;
    [SerializeField] private bool freezePos_Y = false;
    [SerializeField] private bool freezePos_Z = false;
    [SerializeField] private bool freezeRotation_X = false;
    [SerializeField] private bool freezeRotation_Y = false;
    [SerializeField] private bool freezeRotation_Z = false;


    public bool IsTrigger {
        get => isTrigger;
        set {
            isTrigger = value;
            SyncAttrsToEntity();
        }
    }

    public Vector3 Center {
        get => center;
        set {
            center = value;
            SyncAttrsToEntity();
        }
    }

    public Vector3 Size {
        get { return size; }
        set {
            size = value;
            SyncAttrsToEntity();
        }
    }

    public BEPU_PhysicMaterialSO MaterialSo {
        get => materialSo;
        set {
            materialSo = value;
            SyncAttrsToEntity();
        }
    }

    public BEPU_EEntityType EntityType {
        get => entityType;
        set {
            entityType = value;
            SyncAttrsToEntity();
        }
    }

    public float Mass {
        get => mass;
        set {
            mass = value;
            SyncAttrsToEntity();
        }
    }

    public float Drag {
        get => drag;
        set {
            drag = value;
            SyncAttrsToEntity();
        }
    }

    public float AngularDrag {
        get => angularDrag;
        set {
            angularDrag = value;
            SyncAttrsToEntity();
        }
    }

    public bool UseGravity {
        get => useGravity;
        set {
            useGravity = value;
            SyncAttrsToEntity();
        }
    }


    private BEPU_CustomEntity _entity;

    public override BEPU_CustomEntity entity {
        get {
            if (_entity == null) {
                _entity = new(boxShape);
                _entity.Material = material;
                _defaultCollisionRule = _entity.CollisionInformation.CollisionRules.Personal;
            }
            return _entity;
        }
    }

    public override BEPU_EEntityType BEPUEntityType => entityType;

    private BoxShape _boxShape;

    public BoxShape boxShape {
        get {
            if (_boxShape == null) {
                _boxShape = new BoxShape(Width, Height, Length);
            }
            return _boxShape;
        }
    }

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

    private CollisionRule _defaultCollisionRule = CollisionRule.Defer;

    public Fix64 Width => (Fix64)(size.x * transform.lossyScale.x);
    public Fix64 Height => (Fix64)(size.y * transform.lossyScale.y);
    public Fix64 Length => (Fix64)(size.z * transform.lossyScale.z);

    #endregion

    #region life-cycle

    private void Awake() {
        SyncAttrsToEntity();
    }


#if UNITY_EDITOR
    private void OnValidate() {
        SyncAttrsToEntity();
    }
#endif

    private void LateUpdate() {
        SyncPosAndRotation_ToTransform();
    }

    #endregion

    #region private

    public void SyncAttrsToEntity() {
        // 物理材质球同步 
        (materialSo ? materialSo.Data : DefaultAttr.DefaultMaterial).SyncToBEPUMat(material);
        boxShape.Width = this.Width;
        boxShape.Height = this.Height;
        boxShape.Length = this.Length;
        entity.CollisionInformation.CollisionRules.Personal = isTrigger ? CollisionRule.NoSolver : _defaultCollisionRule;
        // Debug.LogError($"entityType :{entityType} {this.gameObject.name}");
        switch (entityType) {
            case BEPU_EEntityType.Kinematic:
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
        entity.Gravity = useGravity ? null : BEPUutilities.Vector3.Zero; // null代表使用默认的重力加速度值

        entity.freezePos_X = this.freezePos_X;
        entity.freezePos_Y = this.freezePos_Y;
        entity.freezePos_Z = this.freezePos_Z;
        entity.freezeRotation_X = this.freezeRotation_X;
        entity.freezeRotation_Y = this.freezeRotation_Y;
        entity.freezeRotation_Z = this.freezeRotation_Z;

        SyncPosAndRotation_ToPhysics();
    }

    private void SyncPosAndRotation_ToPhysics() {
        entity.Position = (transform.position + center).ToFixedVector3();
        entity.Orientation = transform.rotation.ToFixedQuaternion();
    }

    private void SyncPosAndRotation_ToTransform() {
        if (Application.isPlaying) {
            transform.position = entity.Position.ToUnityVector3() + center;
            transform.rotation = entity.Orientation.ToUnityQuaternion();
        }
    }

    #endregion
}