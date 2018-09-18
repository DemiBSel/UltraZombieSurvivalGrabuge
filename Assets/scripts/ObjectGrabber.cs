﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour {

    public FixedJoint joint;
    public GameObject inside ;
    private bool holding;
	// Use this for initialization
	void Start () {
        joint = this.gameObject.GetComponent<FixedJoint>();
        holding = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(inside != null && Input.GetButton("Fire1"))
        {
            if(!holding)
            {
                joint.connectedBody = inside.GetComponent<Rigidbody>();
                joint.connectedBody.isKinematic = true;
                holding = true;
            }
            else
            {
                joint.connectedBody.isKinematic = false;
                joint.connectedBody = null;
                holding = false;
            }

        }

	}

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered " + other.name);
        if(other.gameObject!=this.gameObject)
            inside = other.gameObject;

    }

}
