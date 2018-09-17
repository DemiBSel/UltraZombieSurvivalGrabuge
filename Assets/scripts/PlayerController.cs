using UnityEngine;
using UnityEngine.Networking;


public class PlayerController : NetworkBehaviour
{
	
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
    public Transform gun;
	
	
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
        var rot_y = Input.GetAxis("Mouse Y");

        transform.Rotate(0, rot_x, 0);
        transform.Translate(x, 0, z);

        gun.Rotate(-rot_y, 0, 0);
        
        //shooting
        if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}
        
    }
    
    
    public override void OnStartLocalPlayer()
	{
		//GetComponent<MeshRenderer>().material.color = Color.blue;
        Camera c = (Camera)GetComponentInChildren<Camera>();
        c.depth = 1;

        gun = GameObject.Find("Gun").transform;
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
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		NetworkServer.Spawn(bullet);
		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
	
}
