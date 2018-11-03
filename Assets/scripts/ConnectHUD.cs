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
    Button quitBut;
    InputField hostAddField;
    InputField playerNameField;
    GameObject menu;
    Color pickedColor;
    ColorPickController colorCtrl;
    public GameObject colorPanel;

	// Use this for initialization
	void Start () {

        netMan = GetComponent<CustomNetworkManager>();
        colorCtrl = colorPanel.GetComponent<ColorPickController>();

        UI = GameObject.Find("WelcomeScreen");
        menu = GameObject.Find("MenuScreen");

        startHostBut = UI.transform.Find("StartHostButton").GetComponent<Button>();
        joinHostBut = UI.transform.Find("StartClientButton").GetComponent<Button>();

        hostAddField = UI.transform.Find("hostAddressField").GetComponent<InputField>();
        playerNameField = UI.transform.Find("PlayerNameField").GetComponent<InputField>();

        quitBut = menu.transform.Find("QuitButton").GetComponent<Button>();
        menu.SetActive(false);

        startHostBut.onClick.AddListener(startHost);
        joinHostBut.onClick.AddListener(joinHost);
        quitBut.onClick.AddListener(quitGame);
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
            netMan.setPlayerColor(colorCtrl.pickedColor);
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
            netMan.setPlayerColor(colorCtrl.pickedColor);
            var add = hostAddField.text;
            netMan.networkAddress = add;
            netMan.StartClient();
            UI.SetActive(false);
        }
    }

    void quitGame()
    {
        netMan.StopHost();
        menu.SetActive(false);
        UI.SetActive(true);
    }

    public GameObject getQuitHud()
    {
        return this.menu ;
    }
}
