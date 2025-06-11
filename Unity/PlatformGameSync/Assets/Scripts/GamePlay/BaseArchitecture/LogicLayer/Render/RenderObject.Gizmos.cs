using System;
using GamePlay;
using UnityEngine;

/// <summary>
/// 渲染对象基础:位置, 旋转...
/// RenderObject会持有LogicObject ,
/// 同样的: LogicObject也会持有RenderObject ,二者会互相持有
/// </summary>
public partial class RenderObject {
#if UNITY_EDITOR

    private void OnDrawGizmos() {
        if (baseColliderLogic != null) {
            switch (baseColliderType) {
                case BEPU_ColliderType.None:
                    break;
                case BEPU_ColliderType.Box:
                    BEPU_BoxColliderMono.DrawGizmos(baseColliderLogic);
                    break;
                case BEPU_ColliderType.Sphere:
                    BEPU_SphereColliderMono.DrawGizmos(baseColliderLogic);
                    break;
                case BEPU_ColliderType.Capsule:
                    BEPU_CapsuleColliderMono.DrawGizmos(baseColliderLogic);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"不支持的碰撞体类型{this.LogicObject.ColliderType}");
            }
        }
    }


#endif
}