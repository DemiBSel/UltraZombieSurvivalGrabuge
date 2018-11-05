using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class victoryScript : MonoBehaviour {

    public GameObject panel;
    public GameObject textZone;
    public bool p1;

    // Use this for initialization
    void Start () {
        p1 = false;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && p1)
        {
            win();
        }


        if (other.gameObject.tag == "Player")
        {
            p1 = true;
        }


    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            p1 = false;
        }

    }

    public void win()
    {
        panel.SetActive(true);

        int max = 0;
        foreach (GameObject curPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerController pc = curPlayer.GetComponent<PlayerController>();
            if(pc.score>max)
            {
                max = pc.score;
            }
        }


        textZone.GetComponent<Text>().text = "" + max;
    }

}
