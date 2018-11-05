using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reset_script : MonoBehaviour {

    public GameObject panel;
    public GameObject textZone;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            lose();

        }

    }

    public void lose()
    {
        panel.SetActive(true);

        int max = 0;
        foreach (GameObject curPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerController pc = curPlayer.GetComponent<PlayerController>();
            if (pc.score > max)
            {
                max = pc.score;
            }
        }


        textZone.GetComponent<Text>().text = "" + max;
    }
}
