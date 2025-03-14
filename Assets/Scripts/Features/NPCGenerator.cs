using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

public class NPCGenerator : NetworkBehaviour
{
    [SerializeField] 
    private int npcNumber = 0;

    [SerializeField]
    private GameObject npcPrefab;

    public override void OnNetworkSpawn()
    {
        /*if(IsServer)
            GenerateNPC();*/
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(IsServer)
            GenerateNPC();
    }

    private void GenerateNPC()
    {
        Debug.Log("GenerateNPC");
        for (int i = 0; i < npcNumber; i++)
        {
            Transform npc = Instantiate(npcPrefab).transform;
            npc.position = RandomPosition();
            npc.GetComponent<NetworkObject>().Spawn(true);
        }
    }

    private Vector3 RandomPosition()
    {
        int randomX = Random.Range(-25, 25);
        int randomZ = Random.Range(-25, 25);

        return new Vector3(randomX, 0.5f, randomZ);
    }
}
