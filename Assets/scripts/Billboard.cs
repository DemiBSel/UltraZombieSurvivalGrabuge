using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Billboard : NetworkBehaviour {

    private Camera local_camera;


    void Update () {

        if (local_camera != null)
            transform.LookAt(local_camera.transform);
        else
            local_camera = findCam();
	}

    Camera findCam()
    {
        if (GameObject.Find("Main Camera") != null)
            return GameObject.Find("Main Camera").GetComponent<Camera>();
        else
            return null;
    }
	
}
