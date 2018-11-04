using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class basketRoom : NetworkBehaviour {

    GameObject [] targets;
    GameObject door;

	// Use this for initialization
	void Start () {
        List <GameObject> points = new List <GameObject>();
        door = transform.Find("Porte").gameObject;
        int i = 0;
        while(i<transform.childCount)
        {
            if(transform.GetChild(i).name.Contains("Target"))
            {
                points.Add(transform.GetChild(i).gameObject);
            }
            i++;
        }
        targets = points.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
        int i = 0;
        int currentScore = 0;
        while(i< targets.Length)
        {
            basketTarget cur = targets[i].GetComponent<basketTarget>();
            currentScore += cur.score;
            i++;
        }
        if(currentScore>100)
        {
            CmdFinish();
            disappear();
        }
	}
    [Command]
    private void CmdFinish()
    {
        RpcFinish();
    }

    [ClientRpc]
    private void RpcFinish()
    {
        door.SetActive(false);
    }

    public void disappear()
    {
        int i = 0;
        int currentScore = 0;
        float alpha = targets[0].GetComponent<basketTarget>().indicator.a;
        if(alpha >0)
        {
            while (i < targets.Length)
            {
                basketTarget cur = targets[i].GetComponent<basketTarget>();
                cur.indicator.a = cur.indicator.a - 0.002f;
                cur.CmdPaint(cur.indicator);
                i++;
            }
        }
        else
        {
            while (i < targets.Length)
            {
                targets[i].SetActive(false);
                i++;
            }
        }

    }
}
