using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Invector.vCharacterController;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    private vThirdPersonController controller;
    public enum NPCState
    {
        Stop,
        Move,
        Death,
        None
    }
    
    public NPCState state = NPCState.None;

    private void Awake()
    {
        controller = GetComponent<vThirdPersonController>();
        controller.isNPC = true;
    }

    void Start()
    {
        StartCoroutine(ControlNPC());
    }

    private int SetNPCState()
    {
        int random = Random.Range(0, 2);
        return random;
    }

    private float SetStateDuration()
    {
        float random = Random.Range(5, 10);
        return random;
    }

    private Vector3 SetDirection()
    {
        float randomX = Random.Range(-1.0f, 1.0f);
        float randomY = Random.Range(-1.0f, 1.0f);
        Vector3 direction = new Vector3(randomX, 0, randomY).normalized;
        return direction;
    }

    private IEnumerator ControlNPC()
    {
        WaitForSeconds duration = new WaitForSeconds(0);
        
        while (true)
        {
            this.state = (NPCState)SetNPCState();
            DOAnimation();
            duration = new WaitForSeconds(SetStateDuration());
            yield return duration;
        }
    }

    public void Death()
    {
        if (state == NPCState.Death)
            return;
        
        StopAllCoroutines();
        controller.isMoving = false;
        controller.input = (Vector3.zero);
        controller.Death();
        state = NPCState.Death;
    }

    private void DOAnimation()
    {
        switch (state)
        {
            case NPCState.Stop:
                controller.isMoving = false;
                controller.input = (Vector3.zero);
                break;
            case NPCState.Move:
                controller.isMoving = true;
                controller.input = SetDirection();
                break;
            case NPCState.Death:
                break;
        }
    }
    
}
