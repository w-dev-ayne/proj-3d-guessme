using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Finish : UI_Popup
{
    enum Objects
    {
        TmpObject
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        
        Initialize();

        if (base.Init() == false)
            return false;
        return true;
    }

    private void Initialize()
    {
        string Message = ServerManager.Instance.isWinner
            ? $"You Win"
            : $"You Lose\nPlayer {ServerManager.Instance.winnerID.Value} Win";

        GetObject((int)Objects.TmpObject).GetComponent<TextMeshProUGUI>().text = Message;
    }
}
