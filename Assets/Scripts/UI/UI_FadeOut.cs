using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeOut : UI_Popup
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        return true;
    }

    public void OnFadeOut()
    {
        ServerManager.Instance.StartTimer();
        this.gameObject.SetActive(false);
    }
}
