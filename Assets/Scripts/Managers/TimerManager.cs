using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TimerManager : NetworkBehaviour
{
    public static TimerManager Instance;

    private const int MAX_TIMER = 1000; 
    private NetworkVariable<int> timer = new NetworkVariable<int>(MAX_TIMER);

    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        timer.OnValueChanged += OnTimerChanged;
    }

    public void StartTimer()
    {
        StartCoroutine(CoTimer());
    }

    private IEnumerator CoTimer()
    {
        int count = timer.Value;
        WaitForSeconds oneSecond = new WaitForSeconds(1);

        while (timer.Value != 0)
        {
            timer.Value -= 1;
            yield return oneSecond;
        }
        
        ServerManager.Instance.CheckFinishByScore();
    }

    public void OnTimerChanged(int prev, int curr)
    {
        if(Managers.UI.FindPopup<UI_Score>())
            Managers.UI.FindPopup<UI_Score>().OnTimer(curr);
    }
}
