using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour {

    public Joint joint;
    public GameObject inside ;
    private bool holding;
    public float width = 32.0f;
    public float height = 32.0f;
    // Use this for initialization
    void Start () {
        joint = this.gameObject.GetComponent<ConfigurableJoint>();
        holding = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButton("Fire1"))
        {

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10 , layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red, 10, true);
                Debug.Log("Did Hit ");
               if (hit.transform.gameObject.CompareTag("MoveObject"))
                {
                    Debug.Log("can move it");

                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.black, 10, true);
                Debug.Log("Did not Hit");
            }
            








          /*  if (!holding)
            {
                joint.connectedBody = inside.GetComponent<Rigidbody>();
                holding = true;
            }
            else
            {
                joint.connectedBody = null;
                holding = false;
            }*/

        }

	}

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered " + other.name);
        if(other.gameObject!=this.gameObject)
            inside = other.gameObject;

    }


}
