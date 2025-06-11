using System.Collections.Generic;

namespace WorldSpace.GameWorld {
    public class RoleModel {
        public bool IsLocalPlayer => true;
        public int RoleType;
    }


    public class BattleDataMgr : IDataBehaviour {
        public List<RoleModel> listRoleModels { get; private set; }

        public void OnCreate() {
            listRoleModels = new();
            var selfPlayer = new RoleModel() {
                RoleType = 1000,
            };
            listRoleModels.Add(selfPlayer);
        }

        public void OnDestroy() { }
    }
}