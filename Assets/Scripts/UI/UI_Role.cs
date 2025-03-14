using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Role : UI_Popup
{
    enum Objects
    {
        tmpObject,
        tmp2Object
    }

    enum Buttons
    {
        OKButton
    }
    
    
    public override bool Init()
    {
        FindObjectOfType<vThirdPersonCamera>().lockCamera = true;
        
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        Initialize();
        
        GetButton((int)Buttons.OKButton).gameObject.BindEvent(OnClickOKButton);
        
        //Invoke("ClosePopupUI", 2.0f);
        
        if (base.Init() == false)
            return false;
        return true;
    }

    private void Initialize()
    {
        string info = String.Empty;
        switch (ServerManager.Instance.myCharacter.role)
        {
            case Define.Role.Cloaker:
                info = RoleDefine.ClOAKER_INFO;
                break;
            case Define.Role.Observer:
                info = RoleDefine.OBSERVER_INFO;
                break;
            case Define.Role.Ray:
                info = RoleDefine.RAY_INFO;
                break;
            case Define.Role.Defender:
                info = RoleDefine.DEFENDER_INFO;
                break;
            case Define.Role.Mirror:
                info = RoleDefine.MIRROR_INFO;
                break;
        }

        GetObject((int)Objects.tmpObject).GetComponent<TextMeshProUGUI>().text = info;
        GetObject((int)Objects.tmp2Object).GetComponent<TextMeshProUGUI>().text = "특수 능력은 R키를 눌러 사용할 수 있습니다.";
    }

    private void OnClickOKButton()
    {
        FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
        ClosePopupUI();
    }
}
