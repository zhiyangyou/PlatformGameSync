using System;
using FixMath.NET;
using Vector3 = BEPUutilities.Vector3;
using UnityEngine;
using Quaternion = BEPUutilities.Quaternion;

/// <summary>
/// RenderObject会持有LogicObject ,
/// 同样的: LogicObject也会持有RenderObject ,二者会互相持有
/// 同时具有的基础属性
/// </summary>
public abstract class LogicObject {
    private Vector3 _logicPos; // 逻辑位置

    // private Vector3 _logicDir; // 朝向
    private Quaternion _logicRotation; // 旋转角度
    private Fix64 _logicMoveSpeed = (Fix64)3; // 移动速度
    private Fix64 _logicAxis_X = Fix64.One; // 默认朝右
    private bool _isActive; // 是否激活
    private bool _isForceAllowMove; // 是否强制允许移动
    private bool _isForceNotAllowModifyDir; // 是否强制不允许修改位置
    public bool hasNewLogicPos = false;

    public BEPU_ColliderType ColliderType = BEPU_ColliderType.Box;

    public BEPU_BaseColliderLogic BaseColliderLogic { get; private set; }

    public Vector3 LogicPos {
        get { return _logicPos; }
        set {
            _logicPos = value;
            hasNewLogicPos = true;
        }
    }

    // public Vector3 LogicDir {
    //     get { return _logicDir; }
    //     protected set { _logicDir = value; }
    // }

    public Quaternion LogicRotation {
        get { return _logicRotation; }
        protected set { _logicRotation = value; }
    }

    public virtual Fix64 LogicMoveSpeed {
        get { return _logicMoveSpeed; }
        set { _logicMoveSpeed = value; }
    }

    public Fix64 LogicAxis_X {
        get { return _logicAxis_X; }
        protected set { _logicAxis_X = value; }
    }

    public bool IsActive {
        get => _isActive;
        set => _isActive = value;
    }

    public bool IsForceAllowMove {
        get => _isForceAllowMove;
        set => _isForceAllowMove = value;
    }

    public bool IsForceNotAllowModifyDir {
        get => _isForceNotAllowModifyDir;
        set => _isForceNotAllowModifyDir = value;
    }

    /// <summary>
    /// 渲染对象
    /// </summary>
    public RenderObject RenderObject { get; protected set; }

    private void InitColliderLogic() {
        var name = $"RenderObjectWithCollider-LogicObjectNam";
        switch (ColliderType) {
            case BEPU_ColliderType.None:
                BaseColliderLogic = null;
                break;
            case BEPU_ColliderType.Box:
                BaseColliderLogic = new BEPU_BoxColliderLogic(name, this, SyncEntityPosAndRotationToRenderer_Empty);
                break;
            case BEPU_ColliderType.Sphere:
                BaseColliderLogic = new BEPU_SphereColliderLogic(name, this, SyncEntityPosAndRotationToRenderer_Empty);
                break;
            case BEPU_ColliderType.Capsule:
                BaseColliderLogic = new BEPU_CapsuleColliderLogic(name, this, SyncEntityPosAndRotationToRenderer_Empty);
                break;
            default:
                throw new ArgumentOutOfRangeException($"InitColliderLogic 尚未不支持的碰撞体类型{ColliderType}");
        }
    }


    private void SyncEntityPosAndRotationToRenderer_Empty(Vector3 arg1, Quaternion arg2) { }

    private void TrySyncPhysicsPosAndRotation2LogicObject() {
        if (BaseColliderLogic != null) {
            LogicPos = BaseColliderLogic.entity.Position;
            LogicRotation = BaseColliderLogic.entity.Orientation;
        }
    }

    public abstract string LogicObjectNam { get; }

    public void BindRenderObject(RenderObject renderObject) {
        if (renderObject == null) {
            return;
            Debug.LogError("BindRenderObject renderObject参数是null");
        }
        this.RenderObject = renderObject;
        renderObject.SetLogicObject(this);
    }

    public virtual void OnCreate() {
        InitColliderLogic();
        if (this.BaseColliderLogic != null) {
            BEPU_PhysicsManagerUnity.Instance.AddEntity(BaseColliderLogic);
        }
    }

    public virtual void OnLogicFrameUpdate() {
        TrySyncPhysicsPosAndRotation2LogicObject();
    }

    public virtual void OnDestory() {
        if (this.BaseColliderLogic != null) {
            BEPU_PhysicsManagerUnity.Instance.RemoveEntity(BaseColliderLogic);
        }
    }
}