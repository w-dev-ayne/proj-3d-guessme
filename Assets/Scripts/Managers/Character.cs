using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Character : NetworkBehaviour
{
    public bool isDead = false;
    public ulong ownerId;
    public Define.Role role = Define.Role.Cloaker;

    public NetworkVariable<int> score = new NetworkVariable<int>(readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner);

    [DrawIf("role", Define.Role.Cloaker)]
    public bool isCloaked = false;
    public int cloakCount = RoleManager.MAX_CLOAK_COUNT;
    public Renderer[] renderers;

    [DrawIf("role", Define.Role.Defender)] 
    public NetworkVariable<bool> isDefending =
        new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    public Camera camera;
    
    void Start()
    {
        if (IsOwner)
        {
            ServerManager.Instance.myCharacter = this;
            int randomNum = Random.Range(0, (int)Define.Role.Max);
            this.role = (Define.Role)randomNum;
            Debug.Log($"Random Number : {randomNum}");

            //role = Define.Role.Observer;
            
            RoleManager.Instance.SetRole(this, this.role);
            Managers.UI.ShowPopupUI<UI_Role>();
        }
    }
    
    public void GetScore(int amount)
    {
        this.score.Value += amount;
        Managers.UI.FindPopup<UI_Score>().OnScore(score.Value);
    }

    public void Dead()
    {
        isDead = true;
    }
}