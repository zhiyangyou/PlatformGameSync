using System.Collections.Generic;
using BEPUphysics.CollisionRuleManagement;

public abstract partial class BEPU_PhysicsManagerLogic<T> {
    private Dictionary<BEPU_LayerDefine, CollisionGroup> _dicGroup = new();

    public CollisionGroup GetGroupByLayer(BEPU_LayerDefine layer) {
        if (_dicGroup.TryGetValue(layer, out var g)) {
            return g;
        }
        else {
            BEPU_Logger.LogError($"不存在Layer{layer} 对应的Collision");

            return null;
        }
    }
    
    
}