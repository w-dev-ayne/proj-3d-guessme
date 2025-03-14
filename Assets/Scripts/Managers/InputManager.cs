using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool isInfoTabOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(InputDefine.TabInfoKey))
        {
            Managers.UI.ShowPopupUI<UI_InfoTab>();
        }

        if (Input.GetKeyUp(InputDefine.TabInfoKey))
        {
            Managers.UI.FindPopup<UI_InfoTab>().ClosePopupUI();
        }
    }
}
