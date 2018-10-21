using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour {

    public Transform player;
    public Transform playerCam;
    public int throwForce = 100;

    private bool hasPlayer = false;
    private bool beingCarried = false;
    private bool touched = false;
    private Vector3 position;

    private GameObject parent;

    public Vector3 scale;

    // Use this for initialization
    void Start() {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        player = FindClosestPlayer().transform;
        playerCam = player.GetChild(8).GetChild(0).transform;
        float dist = Vector3.Distance(gameObject.transform.position, player.position);

        if (dist <= 3.0f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }

        if (hasPlayer && (Input.GetKeyDown("e")))
        {
            scale = transform.lossyScale;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.SetParent(playerCam);
            beingCarried = true;
            Debug.Log("et ici on prend l'objet?? ");
            transform.localScale = scale;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        if (beingCarried)
        {
            if (touched)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                touched = false;
                Debug.Log("aaaaaaaaa ");
                gameObject.GetComponent<Renderer>().material.color = Color.white;
                transform.localScale = scale;

            }
            else if (Input.GetKeyUp("e"))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                gameObject.GetComponent<Renderer>().material.color = Color.white;
                transform.localScale = scale;

            }
            else if (Input.GetMouseButtonDown(0))
            {
               
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce);
                Debug.Log(playerCam.forward * throwForce);
                transform.parent = null;
                beingCarried = false;
                gameObject.GetComponent<Renderer>().material.color = Color.white;
                transform.localScale = scale;

            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (beingCarried)
        {
            touched = true;
        }
        if (other.gameObject.tag == "reset")
        {
            transform.position = position;
        }

   }


    public GameObject FindClosestPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
