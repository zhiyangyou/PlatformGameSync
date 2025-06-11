using System;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;

namespace WorldSpace.GameWorld {
    public class HeroLogicCtrl : ILogicBehaviour {
        public List<LogicActor_Player> _listLogicPlayers = null;

        private BattleDataMgr _battleData = null;

        public void OnCreate() {
            _battleData = WorldManager.GetWorld<GameWorld>().GetExitsDataMgr<BattleDataMgr>();
            _listLogicPlayers = new();
            InitHeros();
        }

        public void OnLogicFrameUpdate() {
            foreach (var logicPlayer in _listLogicPlayers) {
                try {
                    logicPlayer.OnLogicFrameUpdate();
                }
                catch (Exception e) {
                    Debug.LogError("执行LogicObject.OnLogicFrameUpdate时发生异常");
                    Debug.LogException(e);
                }
            }
        }

        public void OnDestroy() {
            foreach (var logicPlayer in _listLogicPlayers) {
                try {
                    logicPlayer.OnDestory();
                }
                catch (Exception e) {
                    Debug.LogError("执行LogicObject.OnDestory时发生异常");
                    Debug.LogException(e);
                }
            }
            _listLogicPlayers.Clear();
        }

        private void InitHeros() {
            foreach (var roleData in _battleData.listRoleModels) {
                int roleType = roleData.RoleType;
                var goHero = ZMAsset.Instantiate($"{AssetsPathConfig.Roles}Player_{roleType}.prefab", null);
                var heroRender = goHero.GetOrAddComponnet<RenderObject_Player>();
                goHero.name = $"lockstep_player_{roleType}";
                LogicActor_Player heroLogic = new();
                heroLogic.SetIsLocalPlayer(roleData.IsLocalPlayer);
                heroLogic.BindRenderObject(heroRender);
                heroLogic.OnCreate();
                heroRender.OnCreate();
                _listLogicPlayers.Add(heroLogic);
            }
        }
    }
}