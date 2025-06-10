﻿using System;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Fantasy.Platform.Unity;
using GamePlay;
using UnityEngine;
using ZM.ZMAsset;
using FScene = Fantasy.Scene;

public class NetworkManager : Singleton<NetworkManager> {
    #region 属性和字段

    private FScene _fScene;
    private Session _session;

    public Action OnConnectSuccess;
    public Action OnConnectFailed;
    public Action OnDisconnect;

    #endregion

    #region public

    /// <summary>
    /// 初始化
    /// </summary>
    public async FTask Initlization() {
        await Entry.Initialize(typeof(Main).Assembly);
        _fScene = await Entry.CreateScene();
    }

    public Session Connect(
        string remateAddress,
        NetworkProtocolType networkProtocolType,
        Action onConnectSuccess = null,
        Action onConnectFailed = null,
        Action onDisconnect = null,
        int connectOutTime = 5000) {
        _session = _fScene.Connect(
            remateAddress, networkProtocolType,
            () => {
                OnComplete();
                onConnectSuccess?.Invoke();
            },
            () => {
                OnFailed();
                onConnectFailed?.Invoke();
            },
            () => {
                _OnDisconnect();
                onDisconnect?.Invoke();
            },
            false, connectOutTime);

        return _session;
    }

    public void Disconnect() {
        _session?.Dispose();
        _session = null;
    }

    public void OnRelease() {
        _session?.Dispose();
        _fScene?.Dispose();
        OnConnectSuccess = null;
        OnConnectFailed = null;
        OnDisconnect = null;
    }

    /// <summary>
    /// RPC 消息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="routeID"></param>
    /// <returns></returns>
    public async FTask<T> SendCallMessage<T>(IRequest request, long routeID = 0) where T : IResponse {
        var strSend = ProtoBuffConvert.ToJson(request);
        Debug.Log($"协议发送({typeof(T).Name}) {strSend}");
        var resp = (await _session.Call(request, routeID));
        var strRcv = ProtoBuffConvert.ToJson(resp);
        Debug.Log($"协接回包({typeof(T).Name}) {strRcv}");
        return (T)resp;
    }

    public void Send(IMessage message, uint rpcId = 0, long routeId = 0) {
        var strSend = ProtoBuffConvert.ToJson(message);
        Debug.Log($"协议发送({message.GetType().Name}) {strSend}");
        _session.Send(message, rpcId, routeId);
    }

    #endregion

    #region private

    private void _OnDisconnect() {
        Debug.LogError("NetworkManager:Disconnect");
        OnDisconnect?.Invoke();
    }

    private void OnFailed() {
        Debug.LogError("NetworkManager:Connect Failed");
        OnConnectFailed?.Invoke();
    }

    private void OnComplete() {
        Debug.Log("NetworkManager:Connect Success");
        OnConnectSuccess?.Invoke();
        _session.AddComponent<SessionHeartbeatComponent>().Start(5000); // 5000ms一次心跳
    }

    #endregion
}