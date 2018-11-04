using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset_script : MonoBehaviour {

    public GameObject panel;


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
           panel.SetActive(true);

        }

    }

    public void respawn()
    {
        //var health = other.GetComponent<Health>();
        //health.TakeDamage(1000);
    }
}
