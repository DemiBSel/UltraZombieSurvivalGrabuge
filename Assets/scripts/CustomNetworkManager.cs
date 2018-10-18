using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkManager : NetworkManager {

    private string playerName;

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        InitNameMessage allez = new InitNameMessage();
        allez.txt_message = playerName;
        ClientScene.AddPlayer(conn,0,allez);
    }

    public void setPlayerName(string name)
    {
        playerName = name;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId,NetworkReader yo)
    {
        /*
        string rec = yo.ReadMessage<StringMessage>().value;
        Debug.Log("On a reçu :"+ rec);

        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerController>().name = rec;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        */

        base.OnServerAddPlayer(conn, playerControllerId);
        if (conn.playerControllers.Count > 0)
        {
            GameObject player = conn.playerControllers[0].gameObject;
            string rec = yo.ReadMessage<StringMessage>().value;
            player.GetComponent<PlayerController>().SetName(rec);
        }
    }
}
