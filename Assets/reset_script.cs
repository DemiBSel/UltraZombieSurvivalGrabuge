using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset_script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "reset")
        {
            Destroy(other.gameObject);
        }

    }
}
