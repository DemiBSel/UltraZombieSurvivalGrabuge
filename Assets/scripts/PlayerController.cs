using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = "onChangePlayerName")]
    public string playerName;

    [SyncVar(hook = "paintPlayer")]
    public Color clothesColor;
    public GameObject clothes;

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
    [SyncVar(hook ="onChangePlayerCam")]
    public Transform playerCam;
    public Transform pickUpPosition;
    public float pickUpOffset;
    public Vector3 pickUpVectorToCenter;


    bool hasPlayer = false;
    bool beingCarried = false;
    bool touched = false;
    [SyncVar]
    public Vector3 scale;
    private float throwForce = 25;
    private NetworkIdentity objNetId;
    private GameObject score0;
    public int score;
    private float maxDist;

    

    Quaternion lastRotation;

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



        updateScore();

        pick();


    }

    //color
    public void setColor(Color aColor)
    {
        clothesColor = aColor;
    }

    public void paintPlayer(Color aColor)
    {
        clothesColor = aColor;
        clothes.GetComponent<Renderer>().material.color = clothesColor;
    }

    public void updateScore()
    {
        if (score0 == null)
        {
            score0 = GameObject.Find("Score0");
            maxDist = Vector3.Distance(transform.position, score0.transform.position);
            score = 0;
        }
        if(Vector3.Distance(transform.position,score0.transform.position)>maxDist)
        {
            score += 1;
            maxDist = Vector3.Distance(transform.position, score0.transform.position);
        }
        GameObject.Find("ScoreTextDisplay").GetComponent<Text>().text = "" + score;
    }

    public override void OnStartLocalPlayer()
    {
        //initAllShaders(); //peut etre pour plus tard
        CmdSetNameField(name);
        local_camera = (Camera)transform.Find("Tools").transform.Find("Main Camera").GetComponentInChildren<Camera>();
        local_camera.depth = 1;
        playerCam = local_camera.transform;

        jumping = false;
        fireRate = 0.25f;
        nextFire = 0;


        tools = transform.Find("Tools");
        gun = tools.transform.Find("Gun").transform;
        hand = tools.transform.Find("Hand").transform;

        menu = GameObject.Find("Network Manager").GetComponent<ConnectHUD>().getQuitHud();
        menu.SetActive(false);

        GameObject.Find("Network Manager").GetComponent<ConnectHUD>().lostPanel.transform.Find("EndPanelReset").GetComponent<Button>().onClick.AddListener(RespawnAll);
        GameObject.Find("Network Manager").GetComponent<ConnectHUD>().winPanel.transform.Find("EndPanelReset").GetComponent<Button>().onClick.AddListener(RespawnAll);
        //previously set syncvar callbacks
        foreach (GameObject curPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerController pc = curPlayer.GetComponent<PlayerController>();
            pc.paintPlayer(pc.clothesColor);
            pc.writeNameField();
        }

    }
    public void writeNameField()
    {
        gameObject.transform.Find("Healthbar Canvas").Find("Background").Find("NameField").GetComponent<Text>().text = playerName;
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

    public void RespawnAll()
    {
        CmdRespawnAll();
    }

    [Command]
    public void CmdRespawnAll()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            obj.GetComponent<PlayerController>().CmdRespawn();
            Debug.Log(name + " Told " + obj.name);
        }
    }

    [Command]
    public void CmdRespawn()
    {
        RpcRespawn();
    }
    [ClientRpc]
    public void RpcRespawn()
    {
        Debug.Log("Got rpc respawn");
        if(isLocalPlayer)
        {
            GetComponent<Health>().Respawn();
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
            this.gameObject.GetComponent<PlayerHUDControl>().gotWarnedBy(other);
        }
    }


    public void SetName(string name)
    {
        this.playerName = name;
    }

    public void onChangePlayerName(string name)
    {
        this.playerName = name;
        gameObject.transform.Find("Healthbar Canvas").Find("Background").Find("NameField").GetComponent<Text>().text = playerName;
    }


    
    public void pick()
    {
        if(!beingCarried)
        {
            pickable_Object = FindClosestObject().transform;
            playerCam = transform.GetChild(8).GetChild(0).transform;
        }

      
        RaycastHit hit;
        var ray = local_camera.ScreenPointToRay(Input.mousePosition);
        float dist = 0.0f;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.rigidbody != null)
            {
                dist = Vector3.Distance(gameObject.transform.position, hit.point);
            }
        }




        if (dist <= 5.0f)
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
         if (!beingCarried && hasPlayer && (Input.GetKeyDown("e")))
         {
            CmdPickup(pickable_Object.GetComponent<NetworkIdentity>(),this.GetComponent<NetworkIdentity>());
            lastRotation = pickable_Object.rotation;
        }


         if (beingCarried)
         {
            if (Input.GetKeyDown("e"))
             {
                CmdLetGo(pickable_Object.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());
                
             }
             else if (Input.GetMouseButtonDown(0))
             {
                CmdThrow(pickable_Object.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());
             }
            else
            {
                float step = 10 * Time.deltaTime;
                float angle = pickUpPosition.rotation.normalized.y;
                pickable_Object.transform.position = Vector3.MoveTowards(pickable_Object.transform.position, pickUpPosition.position + playerCam.forward * pickUpOffset/2, step);
                lastRotation = pickUpPosition.rotation; 
            }

         }
    }

    [Command]
    public void CmdPickup(NetworkIdentity obj,NetworkIdentity aPlayer)
    {
        obj.RemoveClientAuthority(obj.clientAuthorityOwner);
        obj.AssignClientAuthority(aPlayer.connectionToClient);
        RpcPickUp(obj.gameObject);
    }
    [ClientRpc]
    public void RpcPickUp(GameObject pickable_Object)
    {
        pickable_Object.GetComponent<Renderer>().material.color = Color.red;
        if (isLocalPlayer)
        {
            RaycastHit hit;
            var ray = local_camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody != null)
                {
                    Vector3 off = hit.point - pickable_Object.transform.position;
                    pickUpOffset = Mathf.Max(off.x,off.y,off.z);
                    pickUpVectorToCenter = off;

                }
            }
            pickable_Object.GetComponent<Rigidbody>().isKinematic = true;
            float step = 5 * Time.deltaTime;
            pickable_Object.transform.position = Vector3.MoveTowards(pickable_Object.transform.position, pickUpPosition.position + playerCam.forward * pickUpOffset, step);
            beingCarried = true;

        }
    }

    [Command]
    public void CmdLetGo(NetworkIdentity obj, NetworkIdentity aPlayer)
    {
        RpcLetGo(obj.gameObject);
    }

    [Command]
    public void CmdRemoveAuth(NetworkIdentity obj,NetworkIdentity aPlayer)
    {
        obj.RemoveClientAuthority(aPlayer.connectionToClient);
    }
    [ClientRpc]
    public void RpcLetGo(GameObject pickable_Object)
    {
        pickable_Object.GetComponent<Renderer>().material.color = Color.white;
        if (isLocalPlayer)
        { 
            pickable_Object.GetComponent<Rigidbody>().isKinematic = false;
            beingCarried = false;
            CmdRemoveAuth(pickable_Object.GetComponent<NetworkIdentity>(), GetComponent<NetworkIdentity>());
        }
    }


    [Command]
    public void CmdThrow(NetworkIdentity obj, NetworkIdentity aPlayer)
    {
        RpcThrow(obj.gameObject);
        obj.GetComponent<Rigidbody>().isKinematic = false;
    }
    [ClientRpc]
    public void RpcThrow(GameObject pickable_Object)
    {
        pickable_Object.GetComponent<Renderer>().material.color = Color.white;

        if (isLocalPlayer)
        {
            beingCarried = false;
            pickable_Object.GetComponent<Rigidbody>().isKinematic = false;
            pickable_Object.GetComponent<Rigidbody>().velocity = GetComponent<PlayerController>().playerCam.forward * throwForce;
        }

    }

    public void onChangePlayerCam(Transform aCam)
    {
        playerCam = aCam;
    }

    public void initAllShaders()
    {
        foreach(GameObject currPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            int i = 0;
            GameObject ch;
            while(i < currPlayer.transform.childCount)
            {
                ch = currPlayer.transform.GetChild(i).gameObject;
                if (ch.name.Contains("Mesh"))
                {
                    Renderer mesh = ch.GetComponent<Renderer>();
                    mesh.material.shader = Shader.Find("Custom/playerShader");
                    
                }
                
                i++;
            }
        }
    }
}


