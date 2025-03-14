using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class RoomManager : NetworkBehaviour
{
    public static RoomManager Instance;

    public Dictionary<ulong, string> players = new Dictionary<ulong, string>();
    public List<string> playersInspector = new List<string>();
    
    public string joinCode = String.Empty;

    public GameObject playerInfoPrefab;

    public UnityEvent onNetworkSpawn;
    
    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        onNetworkSpawn?.Invoke();
        Debug.Log("After Initialize");
    }

    [ServerRpc(RequireOwnership = false)]
    public void RegisterNickNameServerRpc(string nickName, ServerRpcParams serverRpcParams = default)
    {
        players[serverRpcParams.Receive.SenderClientId] = nickName;
        
        Debug.Log($"{serverRpcParams.Receive.SenderClientId} is registered. Nickname : {nickName}");
        
        if(Managers.UI.FindPopup<UI_Room>() != null)
            Managers.UI.FindPopup<UI_Room>().UpdatePlayers();

        playersInspector.Add($"{serverRpcParams.Receive.SenderClientId}:{nickName}");

        foreach (KeyValuePair<ulong, string> pair in players)
        {
            Debug.Log("Send to clients");
            RegisterNickNameClientRpc(pair.Value, pair.Key);
        }
    }
    
    [ClientRpc]
    public void RegisterNickNameClientRpc(string nickName, ulong id)
    {
        if (IsServer) // Collection 중도 수정 방지
            return;
        
        players[id] = nickName;
        
        Debug.Log($"{id} is registered. Nickname : {nickName}");
        
        if(Managers.UI.FindPopup<UI_Room>() != null)
            Managers.UI.FindPopup<UI_Room>().UpdatePlayers();

        playersInspector.Add($"{id}:{nickName}");
    }
}
