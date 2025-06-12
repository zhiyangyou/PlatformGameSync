using System;
using BEPUphysics.CollisionRuleManagement;
using FixMath.NET;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;

public abstract class BEPU_BaseColliderMono : MonoBehaviour {
    #region 属性和字段

    [SerializeField] private FVector3 center = FVector3.Zero;
    [SerializeField] protected bool isTrigger;
    [SerializeField] protected BEPU_PhysicMaterialSO materialSo;

    // 刚体属性
    [SerializeField] protected BEPU_EEntityType entityType = BEPU_EEntityType.Dyanmic;
    [SerializeField] protected Fix64 mass = Fix64.One;
    [SerializeField] protected Fix64 drag = Fix64.Zero; // 移动时空气阻力
    [SerializeField] protected Fix64 angularDrag = Fix64.Zero; // 旋转时的阻力
    [SerializeField] protected Fix64 gravityScale = Fix64.One; // 使用重力
    [SerializeField] protected bool useGravity = true; // 使用重力
    [SerializeField] protected bool freezePos_X = false;
    [SerializeField] protected bool freezePos_Y = false;
    [SerializeField] protected bool freezePos_Z = false;
    [SerializeField] protected bool freezeRotation_X = false;
    [SerializeField] protected bool freezeRotation_Y = false;
    [SerializeField] protected bool freezeRotation_Z = false;
    [SerializeField] protected BEPU_LayerDefine layer = BEPU_LayerDefine.Default;

    [SerializeField] protected bool autoScaleToColliderSize = false;
    private CollisionRule _defaultCollisionRule = CollisionRule.Defer;


    protected virtual void SyncEntityPosAndRotationToRenderer(BEPUutilities.Vector3 entityPos, BEPUutilities.Quaternion EntityRot) {
        if (Application.isPlaying) {
            transform.position = (entityPos - center).ToUnityVector3();
            transform.rotation = EntityRot.ToUnityQuaternion();
        }
    }

    protected virtual void SyncRenderPosAndRotationToEntity() {
        // TODO 这边不能使用Transform上序列化的值直接赋给定点物理世界, 可能造成差异, 需要序列化改造!!!
        entity.Position = transform.position.ToFixedVector3() + center;
        entity.Orientation = transform.rotation.ToFixedQuaternion();
    }

    public BEPU_CustomEntity entity => colliderLogic.entity;

    #endregion

    #region protected

    private void Awake() {
        SyncAllAttrsToEntity();
        colliderLogic.InitInterpolateState();
        if (Application.isPlaying) {
            BEPU_PhysicsManagerUnity.Instance.AddEntity(this.colliderLogic);
        }
    }


    public virtual void SyncAllAttrsToEntity() {
        if (materialSo != null) {
            materialSo.Data.SyncToBEPUMat(this.colliderLogic.entity.Material);
        }
        colliderLogic.isTrigger = isTrigger;
        colliderLogic.entityType = entityType;
        entity.CollisionInformation.CollisionRules.Personal = isTrigger ? CollisionRule.NoSolver : _defaultCollisionRule;
        entity.Mass = (Fix64)mass;
        entity.LinearDamping = (Fix64)drag;
        entity.AngularDamping = (Fix64)angularDrag;
        entity.Gravity = useGravity
            ? (BEPU_PhysicsManagerUnity.Instance.SpaceGravity * (Fix64)gravityScale)
            : BEPUutilities.Vector3.Zero; // null代表使用默认的重力加速度值
        entity.freezePos_X = this.freezePos_X;
        entity.freezePos_Y = this.freezePos_Y;
        entity.freezePos_Z = this.freezePos_Z;
        entity.freezeRotation_X = this.freezeRotation_X;
        entity.freezeRotation_Y = this.freezeRotation_Y;
        entity.freezeRotation_Z = this.freezeRotation_Z;
        entity.Layer = layer;
        entity.CollisionInformation.CollisionRules.Group = BEPU_PhysicsManagerUnity.Instance.GetGroupByLayer(this.layer);
        colliderLogic.SyncAttrsToEntity();
        SyncRenderPosAndRotationToEntity();
        SyncExtendAttrsToEntity();
    }


    protected virtual void OnValidate() {
        SyncAllAttrsToEntity();
    }


    private void OnDestroy() {
        if (Application.isPlaying) {
            BEPU_PhysicsManagerUnity.Instance.RemoveEntity(colliderLogic);
        }
        OnRelease();
    }

    private void LateUpdate() {
        colliderLogic.DoPositionInterpolateUpdate();
    }

    #endregion

    #region virtual

    public abstract BEPU_BaseColliderLogic colliderLogic { get; }
    protected abstract void OnRelease();
    protected abstract string ColliderName { get; }
    public abstract void SyncExtendAttrsToEntity();

    #endregion
}