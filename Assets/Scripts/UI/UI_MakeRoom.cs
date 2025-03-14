using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class UI_MakeRoom : UI_Popup
{
    private TMP_InputField nickNameInput;
    
    enum Objects
    {
        NickNameInputObject
    }

    enum Buttons
    {
        OKButton,
        BackButton
    }
    
    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.OKButton).gameObject.BindEvent(OnClickOKButton);
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        
        nickNameInput = GetObject((int)Objects.NickNameInputObject).GetComponent<TMP_InputField>();
        
        if (base.Init() == false)
            return false;
        return true;
    }

    private void OnClickOKButton()
    {
        if (GetButton((int)Buttons.OKButton).interactable == false)
            return;
        
        RoomManager.Instance.onNetworkSpawn.AddListener(() =>
        {
            RoomManager.Instance.RegisterNickNameServerRpc(nickNameInput.text);
            Managers.UI.ShowPopupUI<UI_Room>();
            ClosePopupUI();
        });
        RelayManager.Instance.OnClickServerButton();
        GetButton((int)Buttons.OKButton).interactable = false;
    }

    private void OnClickBackButton()
    {
        
        Managers.UI.ShowPopupUI<UI_Select>();
        ClosePopupUI();
    }
}
