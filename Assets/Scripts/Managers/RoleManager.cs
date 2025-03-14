using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RoleManager : MonoBehaviour
{
    public static RoleManager Instance;
    
    
    [Header("Ray")]
    public Material transparentMat;
    
    [Header("Cloaker")]
    public const int MAX_CLOAK_COUNT = 5;
    public Material originMat;
    public Material cloakMat;
    
    [Header("Observer")]
    public Material observerMat;

    [Header("Mirror")] 
    public GameObject mirrorObj;
    public RenderTexture mirrorTexture;
    
    public UnityEvent skill;

    public void SetRole(Character character, Define.Role role)
    {
        skill.RemoveAllListeners();
        
        mirrorObj.SetActive(false);
        
        switch (role)
        {
            case Define.Role.Cloaker:
                Managers.UI.ShowPopupUI<UI_Cloak>();
                skill.AddListener(() =>
                {
                    if (character.cloakCount == 0)
                        return;
                    
                    if (!character.isCloaked) // 클로킹 진입
                    {
                        StartCoroutine(CoCloakTimer());
                        character.cloakCount--;
                        Managers.UI.FindPopup<UI_Cloak>().SetCountTMP(character.cloakCount);
                    }
                    else // 클로킹 해제
                    {
                        Managers.UI.FindPopup<UI_Cloak>().SetTimerTMP(5);
                        StopAllCoroutines();
                    }
                    ServerManager.Instance.CloakServerRpc();
                });
                break;
            case Define.Role.Observer:
                StartCoroutine(ApplyMaterial());
                break;
            case Define.Role.Ray:
                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                foreach (GameObject building in buildings)
                {
                    building.GetComponent<Renderer>().material = transparentMat;
                }
                break;
            case Define.Role.Defender:
                break;
            case Define.Role.Mirror:
                mirrorObj.SetActive(true);
                character.camera.targetTexture = this.mirrorTexture;
                character.camera.gameObject.SetActive(true);
                skill.AddListener(() =>
                {
                    mirrorObj.SetActive(!mirrorObj.activeInHierarchy);
                });
                break;
            default:
                break;
        }
    }

    private IEnumerator ApplyMaterial()
    {
        yield return new WaitForSeconds(3.0f);
        
        Character[] ccs = GameObject.FindObjectsByType<Character>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        Debug.Log($"CC Num : {ccs.Length}");
        
        for (int i = 0; i < ccs.Length - 1;)
        {
            int randomNum = Random.Range(0, ccs.Length);
            if(ccs[randomNum].IsOwner)
                continue;
            else
            {
                ccs[randomNum].renderers[0].material = observerMat;
                i++;
            }
        }
    }
    
    private IEnumerator CoCloakTimer()
    {
        int timer = 5;
        WaitForSeconds oneSecond = new WaitForSeconds(1);

        while (timer != 0)
        {
            Managers.UI.FindPopup<UI_Cloak>().SetTimerTMP(timer);
            yield return oneSecond;
            timer--;
        }

        if (ServerManager.Instance.myCharacter.isCloaked) // 클로킹 해제
        {
            ServerManager.Instance.CloakServerRpc();
            Managers.UI.FindPopup<UI_Cloak>().SetTimerTMP(5);
        }
        
                
    }

    public void Cloak(Character character)
    {
        if (!character.isCloaked)
        {
            foreach (Renderer renderer in character.renderers)
            {
                renderer.material = cloakMat;
            }

            character.isCloaked = true;
        }
        else
        {
            foreach (Renderer renderer in character.renderers)
            {
                renderer.material = originMat;
            }
                        
            character.isCloaked = false;
        }
    }

    void Awake()
    {
        Instance = this;
    }
}
