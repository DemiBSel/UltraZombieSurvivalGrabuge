using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrabugeManager : NetworkBehaviour {

    List<PlayerController> players; 


	// Use this for initialization
	void Start () {
        players = new List<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void AddPlayer(PlayerController pl)
    {
        players.Add(pl);
    }

    public void respawnAll()
    {
        PlayerController[] lPl = players.ToArray();
        int i = 0;
        while(i< lPl.Length)
        {
            lPl[i].gameObject.GetComponent<Health>().RpcRespawn();
            i++;
        }
        RpcPanel();
    }


 

    [ClientRpc]
    public void RpcPanel()
    {
        if (GameObject.Find("EndPanel").active)
        {
            GameObject.Find("EndPanel").SetActive(false);
        }
        if (GameObject.Find("VictoryPanel").active)
        {
            GameObject.Find("VictoryPanel").SetActive(false);
        }
    }
}
