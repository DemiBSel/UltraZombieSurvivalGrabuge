using UnityEngine;
using UnityEngine.Networking;


public class PlayerController : NetworkBehaviour
{
	
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
    public Transform gun;
    public Transform hand;
    private Camera camera;
    public string state;
	
	
    void Update()
    {
		//to make sure only the local player runs this
		if (!isLocalPlayer)
		{
			return;
		}
		
		//movements
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        var rot_x = Input.GetAxis("Mouse X") * 10.0f; ;
        var rot_y = Input.GetAxis("Mouse Y")* 10.0f;


        transform.Rotate(0, rot_x, 0);
        transform.Translate(x, 0, z);

        //on bouge la caméra le pistolet et la main en faisant gaffe que la caméra parte pas trop haut ou trop bas
        if (camera.gameObject.transform.localRotation.x-rot_y/100.0f > -0.80 && camera.gameObject.transform.localRotation.x-rot_y/100.0f <0.8)
        {
            gun.Rotate(-rot_y, 0, 0);
            hand.Rotate(-rot_y, 0, 0);
            camera.transform.Rotate(-rot_y, 0, 0);
        }

        //shooting
        if (Input.GetButton("Fire2"))
		{
			CmdFire();
		}

        //state machine (pour les animations)
        if(x>0 || z>0)
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
        camera = (Camera)GetComponentInChildren<Camera>();
        camera.depth = 1;

        gun = transform.Find("Gun").transform;
        hand = transform.Find("Hand").transform;
	}
	
	[Command]
	void CmdFire()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 25;
        gun.Rotate(-0.5f, 0, 0);
        hand.Rotate(-0.5f, 0, 0);
        camera.transform.Rotate(-0.5f, 0, 0);

        NetworkServer.Spawn(bullet);
		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
	
}
