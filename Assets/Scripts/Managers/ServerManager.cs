using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ServerManager : NetworkBehaviour
{
    public static ServerManager Instance;
    
    public GameObject playerPrefab;
    public Dictionary<ulong, Character> characters = new Dictionary<ulong, Character>();

    public NetworkVariable<ulong> winnerID = new NetworkVariable<ulong>();
    public bool isWinner = false;

    public Transform spawnZones;
    public Character myCharacter;

    private RandomGenerator randomGenerator;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        randomGenerator = new RandomGenerator(spawnZones.childCount);
        SetCharactersServerRpc();
    }

    public void StartTimer()
    {
        if(IsServer)
            TimerManager.Instance.StartTimer();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SetCharactersServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Transform ch = Instantiate(playerPrefab, spawnZones.GetChild(randomGenerator.GenerateRandomNum()).position, Quaternion.identity).transform;
        
        Debug.Log(ch.position);
        
        //ch.position = Vector3.zero;
        ch.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId, true);
        ch.GetComponent<Character>().ownerId = serverRpcParams.Receive.SenderClientId;
        
        Character[] tempCharacters = GameObject.FindObjectsByType<Character>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Character character in tempCharacters)
        {
            this.characters[character.GetComponent<NetworkObject>().OwnerClientId] = character;
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void KillCharacterServerRpc(ulong deathID, ServerRpcParams serverRpcParams = default)
    {
        //ulong clientIDUlong = serverRpcParams.Receive.SenderClientId;
        
        this.characters[deathID].Dead();
        
        CheckFinish();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void CloakServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong cloakClientID = serverRpcParams.Receive.SenderClientId;
        CloakClientRpc(cloakClientID);
    }



    [ClientRpc]
    public void CloakClientRpc(ulong cloakID)
    {
        Character[] tempCharacters = GameObject.FindObjectsByType<Character>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Character character in tempCharacters)
        {
            if (character.OwnerClientId == cloakID)
            {
                RoleManager.Instance.Cloak(character);
            }
        }
    }

    private void CheckFinish()
    {
        ulong winnerID = 0;
        int deadCount = 0;

        foreach (KeyValuePair<ulong, Character> pair in characters)
        {
            if (pair.Value.isDead)
            {
                deadCount++;
            }
            else
            {
                winnerID = pair.Key;
            }
        }

        if (deadCount == characters.Count - 1)
        {
            FinishGame(winnerID);
            Debug.Log("Finish");
        }
    }

    public void CheckFinishByScore()
    {
        ulong winnerID = 0;
        int maxScore = 0;
        
        foreach (KeyValuePair<ulong, Character> pair in characters)
        {
            if (pair.Value.score.Value > maxScore)
            {
                maxScore = pair.Value.score.Value;
                winnerID = pair.Key;
            }
        }
        
        FinishGame(winnerID);
    }
    
    public void FinishGame(ulong winnerID)
    {
        List<ulong> loserIDs = characters.Keys.ToList();
        loserIDs.Remove(winnerID);

        this.winnerID.Value = winnerID;
        
        LoseClientRpc(new ClientRpcParams()
        {
            Send = new ClientRpcSendParams { TargetClientIds = loserIDs }
        });
        
        WinClientRpc(new ClientRpcParams()
        {
            Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { winnerID } }
        });
    }

    [ClientRpc]
    public void LoseClientRpc(ClientRpcParams param)
    {
        Debug.Log("Loss");
        isWinner = false;

        Managers.UI.ShowPopupUI<UI_Finish>();
    }

    [ClientRpc]
    public void WinClientRpc(ClientRpcParams param)
    {
        Debug.Log("Win");
        isWinner = true;
        
        Managers.UI.ShowPopupUI<UI_Finish>();
    }
    
    /*SetMyCharacterClientRpc(clientIDInt, new ClientRpcParams
    {
        Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { clientIDUlong } }
    });*/
}
