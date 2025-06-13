using GamePlay;
using UnityEngine;

/// <summary>
/// 渲染对象基础:位置, 旋转...
/// RenderObject会持有LogicObject ,
/// 同样的: LogicObject也会持有RenderObject ,二者会互相持有
/// </summary>
public partial class RenderObject : MonoBehaviour {
    #region 属性和字段

    public LogicObject LogicObject { get; private set; }

    /// <summary>
    /// 位置插值速度?
    /// </summary>
    protected float _smoothPosSpeed => 20f;

    private bool _isUpdatePosAndDir = true;

    /// <summary>
    /// 是本地玩家吗?
    /// </summary>
    private bool _isLocalPlayer = false;

    /// <summary>
    /// 预测的位置
    /// </summary>
    private Vector3 _preTargetPos;

    /// <summary>
    /// 预测的次数
    /// </summary>
    private int _curPreMoveCount = 0;

    #endregion

    #region life-cycle

    protected virtual void Update() {
        if (LogicObject != null) {
            UpdatePosRotation();
        }
    }

    #endregion

    #region public

    public void ForceUpdateRotationNow() {
        if (LogicObject != null) {
            UpdateRotation();
        }
    }

    public void SetIsLocalPlayer(bool v) {
        _isLocalPlayer = v;
    }

    public void SetLogicObject(LogicObject logicObject, bool isUpdatePosAndDir = true) {
        this.LogicObject = logicObject;
        this._isUpdatePosAndDir = isUpdatePosAndDir;
        // 初始化位置
        transform.position = logicObject.LogicPos.ToUnityVector3();
        if (!isUpdatePosAndDir) {
            transform.localPosition = Vector3.zero;
        }
        UpdatePosRotation();
    }

    public virtual void OnCreate() {
        SyncAllAttrsToEntity();
    }

    /// <summary>
    /// 通用逻辑:更新方向
    /// </summary>
    public virtual void UpdateRotation() {
        transform.rotation = LogicObject.LogicRotation.ToUnityQuaternion();
    }

    /// <summary>
    /// 通用逻辑:更新位置
    /// </summary>
    public virtual void UpdatePosition() {
        // 本地预测和回滚
        if (_isLocalPlayer) {
            if (LogicObject.hasNewLogicPos) {
                _preTargetPos = LogicObject.LogicPos.ToUnityVector3();
                LogicObject.hasNewLogicPos = false;
                _curPreMoveCount = 0; // 真正的逻辑位置从网络到达, 
                // Debug.LogError($"后端逻辑位置抵达:{LogicObject.LogicPos.ToUnityVector3()}");
            }
            else {
                // 进行预测
                if (_curPreMoveCount >= GameConstConfigs.MaxPreMoveCount) {
                    return; // 超出预测最大限度, 不执行预测逻辑了
                }
                // 计算预测位置的增量
                Vector3 deltaPos = LogicObject.LogicRotation.ToUnityQuaternion().eulerAngles * ((float)LogicObject.LogicMoveSpeed * Time.deltaTime);
                _preTargetPos += deltaPos;
                _curPreMoveCount++;
                // Debug.LogError($"预测位置:{_preTargetPos}");
            }
            transform.position = Vector3.Lerp(transform.position, _preTargetPos, Time.deltaTime * _smoothPosSpeed);
        }
        else {
            transform.position = Vector3.Lerp(transform.position, LogicObject.LogicPos.ToUnityVector3(), Time.deltaTime * _smoothPosSpeed);
            // transform.position = LogicObject.LogicPos.ToUnityVector3();
        }
    }

    #endregion

    #region private

    private void UpdatePosRotation() {
        if (!_isUpdatePosAndDir) {
            return;
        }
        UpdateRotation();
        UpdatePosition();
    }

    #endregion
}