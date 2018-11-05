using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    
    void OnCollisionEnter(Collision collision)
    {
		var hit = collision.gameObject;
		var health = hit.GetComponent<Health>();
		if(health != null)
		{
			health.TakeDamage(10);
		}
        var hud = hit.GetComponent<PlayerHUDControl>();
        if (hud != null)
        {
            hud.gotHurtBy(this.gameObject);
        }

        Destroy(gameObject);

 
       
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
