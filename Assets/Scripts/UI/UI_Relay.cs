using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Relay : UI_Popup
{
    private string joinCode = String.Empty;

    private bool serverEnabled = true;
    private bool clientEnabled = true;
    
    private TextMeshProUGUI joinCodeTmp;
    private TMP_InputField joinCodeInput;
    
    enum Objects
    {
        JoinCodeObject,
        JoinCodeInputObject
    }
    
    enum Buttons
    {
        ServerButton,
        ClientButton,
    }
    
    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.ServerButton).gameObject.BindEvent(OnClickServerButton);
        GetButton((int)Buttons.ClientButton).gameObject.BindEvent(OnClickClientButton);

        joinCodeTmp = GetObject((int)Objects.JoinCodeObject).GetComponent<TextMeshProUGUI>();
        joinCodeInput = GetObject((int)Objects.JoinCodeInputObject).GetComponent<TMP_InputField>();
        joinCodeInput.onValueChanged.AddListener(OnJoinCodeInputValueChanged);
        
        if (base.Init() == false)
            return false;
        return true;
    }

    private void OnClickServerButton()
    {
        if (!serverEnabled)
            return;
        
        RelayManager.Instance.OnClickServerButton();

        serverEnabled = false;
        GetButton((int)Buttons.ServerButton).interactable = serverEnabled;
        clientEnabled = false;
        GetButton((int)Buttons.ClientButton).interactable = clientEnabled;
    }

    private void OnClickClientButton()
    {
        if (!clientEnabled)
            return;
        
        RelayManager.Instance.OnClickClientButton(this.joinCode);
    }

    public void SetJoinCode(string joinCode)
    {
        joinCodeTmp.text = joinCode;
    }

    private void OnJoinCodeInputValueChanged(string value)
    {
        serverEnabled = value == String.Empty;
        GetButton((int)Buttons.ServerButton).interactable = serverEnabled;
        
        joinCode = value;
    }
}
