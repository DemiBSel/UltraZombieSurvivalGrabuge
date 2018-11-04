using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class basketTarget : NetworkBehaviour {

    private int value;
    [SyncVar]
    public int score;
    [SyncVar (hook="colorChange")]
    Color indicator;
	// Use this for initialization
	void Start () {
        score = 0;
        value = (int) transform.lossyScale.x;
        indicator = new Color(1, 1, 1, 1);
        GetComponent<Renderer>().material.color = indicator;
    }

    // Update is called once per frame
    void Update () {
        GetComponent<Renderer>().material.color = indicator;
	}

    private void OnTriggerEnter(Collider other)
    {
        CmdGetHit();
    }

    [Command]
    public void CmdGetHit()
    {
        score += value;
        int i = (int)Random.Range(0, 2);
        switch (i)
        {
            case 0:
                indicator.r -= 0.1f;
                break;
            case 1:
                indicator.g -= 0.1f;
                break;
            case 2:
                indicator.b -= 0.1f;
                break;
        }
        GetComponent<Renderer>().material.color = indicator;
        RpcSetColor(indicator);
    }

    public void colorChange(Color aColor)
    {
        indicator = aColor;
        GetComponent<Renderer>().material.color = aColor;
    }

    [ClientRpc]
    public void RpcSetColor(Color aColor)
    {
        indicator = aColor;
        GetComponent<Renderer>().material.color = aColor;
    }

}
