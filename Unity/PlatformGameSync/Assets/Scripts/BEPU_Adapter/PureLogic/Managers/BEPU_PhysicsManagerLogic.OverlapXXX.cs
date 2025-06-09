using System;
using System.Collections.Generic;
using BEPU_Adapter.Pool;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUutilities;
using FixMath.NET;


// 实现和Unity的Physics.OverlayXXX等价的功能
public abstract partial class BEPU_PhysicsManagerLogic<T> {
    private int Overlap(
        BEPU_LayerDefine layerMask,
        List<BEPU_BaseColliderLogic> listResults,
        Action<List<BroadPhaseEntry>> onGetGetResult) {
        // 用于存储查询结果的列表
        var results = ListPool<BroadPhaseEntry>.Get();

        // 执行重叠查询
        onGetGetResult(results);

        foreach (var entry in results) {
            ConvexCollidable c = entry as ConvexCollidable;
            if (c is { Entity: BEPU_CustomEntity { UserObj: BEPU_BaseColliderLogic baseColliderLogic } }) {
                if (layerMask == baseColliderLogic.entity.Layer) {
                    listResults.Add(baseColliderLogic);
                }
            }
        }
        ListPool<BroadPhaseEntry>.Release(results);
        return listResults.Count;
    }

    public int OverlapBoxAll(
        Vector3 centerPos,
        Vector3 halfWHL,
        BEPU_LayerDefine layerMask,
        List<BEPU_BaseColliderLogic> listResults
    ) {
        Vector3 min = centerPos - halfWHL;
        Vector3 max = centerPos + halfWHL;
        BoundingBox boundingBox = new BoundingBox(min, max);
        return Overlap(layerMask, listResults, (results) => { Space.BroadPhase.QueryAccelerator.GetEntries(boundingBox, results); });
    }

    public int OverlapCircleAll(
        Vector3 centerPos,
        Fix64 radiu,
        BEPU_LayerDefine layerMask,
        List<BEPU_BaseColliderLogic> listResults
    ) {
        BoundingSphere boundingSphere = new(centerPos, radiu);


        return Overlap(layerMask, listResults, (results) => { Space.BroadPhase.QueryAccelerator.GetEntries(boundingSphere, results); });
    }
}