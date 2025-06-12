using System;
using System.Collections.Generic;
using BEPU_Adapter.Pool;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUutilities;
using FixMath.NET;
using Vector3 = BEPUutilities.Vector3;

public struct BEPU_RayCastReuslt {
    public BEPU_BaseColliderLogic baseLogic;
    public RayHit HitData;
}

public abstract partial class BEPU_PhysicsManagerLogic<T> {
    private bool Filter_Any(BEPU_BaseColliderLogic arg) {
        return arg != null;
    }

    public bool RaycastAnyLayer(
        Vector3 origin,
        Vector3 direction,
        Fix64 maxDistance,
        BEPU_LayerDefine layer) {
        List<BEPU_RayCastReuslt> listRet = ListPool<BEPU_RayCastReuslt>.Get();
        var ret = Raycast(origin, direction, maxDistance, (logic) => logic.entity.Layer == layer, listRet);
        var hasAny = listRet.Count > 0;
        ListPool<BEPU_RayCastReuslt>.Release(listRet);
        return hasAny;
    }

    public bool Raycast(
        Vector3 origin,
        Vector3 direction,
        Fix64 maxDistance,
        BEPU_LayerDefine layer,
        List<BEPU_RayCastReuslt> listRet) {
        var ret = Raycast(origin, direction, maxDistance, (logic) => logic.entity.Layer == layer, listRet);
        return ret;
    }


    public bool Raycast(
        Vector3 origin,
        Vector3 direction,
        Fix64 maxDistance,
        IList<BEPU_RayCastReuslt> baseColliderLogics) {
        return Raycast(origin, direction, maxDistance, Filter_Any, baseColliderLogics);
    }


    public bool Raycast(
        Vector3 origin,
        Vector3 direction,
        Fix64 maxDistance,
        Func<BEPU_BaseColliderLogic, bool> filter,
        IList<BEPU_RayCastReuslt> baseColliderLogics) {
        // 标准化方向向量
        Vector3 normalizedDir = direction;
        if (normalizedDir.LengthSquared() != Fix64.One) {
            normalizedDir.Normalize();
        }


        baseColliderLogics.Clear();
        // 创建射线
        BEPUutilities.Ray ray = new BEPUutilities.Ray(origin, normalizedDir);


        var listForRet = ListPool<RayCastResult>.Get();

        try {
            bool hit = Space.RayCast(ray, maxDistance, listForRet);
            for (int i = 0; i < listForRet.Count; i++) {
                var hitResult = listForRet[i];
                // hitResult.HitObject.Tag
                ConvexCollidable c = hitResult.HitObject as ConvexCollidable;
                if (c is { Entity: BEPU_CustomEntity { UserObj: BEPU_BaseColliderLogic baseColliderLogic } }) {
                    try {
                        if (filter.Invoke(baseColliderLogic)) {
                            baseColliderLogics.Add(new BEPU_RayCastReuslt() {
                                HitData = hitResult.HitData,
                                baseLogic = baseColliderLogic,
                            });
                        }
                    }
                    catch (Exception e) {
                        BEPU_Logger.LogException(e);
                    }
                }
            }
            return hit;
        }
        catch (Exception e) {
            BEPU_Logger.LogException(e);
        }
        finally {
            ListPool<RayCastResult>.Release(listForRet);
        }
        return false;
    }
}