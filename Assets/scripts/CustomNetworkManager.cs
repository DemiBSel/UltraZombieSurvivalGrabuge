using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkManager : NetworkManager {

    private string playerName;
    private Color color;
    public GameObject grabugeurObj;
    private GrabugeManager grabugeur;

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        InitNameMessage allez = new InitNameMessage();

        allez.txt_message = playerName;
        allez.txt_message += ";" + color.r+";"+color.g + ";" +color.b;
        Debug.Log("got color" + color.ToString());
        ClientScene.AddPlayer(conn,0,allez);
    }

    public void setPlayerName(string name)
    {
        playerName = name;
    }

    public void setPlayerColor(Color aColor)
    {
        Debug.Log("setplayercolor " + aColor.ToString());
        this.color = aColor;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId,NetworkReader yo)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        if (conn.playerControllers.Count > 0)
        {
            GameObject player = conn.playerControllers[0].gameObject;
            string [] rec = yo.ReadMessage<StringMessage>().value.Split(';');
            player.GetComponent<PlayerController>().SetName(rec[0]);
            player.GetComponent<PlayerController>().setColor(new Color(float.Parse(rec[1]), float.Parse(rec[2]), float.Parse(rec[3]), 1));
            grabugeur.AddPlayer(player.GetComponent<PlayerController>());
        }
    }

    public GrabugeManager getGrabugeur()
    {
        return this.grabugeur;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        grabugeur = grabugeurObj.GetComponent<GrabugeManager>();
    }

}
