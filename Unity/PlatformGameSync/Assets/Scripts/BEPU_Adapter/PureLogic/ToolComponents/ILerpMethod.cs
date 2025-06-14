using System;
using BEPUphysics.Entities;
using FixMath.NET;


public enum BEPU_LerpMethod {
    Interpolate,
    Extrapolate,
}
public interface ILerpMethod {
    public void Init(Entity entity, BEPU_BaseColliderLogic baseCollider, Fix64 physicsTimeStep);
    public void BeforeWorldUpdate();
    public void AfterWorldUpdate();
    public (BEPUutilities.Vector3 interPos, BEPUutilities.Quaternion interRotation) UpdateLearp(Fix64 deltaTime);
}

public static class LerpMethodFactory {
    public static ILerpMethod NewMethod(BEPU_LerpMethod method) {
        ILerpMethod lerper = null;
        switch (method) {
            case BEPU_LerpMethod.Interpolate:
                lerper = new LerpMethod_Interpolate();
                break;
            case BEPU_LerpMethod.Extrapolate:
                lerper = new LerpMethod_Extrapolate();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
        return lerper;
    }
}