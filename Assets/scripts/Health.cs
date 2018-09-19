using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour 
{
    public const int maxHealth = 100;  
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
	public RectTransform healthBar;
    public bool destroyOnDeath;
    private NetworkStartPosition[] spawnPoints;


    private void Start()
    {
        if(isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int amount)
    {
		if(isServer)
		{

                currentHealth -= amount;
                if (currentHealth <= 0)
                {
                    if (destroyOnDeath)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        currentHealth = 0;
                        RpcRespawn();
                    }
                }
            
		}
    }
    
    void OnChangeHealth(int currentHealth)
    {
		healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
	}
	
	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			//reset initial health
			currentHealth = maxHealth;
			// move back to zero location
			Vector3 spawnPoint = Vector3.zero;

            if(spawnPoints != null && spawnPoints.Length>0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            transform.position = spawnPoint;
		}
	}
	
}
