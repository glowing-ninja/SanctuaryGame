using UnityEngine;
using System.Collections;

public class BossDeath : ENDeath {

	public GameObject MobsContainer;
	public GameObject bossTp;
	public Transform bossTpAncla;

	// Use this for initialization
	void Start ()
	{
		this.stats = gameObject.GetComponent<ENEstadisticas> ();
		this.enemigo = gameObject.GetComponent<ENComportamiento>();
		this.playerDB = GameObject.Find("MultiplayerManager").GetComponent<PlayerDataBase>();
		this.MobsContainer = GameObject.Find("BabyDragonContainer");
		this.bossTpAncla = GameObject.Find("tpPosition").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (stats.PuntosSalud <= 0) 
		{
			if (Network.isServer) 
			{
				//Network.Destroy(gameObject);
				GetComponent<NetworkView>().RPC ("SpawnChestAcrossTheNetwork", RPCMode.All, "BossLevel");
				GetComponent<NetworkView>().RPC ("DestroyAccrosTheNetwork", RPCMode.All);

				int childNum = this.MobsContainer.transform.childCount;

				//for(int i = childNum - 1; i >= 0; i++)
                for (int i = 0; i < childNum; i++)
                {
                    this.MobsContainer.transform.GetChild(i).gameObject.GetComponent<AddsDeath>().DeadthDragon();
                    //GameObject.Destroy(this.MobsContainer.transform.GetChild(i).gameObject);
                }
				GameObject.Instantiate(bossTp, bossTpAncla.position, bossTp.transform.rotation);

				Network.RemoveRPCs (GetComponent<NetworkView>().viewID);
			}
		}
	}
}
