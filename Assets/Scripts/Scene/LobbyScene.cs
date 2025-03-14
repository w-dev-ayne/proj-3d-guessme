using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.Lobby;
        Debug.Log($"{SceneType} Init");

        Managers.UI.ShowPopupUI<UI_Select>();
        //Managers.UI.ShowPopupUI<UI_Relay>();

        return true;
    }
}
