using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class basketTarget : NetworkBehaviour {

    private int value;
    [SyncVar]
    public int score;
    [SyncVar (hook="colorChange")]
    public Color indicator;
    [SyncVar]
    Vector3 initialPos;
    float inc = 1.0f;
    // Use this for initialization
    void Start () {
        score = 0;
        value = (int) transform.lossyScale.x;
        indicator = new Color(1, 1, 1, 1);
        GetComponent<Renderer>().material.color = indicator;
        initialPos = transform.position;

    }

    // Update is called once per frame
    void Update () {
        GetComponent<Renderer>().material.color = indicator;
        if(transform.position != initialPos) {
            float step = inc * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, initialPos, step);
            inc += 0.1f;
        }
        else
        {
            inc = 1.0f;
        }
       

    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("object"))
        { 
            CmdGetHit();
        }*/

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
        //float step = 100 * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, initialPos, step);
    }

}
