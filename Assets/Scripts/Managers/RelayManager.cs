using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Http;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance;
    
    const int m_MaxConnections = 4;

    public static string RelayJoinCode = String.Empty;

    void Awake()
    {
        Instance = this;
        
        Example_AuthenticatingAPlayer();
    }

    void Start()
    {
        //RNetworkManager.Singleton.OnClientConnectedCallback += ChangeScene;
    }

    public void OnClickServerButton(UnityAction onServerConfigured = null)
    {
        StartCoroutine(Example_ConfigureTransportAndStartNgoAsHost(onServerConfigured));
    }

    public void OnClickClientButton(string joinCode)
    {
        StartCoroutine(Example_ConfigureTransportAndStartNgoAsConnectingPlayer(joinCode));
    }
    
    async void Example_AuthenticatingAPlayer()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            var playerID = AuthenticationService.Instance.PlayerId;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    
    public static async Task<RelayServerData> AllocateRelayServerAndGetJoinCode(int maxConnections,  UnityAction onServerConfigured = null, string region = null)
    {
        Allocation allocation;
        //string createJoinCode;
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay create allocation request failed {e.Message}");
            throw;
        }

        Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"server: {allocation.AllocationId}");

        try
        {
            RelayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RoomManager.Instance.joinCode = RelayJoinCode;
            //Managers.UI.FindPopup<UI_Relay>().SetJoinCode(RelayJoinCode);
            Debug.Log(RelayJoinCode);
            onServerConfigured?.Invoke();
        }
        catch
        {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        return new RelayServerData(allocation, "dtls");
    }
    
    IEnumerator Example_ConfigureTransportAndStartNgoAsHost(UnityAction onServerConfigured = null)
    {
        var serverRelayUtilityTask = AllocateRelayServerAndGetJoinCode(m_MaxConnections, onServerConfigured);
        while (!serverRelayUtilityTask.IsCompleted)
        {
            yield return null;
        }
        if (serverRelayUtilityTask.IsFaulted)
        {
            Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
            yield break;
        }

        var relayServerData = serverRelayUtilityTask.Result;

        // Display the joinCode to the user.

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartHost();
        yield return null;
    }
    
    public static async Task<RelayServerData> JoinRelayServerFromJoinCode(string joinCode)
    {
        JoinAllocation allocation;
        try
        {
            allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch
        {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
        Debug.Log($"client: {allocation.AllocationId}");

        return new RelayServerData(allocation, "dtls");
    }
    
    IEnumerator Example_ConfigureTransportAndStartNgoAsConnectingPlayer(string joinCode)
    {
        // Populate RelayJoinCode beforehand through the UI
        var clientRelayUtilityTask = JoinRelayServerFromJoinCode(joinCode);

        while (!clientRelayUtilityTask.IsCompleted)
        {
            yield return null;
        }

        if (clientRelayUtilityTask.IsFaulted)
        {
            Debug.LogError("Exception thrown when attempting to connect to Relay Server. Exception: " + clientRelayUtilityTask.Exception.Message);
            yield break;
        }

        var relayServerData = clientRelayUtilityTask.Result;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
        yield return null;
    }

    public void ChangeScene()
    {
        /*if (!NetworkManager.Singleton.IsServer)
            return;
        if (NetworkManager.Singleton.ConnectedClients.Count != 2)
            return;*/
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
