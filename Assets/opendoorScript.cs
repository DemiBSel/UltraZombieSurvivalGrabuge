using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class opendoorScript : NetworkBehaviour {

    GameObject door;
    [SyncVar]
    bool opening;

    bool opened=false;
    bool closed = true;
    float initX;
    float max_angle = 90.0f;

	// Use this for initialization
	void Start () {
        door = transform.parent.Find("throwDoor").gameObject;
        initX = door.transform.localPosition.x;
    }
	
	// Update is called once per frame
	void Update () {
        

		if(opening && ! opened)
        {
            door.transform.Translate(door.transform.right  * 3 * Time.deltaTime);
        }
        if(!opening && !closed )
        {
            door.transform.Translate(-door.transform.right * 5 * Time.deltaTime);
        }

        if(door.transform.localPosition.x>initX+ door.transform.localScale.x)
        {
            opened = true;
        }
        if(door.transform.localPosition.x <= initX)
        {
            closed = true;
            opened = false;
        }
        else
        {
            closed = false;
        }

	}


    

    private void OnTriggerEnter(Collider other)
    {
        opening = true;
    }

   
    
    private void OnTriggerExit(Collider other)
    {
        opening = false;
    }


}
