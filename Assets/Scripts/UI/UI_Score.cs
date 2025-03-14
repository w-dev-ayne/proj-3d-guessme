using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Score : UI_Popup
{
    private TextMeshProUGUI scoreTmp;
    private TextMeshProUGUI timerTmp;
    enum Objects
    {
        ScoreObject,
        TimerObject
    }
    
    public override bool Init()
    {
        BindObject(typeof(Objects));

        scoreTmp = GetObject((int)Objects.ScoreObject).GetComponent<TextMeshProUGUI>();
        timerTmp = GetObject((int)Objects.TimerObject).GetComponent<TextMeshProUGUI>();
        
        if (base.Init() == false)
            return false;
        return true;
    }

    public void OnScore(int score)
    {
        scoreTmp.text = $"Score : {score}";
    }

    public void OnTimer(int timer)
    {
        timerTmp.text = timer.ToString();
    }
    
}
