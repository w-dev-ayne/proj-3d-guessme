using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Room : UI_Popup
{
    enum Objects
    {
        PlayersObject,
        JoinCodeObject
    }

    enum Buttons
    {
        StartButton
    }
    
    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        if (!RoomManager.Instance.IsServer)
        {
            GetButton((int)Buttons.StartButton).interactable = false;
        }
        else
        {
            GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
        }

        GetObject((int)Objects.JoinCodeObject).GetComponent<TextMeshProUGUI>().text = RoomManager.Instance.joinCode;
        
        UpdatePlayers();
        
        if (base.Init() == false)
            return false;
        return true;
    }

    private void OnClickStartButton()
    {
        if (GetButton((int)Buttons.StartButton).interactable == false)
            return;
        
        RelayManager.Instance.ChangeScene();
        GetButton((int)Buttons.StartButton).interactable = false;
    }

    public void UpdatePlayers()
    {
        Transform parent = GetObject((int)Objects.PlayersObject).transform;

        int count = 0;
        
        foreach (KeyValuePair<ulong, string> pair in RoomManager.Instance.players)
        {
            parent.GetChild(count).Find("NumberBack").GetChild(0).GetComponent<TextMeshProUGUI>().text = pair.Key.ToString();
            parent.GetChild(count).Find("PlayerBack").GetChild(0).GetComponent<TextMeshProUGUI>().text = pair.Value.ToString();
            count++;
        }
    }
}
