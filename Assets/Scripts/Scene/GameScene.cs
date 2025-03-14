using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.Game;
        Debug.Log($"{SceneType} Init");

        Managers.UI.ShowPopupUI<UI_Score>();

        return true;
    }
}
