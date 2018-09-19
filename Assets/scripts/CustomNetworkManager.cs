using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkManager : NetworkManager {

    private string playerName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        byte[] message = Encoding.ASCII.GetBytes(playerName);
        //AddPlayerMessage nameMessage = new AddPlayerMessage();
        InitNameMessage allez = new InitNameMessage();
        allez.txt_message = playerName;

        ClientScene.AddPlayer(conn,0,allez);
        //nameMessage.msgData = message;
        //nameMessage.msgSize = message.Length;
        Debug.Log("should have sent message");
    }

    public void setPlayerName(string name)
    {
        playerName = name;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId,NetworkReader yo)
    {
        Debug.Log("On a reçu :"+yo.ReadMessage<StringMessage>().value);

        base.OnServerAddPlayer(conn, playerControllerId);
    }
}
