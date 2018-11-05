using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Networked3DObject : NetworkBehaviour
{

    [SyncVar(hook = "colorChange")]
    public Color indicator;
    // Use this for initialization
    void Start () {
        indicator = new Color(1, 1, 1, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        CmdGetHit();

    }

    [Command]
    public void CmdGetHit()
    {
        int i = (int)Random.Range(0, 3);
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

    [Command]
    public void CmdPaint(Color color)
    {
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
