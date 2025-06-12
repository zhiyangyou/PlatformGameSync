using System;
using BEPUphysics.CollisionRuleManagement;
using FixMath.NET;
using GamePlay;
using UnityEngine;


public partial class RenderObject {
    #region 属性和字段

    [SerializeField] BEPU_ColliderType ColliderTypeForEditorTest = BEPU_ColliderType.Box;

    private BEPU_BaseColliderLogic _baseColliderLogicForEditor = null;

    protected BEPU_ColliderType baseColliderType {
        get {
#if UNITY_EDITOR
            if (Application.isPlaying) {
                if (LogicObject != null) {
                    return LogicObject.ColliderType;
                }
                else {
                    return BEPU_ColliderType.None;
                }
            }
            else {
                return ColliderTypeForEditorTest;
            }
#else
            if (LogicObject != null) {
                return LogicObject.ColliderType;
            }
            else {
                return BEPU_ColliderType.None;
            }

#endif
        }
    }

    protected BEPU_BaseColliderLogic baseColliderLogic {
        get {
#if UNITY_EDITOR
            if (Application.isPlaying) {
                return LogicObject?.BaseColliderLogic;
            }
            else {
                if (_baseColliderLogicForEditor == null) {
                    ProcessEditorCollider();
                }
                return _baseColliderLogicForEditor;
            }
#else
            return LogicObject?.BaseColliderLogic;

#endif
        }
    }


    2025年6月12日23:38:35  工作断点, 处理此处的定点数序列化值
    
    [SerializeField] private Vector3 center = Vector3.zero;
    [SerializeField] protected bool isTrigger;
    [SerializeField] protected BEPU_PhysicMaterialSO materialSo;

    // 刚体属性
    [SerializeField] protected BEPU_EEntityType entityType = BEPU_EEntityType.Dyanmic;
    [SerializeField] protected float mass = 1f;
    [SerializeField] [Range(0f, 1f)] protected float drag = 0f; // 移动时空气阻力
    [SerializeField] protected float angularDrag = 0f; // 旋转时的阻力
    [SerializeField] protected bool useGravity = true; // 使用重力
    [SerializeField] protected float gravityScale = 1f; // 使用重力
    [SerializeField] protected bool freezePos_X = false;
    [SerializeField] protected bool freezePos_Y = false;
    [SerializeField] protected bool freezePos_Z = false;
    [SerializeField] protected bool freezeRotation_X = false;
    [SerializeField] protected bool freezeRotation_Y = false;
    [SerializeField] protected bool freezeRotation_Z = false;
    [SerializeField] protected BEPU_LayerDefine layer = BEPU_LayerDefine.Default;

    private CollisionRule _defaultCollisionRule = CollisionRule.Defer;

    #endregion

#if UNITY_EDITOR
    protected virtual void SyncRenderPosAndRotationToEntity() {
        if (baseColliderLogic  == null) {
            return;
        }
        baseColliderLogic.entity.Position = (transform.position + center).ToFixedVector3();
        baseColliderLogic.entity.Orientation = transform.rotation.ToFixedQuaternion();
    }

    protected virtual void OnValidate() {
        SyncAllAttrsToEntity();
        SyncRenderPosAndRotationToEntity();
    }
#endif

    public virtual void SyncExtendAttrsToEntity() { }

    public virtual void SyncAllAttrsToEntity() {
        if (baseColliderLogic == null) {
            return;
        }

        if (materialSo != null) {
            materialSo.Data.SyncToBEPUMat(this.baseColliderLogic.entity.Material);
        }
        baseColliderLogic.isTrigger = isTrigger;
        baseColliderLogic.entityType = entityType;
        baseColliderLogic.entity.CollisionInformation.CollisionRules.Personal = isTrigger ? CollisionRule.NoSolver : _defaultCollisionRule;
        baseColliderLogic.entity.Mass = (Fix64)mass;
        baseColliderLogic.entity.LinearDamping = (Fix64)drag;
        baseColliderLogic.entity.AngularDamping = (Fix64)angularDrag;
        baseColliderLogic.entity.Gravity = useGravity
            ? (BEPU_PhysicsManagerUnity.Instance.SpaceGravity * (Fix64)gravityScale)
            : BEPUutilities.Vector3.Zero; // null代表使用默认的重力加速度值
        baseColliderLogic.entity.freezePos_X = this.freezePos_X;
        baseColliderLogic.entity.freezePos_Y = this.freezePos_Y;
        baseColliderLogic.entity.freezePos_Z = this.freezePos_Z;
        baseColliderLogic.entity.freezeRotation_X = this.freezeRotation_X;
        baseColliderLogic.entity.freezeRotation_Y = this.freezeRotation_Y;
        baseColliderLogic.entity.freezeRotation_Z = this.freezeRotation_Z;
        baseColliderLogic.entity.Layer = layer;
        baseColliderLogic.entity.CollisionInformation.CollisionRules.Group = BEPU_PhysicsManagerUnity.Instance.GetGroupByLayer(this.layer);
        if (LogicObject != null) {
            LogicObject.PhysicsEntryCenter = this.center.ToFixedVector3();
        }
        baseColliderLogic.SyncAttrsToEntity();
        ProcessEditorCollider();
        SyncExtendAttrsToEntity();
    }


    private void ProcessEditorCollider() {
#if UNITY_EDITOR
        if (!Application.isPlaying) {
            switch (ColliderTypeForEditorTest) {
                case BEPU_ColliderType.None:
                    break;
                case BEPU_ColliderType.Box:
                    _baseColliderLogicForEditor = new BEPU_BoxColliderLogic("forEditorTest_box", this, null);
                    break;
                case BEPU_ColliderType.Sphere:
                    _baseColliderLogicForEditor = new BEPU_SphereColliderLogic("forEditorTest_sphere", this, null);
                    break;
                case BEPU_ColliderType.Capsule:
                    _baseColliderLogicForEditor = new BEPU_CapsuleColliderLogic("forEditorTest_capsule", this, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
#endif
    }
}