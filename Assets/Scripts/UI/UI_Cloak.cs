using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Cloak : UI_Popup
{
    private TextMeshProUGUI timerTmp;
    private TextMeshProUGUI countTmp;
    
    enum Objects
    {
        TimerObject,
        CountObject
    }
    
    public override bool Init()
    {
        BindObject(typeof(Objects));

        timerTmp = GetObject((int)Objects.TimerObject).GetComponent<TextMeshProUGUI>();
        countTmp = GetObject((int)Objects.CountObject).GetComponent<TextMeshProUGUI>();

        countTmp.text = $"남은 횟수 : {RoleManager.MAX_CLOAK_COUNT.ToString()}";
        
        if (base.Init() == false)
            return false;
        return true;
    }

    public void SetTimerTMP(int timer)
    {
        timerTmp.text = $"남은 시간 : {timer.ToString()}";
    }

    public void SetCountTMP(int count)
    {
        countTmp.text = $"남은 횟수 : {count.ToString()}";
    }
}
