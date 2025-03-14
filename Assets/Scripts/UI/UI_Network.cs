using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_Network : MonoBehaviour
{
    public Button hostButton;

    public Button clientButton;
    
    // Start is called before the first frame update
    void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.StartServer();
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
