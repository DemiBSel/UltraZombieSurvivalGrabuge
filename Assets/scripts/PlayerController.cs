using UnityEngine;
using UnityEngine.Networking;


public class PlayerController : NetworkBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform gun;

    public Transform tools;
    public Transform hand;
    private Camera local_camera;
    public string state;
    public bool jumping;

    public float fireRate;
    public float nextFire;


    void Update()
    {
        //to make sure only the local player runs this
        if (!isLocalPlayer)
        {
            if(tools==null)
            tools = transform.Find("Tools");
            return;
        }
        //movements
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;


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
        if (Input.GetButton("Fire2") && Time.time >nextFire)
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

    }


    public override void OnStartLocalPlayer()
    {
        //GetComponent<MeshRenderer>().material.color = Color.blue;
        local_camera = (Camera)transform.Find("Tools").transform.Find("Main Camera").GetComponentInChildren<Camera>();
        local_camera.depth = 1;

        jumping = false;
        fireRate = 0.25f;
        nextFire = 0;


    tools = transform.Find("Tools");
        gun = tools.transform.Find("Gun").transform;
        hand = tools.transform.Find("Hand").transform;
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

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Terrain"))
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

}
