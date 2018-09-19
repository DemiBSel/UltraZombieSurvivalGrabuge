using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConnectHUD : MonoBehaviour {

    CustomNetworkManager netMan;
    GameObject UI;
    Button startHostBut;
    Button joinHostBut;
    InputField hostAddField;
    InputField playerNameField;

	// Use this for initialization
	void Start () {

        netMan = GetComponent<CustomNetworkManager>();
        UI = GameObject.Find("WelcomeScreen");
        startHostBut = UI.transform.Find("StartHostButton").GetComponent<Button>();
        joinHostBut = UI.transform.Find("StartClientButton").GetComponent<Button>();

        hostAddField = UI.transform.Find("hostAddressField").GetComponent<InputField>();
        playerNameField = UI.transform.Find("PlayerNameField").GetComponent<InputField>();


        startHostBut.onClick.AddListener(startHost);
        joinHostBut.onClick.AddListener(joinHost);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void startHost()
    {
        if (playerNameField.text.Equals(""))
        {
            Debug.Log("No pseudal");
        }
        else
        {
            netMan.setPlayerName(playerNameField.text);
            netMan.StartHost();
            UI.SetActive(false);
        }
    }

    void joinHost()
    {
        if(playerNameField.text.Equals(""))
        {
            Debug.Log("No pseudal");
        }
        else
        {
            netMan.setPlayerName(playerNameField.text);
            var add = hostAddField.text;
            netMan.networkAddress = add;
            netMan.StartClient();
            UI.SetActive(false);
        }
    }
}
