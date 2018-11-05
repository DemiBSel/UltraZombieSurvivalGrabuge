using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victoryScript : MonoBehaviour {

    public GameObject panel;
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
            panel.SetActive(true);
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

}
