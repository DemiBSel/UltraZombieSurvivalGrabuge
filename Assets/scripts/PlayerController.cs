using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = "onChangePlayerName")]
    public string playerName;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform gun;

    GameObject menu;
    bool menuOn = false;

    public Transform tools;
    public Transform hand;
    private Camera local_camera;
    public string state;
    public bool jumping;

    public float fireRate;
    public float nextFire;

    public Transform pickable_Object;
    public Transform playerCam;
    bool hasPlayer = false;
    bool beingCarried = false;
    bool touched = false;
    public Vector3 scale;
    public int throwForce = 100;
    private NetworkIdentity objNetId;

    void Update()
    {
        //to make sure only the local player runs this
        if (!isLocalPlayer)
        {
            if (tools == null)
                tools = transform.Find("Tools");
            return;
        }
        //movements
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 5.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 5.0f;


        //jump

        if (Input.GetButton("Jump") && !jumping)
        {
            this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 0));
            jumping = true;
        }


        var rot_x = Input.GetAxis("Mouse X") * 10.0f; ;
        var rot_y = Input.GetAxis("Mouse Y") * 10.0f;


        bool blockCam = tools.localRotation.x - rot_y / 100.0f > 0 && tools.localRotation.x - rot_y / 100.0f < 1;
        CmdMovements(blockCam, (float)x, (float)z, (float)rot_x, (float)rot_y);
        //attention le nom de la variable blockCam est pas logique du tout mdr


        //shooting
        if (Input.GetButton("Fire2") && Time.time > nextFire)
        {
            CmdFire();
            nextFire = Time.time + fireRate;
        }

        //state machine (pour les animations)
        if (x > 0 || z > 0)
        {
            state = "running";
        }
        else
        {
            state = "idle";
        }

        // Update is called once per frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOn)
            {
                menu.SetActive(false);
                menuOn = false;
                Debug.Log("coucou???");
            }
            else
            {
                menu.SetActive(true);
                menuOn = true;
            }
        }


        //To warn others
        if (Input.GetKeyDown("a"))
        {
            Debug.Log("Warned others");
            CmdWarnOthers();
        }

        pick();


    }

    public override void OnStartLocalPlayer()
    {
        CmdSetNameField(name);
        local_camera = (Camera)transform.Find("Tools").transform.Find("Main Camera").GetComponentInChildren<Camera>();
        local_camera.depth = 1;

        jumping = false;
        fireRate = 0.25f;
        nextFire = 0;


        tools = transform.Find("Tools");
        gun = tools.transform.Find("Gun").transform;
        hand = tools.transform.Find("Hand").transform;

        menu = GameObject.Find("Network Manager").GetComponent<ConnectHUD>().getQuitHud();
        menu.SetActive(false);

    }

    [Command]
    void CmdSetNameField(string name)
    {
        gameObject.transform.Find("Healthbar Canvas").Find("Background").Find("NameField").GetComponent<Text>().text = playerName;
    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 25;
        //tools.Rotate(-0.5f, 0, 0);
        //RpcRecoil();

        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
    [ClientRpc]
    void RpcRecoil()
    {
        tools.Rotate(-0.5f, 0, 0);
    }

    void CmdMovements(bool blockCam, float x, float z, float rot_x, float rot_y)
    {

        transform.Rotate(0, rot_x, 0);
        transform.Translate(x, 0, z);

        //on bouge la caméra le pistolet et la main en faisant gaffe que la caméra parte pas trop haut ou trop bas
        if (blockCam)
        {
            tools.Rotate(-rot_y, 0, 0);
        }
    }


    //ça c pour empecher les double jumps
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Terrain"))
        {
            this.jumping = false;
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Terrain"))
        {
            this.jumping = false;
        }
    }

    [Command]
    public void CmdWarnOthers()
    {
        GameObject[] others = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject other in others)
        {
            if (other != this.gameObject)
            {
                PlayerController otherContr = other.GetComponent<PlayerController>();
                otherContr.RpcReceiveWarn(this.gameObject);
            }
        }
    }


    public GameObject FindClosestObject()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("object");
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

    [ClientRpc]
    public void RpcReceiveWarn(GameObject other)
    {
        if (isLocalPlayer)
        {
            Debug.Log("reçu un warn");
            this.gameObject.GetComponent<PlayerHUDControl>().gotWarnedBy(other);
        }
    }


    public void SetName(string name)
    {
        this.playerName = name;
        gameObject.transform.Find("Healthbar Canvas").Find("Background").Find("NameField").GetComponent<Text>().text = playerName;
    }

    public void onChangePlayerName(string name)
    {
        this.playerName = name;
        gameObject.transform.Find("Healthbar Canvas").Find("Background").Find("NameField").GetComponent<Text>().text = playerName;
    }


    
    public void pick()
    {
        
        pickable_Object = FindClosestObject().transform;
        playerCam = transform.GetChild(8).GetChild(0).transform;
        float dist = Vector3.Distance(gameObject.transform.position, pickable_Object.position);

        if (dist <= 3.0f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }
       

        pickup();


    }


    public void pickup()
    {
         if (hasPlayer && (Input.GetKeyDown("e")))
         {
            CmdPickup(pickable_Object.GetComponent<NetworkIdentity>(),this.GetComponent<NetworkIdentity>());
         }


         if (beingCarried)
         {
             if (touched)
             {
                 pickable_Object.GetComponent<Rigidbody>().isKinematic = false;
                 pickable_Object.parent = null;
                 beingCarried = false;
                 touched = false;
                 Debug.Log("aaaaaaaaa ");
                 pickable_Object.GetComponent<Renderer>().material.color = Color.white;
                 pickable_Object.localScale = scale;

             }
             else if (Input.GetKeyUp("e"))
             {
                CmdLetGo(pickable_Object.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());
             }
             else if (Input.GetMouseButtonDown(0))
             {
                CmdThrow(pickable_Object.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());
             }

         }
    }

    [Command]
    public void CmdPickup(NetworkIdentity obj,NetworkIdentity aPlayer)
    {
        obj.AssignClientAuthority(aPlayer.connectionToClient);
        RpcPickUp();

    }
    [ClientRpc]
    public void RpcPickUp()
    {
        if (isLocalPlayer)
        {

            pickable_Object.GetComponent<Rigidbody>().isKinematic = true;
            scale = pickable_Object.lossyScale;
            pickable_Object.SetParent(playerCam);
            beingCarried = true;
            Debug.Log("et ici on prend l'objet?? ");
            pickable_Object.localScale = scale;
            pickable_Object.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    [Command]
    public void CmdLetGo(NetworkIdentity obj, NetworkIdentity aPlayer)
    {
        RpcLetGo();
        obj.RemoveClientAuthority(aPlayer.connectionToClient);
    }
    [ClientRpc]
    public void RpcLetGo()
    {
        if (isLocalPlayer)
        {
            pickable_Object.GetComponent<Rigidbody>().isKinematic = false;
            pickable_Object.parent = null;
            beingCarried = false;
            pickable_Object.GetComponent<Renderer>().material.color = Color.white;
            pickable_Object.localScale = scale;
        }
    }


    [Command]
    public void CmdThrow(NetworkIdentity obj, NetworkIdentity aPlayer)
    {
        obj.RemoveClientAuthority(aPlayer.connectionToClient);
        RpcThrow();

    }
    [ClientRpc]
    public void RpcThrow()
    {
        if (isLocalPlayer)
        {
            pickable_Object.GetComponent<Rigidbody>().isKinematic = false;
            pickable_Object.GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce);
            pickable_Object.parent = null;
            beingCarried = false;
            pickable_Object.GetComponent<Renderer>().material.color = Color.white;
            pickable_Object.localScale = scale;
        }

    }
}


