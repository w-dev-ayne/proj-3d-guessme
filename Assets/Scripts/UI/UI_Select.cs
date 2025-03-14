using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Select : UI_Popup
{
    enum Buttons
    {
        MakeRoomButton,
        JoinRoomButton
    }
    public override bool Init()
    {
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.MakeRoomButton).gameObject.BindEvent(OnClickMakeRoomButton);
        GetButton((int)Buttons.JoinRoomButton).gameObject.BindEvent(OnClickJoinRoomButton);
        
        if (base.Init() == false)
            return false;
        return true;
    }

    private void OnClickMakeRoomButton()
    {
        Managers.UI.ShowPopupUI<UI_MakeRoom>();
        ClosePopupUI();
    }

    private void OnClickJoinRoomButton()
    {
        Managers.UI.ShowPopupUI<UI_JoinRoom>();
        ClosePopupUI();
    }
}
