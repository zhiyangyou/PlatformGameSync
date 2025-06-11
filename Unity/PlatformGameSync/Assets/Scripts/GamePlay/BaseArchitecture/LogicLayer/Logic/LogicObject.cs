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
public abstract partial class LogicObject {
    private Vector3 _logicPos; // 逻辑位置

    private Quaternion _logicRotation = Quaternion.Identity; // 旋转角度
    private Fix64 _logicMoveSpeed = (Fix64)3; // 移动速度
    private Fix64 _logicAxis_X = Fix64.One; // 默认朝右
    private bool _isActive; // 是否激活
    private bool _isForceAllowMove; // 是否强制允许移动
    private bool _isForceNotAllowModifyDir; // 是否强制不允许修改位置
    public bool hasNewLogicPos = false;

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
        TrySyncPhysicsPosAndRot2Logic();
    }

    public virtual void OnDestory() {
        if (this.BaseColliderLogic != null) {
            BEPU_PhysicsManagerUnity.Instance.RemoveEntity(BaseColliderLogic);
        }
    }
}