using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Status : UI_Popup
{
    enum Objects
    {
        ScoreObject
    }

    enum Buttons
    {
        
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        
        if (base.Init() == false)
            return false;
        return true;
    }
}
