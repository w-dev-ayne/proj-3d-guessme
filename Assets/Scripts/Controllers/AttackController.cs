using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using Unity.Netcode;
using UnityEngine;

public class AttackController : NetworkBehaviour
{
    public bool isAttacking = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking)
            return;
        
        if (other.TryGetComponent<NPCController>(out NPCController npcController))
        {
            if (npcController.state == NPCController.NPCState.Death)
                return;
            
            npcController.Death();
            this.GetComponentInParent<Character>().GetScore(Define.KILL_SCORE); 
        }
        else if (other.TryGetComponent<vThirdPersonController>(out vThirdPersonController controller))
        {
            if (controller.GetComponent<Character>().isDefending.Value)
                return;
            controller.Death();
        }
    }
}
