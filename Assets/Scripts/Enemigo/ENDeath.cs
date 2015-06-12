using UnityEngine;
using System.Collections;

public class ENDeath : MonoBehaviour {
	
	protected ENEstadisticas stats;
    protected ENComportamiento enemigo;
	public ParticleSystem muerte;
	protected PlayerDataBase playerDB;
	public int level;
	public int index;
    private bool deadthInExplosion = false;
	public int chestLevel = 1;
	
	void Start() {
		stats = gameObject.GetComponent<ENEstadisticas> ();
        enemigo = gameObject.GetComponent<ENComportamiento>();
        
		this.playerDB = GameObject.Find("MultiplayerManager").GetComponent<PlayerDataBase>();
		
		char[] separator = {'_'};
		if(this.transform.parent != null)
		{
			string[] values = this.name.Split(separator);
            if (values[0] == "Boss")
            {
                this.level = -1;
                return;
            }
			this.level = int.Parse( values[1]);
			this.index = int.Parse( values[2]);
		}
		else 
			this.level = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (stats.PuntosSalud <= 0) {
			if (Network.isServer) {
				string parentName;
				//Instantiate(muerte, transform.position, Quaternion.identity);
				if(gameObject.transform.parent != null)
					parentName = gameObject.transform.parent.gameObject.name;
				else
					parentName = "Level_0";
				
				MapGenerator mG = GameObject.Find("Dungeon").GetComponent<MapGenerator>();
				
				
				//networkView.RPC ("SpawnChestAcrossTheNetwork", RPCMode.All, parentName);
				//networkView.RPC ("DestroyAccrosTheNetwork", RPCMode.AllBuffered);
				
				mG.MazmorraCompleta[this.level].enemyDatabase.EnemyList[this.index].isDead = true;
                deadthInExplosion = enemigo.DeadthInExplosion();
                GetComponent<NetworkView>().RPC("deadthInExplosionRPC", RPCMode.All, deadthInExplosion);
				GetComponent<NetworkView>().RPC ("SpawnChestAcrossTheNetwork", RPCMode.All, parentName);
				GetComponent<NetworkView>().RPC ("DestroyAccrosTheNetwork",RPCMode.AllBuffered);

				/*int exp = 0;
				switch(stats.Nivel) {
				case 1:
					exp = 50;
					break;
				case 2:
					exp = 100;
					break;
				case 3:
					exp = 200;
					break;
				case 4:
					exp = 800;
					break;
				case 5:
					exp = 1600;
					break;
				case 6:
					exp = 3200;
					break;
				case 7:
					exp = 12800;
					break;
				case 8:
					exp = 25600;
					break;
				case 9:
					exp = 51200;
					break;
				}
				Utils.player.GetComponent<Attributtes>().addExp(exp);
				
				//generateSlots gs = GameObject.Find("InventoryPanel").GetComponent<generateSlots>();
				//gs.AddGold(Utils.player.GetComponent<Attributtes>().level * 5);
				Utils.player.GetComponent<Attributtes>().addGold(stats.Nivel * 50);*/
				
				//Network.Destroy(gameObject);
				Network.RemoveRPCs (GetComponent<NetworkView>().viewID);
			}
		}

	}

    [RPC]
    void deadthInExplosionRPC(bool pe_death)
    {
        deadthInExplosion = pe_death;
    }

	[RPC]
	void SpawnChestAcrossTheNetwork(string parentName) {

        if (deadthInExplosion)
            return;
		int exp = 0;
        
		switch(stats.Nivel) {
		case 1:
			exp = 50;
			break;
		case 2:
			exp = 100;
			break;
		case 3:
			exp = 200;
			break;
		case 4:
			exp = 800;
			break;
		case 5:
			exp = 1600;
			break;
		case 6:
			exp = 3200;
			break;
		case 7:
			exp = 12800;
			break;
		case 8:
			exp = 25600;
			break;
		case 9:
			exp = 51200;
			break;
		case 10:
			exp = 51200;
			break;
		}
        
		Utils.player.GetComponent<Attributtes>().addExp(exp);
		
		//generateSlots gs = GameObject.Find("InventoryPanel").GetComponent<generateSlots>();
		//gs.AddGold(Utils.player.GetComponent<Attributtes>().level * 5);
		Utils.player.GetComponent<Attributtes>().addGold(stats.Nivel * 50);


		GameObject obj;
		Transform parentTransform = null;
		
		obj = GameObject.Find("parentName") as GameObject;
		if(obj != null)
			parentTransform = obj.transform;
		
		ChestSpawn.Spawn(chestLevel, gameObject.transform.position, parentTransform);
	}
	
	[RPC]
	void ApplyDamageOnOthers(int DmgRecibido)
	{
		stats.PuntosSalud = stats.PuntosSalud - DmgRecibido;
	}
	
	[RPC]
	void DestroyAccrosTheNetwork()
	{

		
		//Network.Destroy(gameObject);


        Destroy(stats.GetCanvasEnemigo());
		Destroy (gameObject);
	}
	
/*	void SendSameLevel(string parentName, int level)
	{
		for(int i = 0; i < this.playerDB.playersList.Count; i++)
		{
			if(this.playerDB.playersList[i].CurrentLevel == level)
			{
				if(this.playerDB.playersList[i].NetworkPlayer == Network.player)
				{
					SpawnChestAcrossTheNetwork(parentName);
					DestroyAccrosTheNetwork();
				}
				else
				{
					GetComponent<NetworkView>().RPC ("SpawnChestAcrossTheNetwork", this.playerDB.playersList[i].NetworkPlayer, parentName);
					GetComponent<NetworkView>().RPC ("DestroyAccrosTheNetwork", this.playerDB.playersList[i].NetworkPlayer);
				}
			}
		}
	}*/
}
