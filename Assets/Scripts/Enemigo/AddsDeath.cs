using UnityEngine;
using System.Collections;

public class AddsDeath : ENDeath {
	
	public GameObject MobsContainer;
	
	// Use this for initialization
	void Start ()
	{
		this.stats = gameObject.GetComponent<ENEstadisticas> ();
		this.enemigo = gameObject.GetComponent<ENComportamiento>();
		this.playerDB = GameObject.Find("MultiplayerManager").GetComponent<PlayerDataBase>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (stats.PuntosSalud <= 0) 
		{
			if (Network.isServer) 
			{
				//Network.Destroy(gameObject);
				GetComponent<NetworkView>().RPC ("DestroyAccrosTheNetwork", RPCMode.All);
			}
		}
	}
    public void DeadthDragon()
    {
        stats.PuntosSalud = -10;
    }
}
