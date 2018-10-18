using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour {

    public Transform player;
    public Transform playerCam;
    public int throwForce = 5000;

    private bool hasPlayer = false;
    private bool beingCarried = false;
    private bool touched = false;

    public Vector3 scale;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        player = FindClosestPlayer().transform;
        playerCam = player.GetChild(8).GetChild(0).transform;
        float dist = Vector3.Distance(gameObject.transform.position, player.position);

        if (dist <= 2.0f)
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
            transform.localScale = Vector3.Scale(transform.parent.lossyScale,scale);
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
            }
            else if (Input.GetKeyUp("e"))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
            else if (Input.GetMouseButtonDown(0))
            {
               
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce);
                transform.parent = null;
                beingCarried = false;
                gameObject.GetComponent<Renderer>().material.color = Color.white;
                
            }
        }
    }
    void OnTriggerEnter()
    {
        if (beingCarried)
        {
            touched = true;
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
