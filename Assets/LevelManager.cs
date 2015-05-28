using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelManager : MonoSingleton<LevelManager> {
	
	
	public Utils.DungeonType currentDungeon;
	public bool bossLoaded = false;
	public bool inBoss = false;
	public int playersInDungeon = 0;
	private PlayerDataBase playerDB;
	private MapGenerator mapGenerator = null;
	private bool isPlayerInDungeon = false;
	//private MapGenerator mapGenerator;

    private Pathfinder pathFinder;

	public override void Init ()
	{
		base.Init ();
		this.currentDungeon = Utils.DungeonType.NONE;
		this.playerDB = GameObject.Find("MultiplayerManager").GetComponent<PlayerDataBase>();
		//this.mapGenerator = this.gameObject.GetComponent<MapGenerator>();
	}
	
	public void EnterDungeon(Utils.DungeonType nextDungeon)
	{
		
		//Puede que nos interese volver al nivel principal
		if(nextDungeon == Utils.DungeonType.NONE)
		{
			
		}
		else if(this.currentDungeon == Utils.DungeonType.NONE)
		{
			StartCoroutine("OscurecerPantalla");
			
			//Si no tenemos una mazmorra abierta, creamos la mazmorra y nos desplazamos al primer nivel
			if(Network.isServer)
			{
				switch(nextDungeon) {
				case Utils.DungeonType.DUNGEON1:
					GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewAdditiveLevel ("2_GameField", Network.player, (int)nextDungeon, false);
					currentDungeon = nextDungeon;
					this.SetIsInDungeon(Network.player, true, (int)currentDungeon, 0);
					break;
				case Utils.DungeonType.DUNGEON2:
					GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewAdditiveLevel ("5_IceDungeon", Network.player, (int)nextDungeon, false);
					currentDungeon = nextDungeon;
					this.SetIsInDungeon(Network.player, true, (int)currentDungeon, 0);
					break;
				}
			}
			else
			{
				switch(nextDungeon) {
				case Utils.DungeonType.DUNGEON1:
					GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().GetComponent<NetworkView>().RPC("LoadNewAdditiveLevelOnServer", RPCMode.Server, "2_GameField", Network.player, (int)nextDungeon, false);
					currentDungeon = nextDungeon;
					this.SetIsInDungeon(Network.player, true, (int)currentDungeon, 0);
					break;
				case Utils.DungeonType.DUNGEON2:
					GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().GetComponent<NetworkView>().RPC("LoadNewAdditiveLevelOnServer", RPCMode.Server, "5_IceDungeon", Network.player, (int)nextDungeon, false);
					currentDungeon = nextDungeon;
					this.SetIsInDungeon(Network.player, true, (int)currentDungeon, 0);
					break;
				}
				GetComponent<NetworkView>().RPC("SetIsInDungeon", RPCMode.Server, Network.player, true, (int)currentDungeon, 0);
			}
			this.isPlayerInDungeon = true;
		}
		else if(this.currentDungeon != nextDungeon)
		{
			//Si tenemos una mazmorra abierta e intentamos entrar en otra distinta, mostramos un mensaje de que estamos en otra
			Debug.Log("Aqui debemos informar de que tenemos otra mazmora abierta");
		}
		else
		{
			//Si tenemos una mazmorra abierta e intentamos entrar en la misma, nos desplazamos al primer nivel
			Utils.player.transform.position = new Vector3(-10f + Utils.mapOffset, 1f, 10f + Utils.mapOffset);
			GetComponent<NetworkView>().RPC("SetIsInDungeon", RPCMode.Server, Network.player, true, (int)currentDungeon, 0);
			if(!Network.isServer)
			{
				//GetComponent<NetworkView>().RPC("RequestEnemiesOnLevel", RPCMode.Server, 0);
				this.SetIsInDungeon(Network.player, true, (int)currentDungeon, 0);
				GetComponent<NetworkView>().RPC("SetIsInDungeon", RPCMode.Server, Network.player, true, (int)currentDungeon, 0);
			}
			this.SetIsInDungeon(Network.player, true, (int)currentDungeon, 0);
			this.isPlayerInDungeon = true;
		}
	}
	
	public void SwitchDungeonLevel(bool nextLevel)
	{
		
		
		GameObject dungeon = GameObject.Find ("Dungeon");
		
		
		if(dungeon)
		{
			//De momento usamos un Find. Mas adelante lo tendra desde el inicio
			MapGenerator mg = dungeon.GetComponent<MapGenerator> ();
			int actual = mg.actual;
			if (nextLevel && actual + 1 < mg.depth) 
			{
				if (mg.MazmorraCompleta[actual + 1] == null)
				{
					actual++;
					if(Network.isServer)
					{
						this.ChangeMapLevel(nextLevel, actual, Network.player);
						this.SetIsInDungeon(Network.player, true, (int)currentDungeon, actual);
					}
					else
					{
						this.GetComponent<NetworkView>().RPC("ChangeMapLevel", RPCMode.Server, nextLevel, actual, Network.player);
						GetComponent<NetworkView>().RPC("SetIsInDungeon", RPCMode.Server, Network.player, true, (int)currentDungeon, actual);
						this.HideAndShowLevel(actual - 1, actual);
						//GetComponent<NetworkView>().RPC("RequestEnemiesOnLevel", RPCMode.Server, actual);
						
					}
					mg.actual = actual;
					
				}
				else
				{
					actual++;
					Vector3 newPos = new Vector3(10f + 250 * (actual + 1), 1f, 10f + 250 * (actual + 1));
					Utils.player.transform.position = newPos;
					mg.actual = actual;
					if(Network.isServer)
						this.SetIsInDungeon(Network.player, true, (int)currentDungeon, actual);
					else
					{
						GetComponent<NetworkView>().RPC("SetIsInDungeon", RPCMode.Server, Network.player, true, (int)currentDungeon, actual);
						this.HideAndShowLevel(actual - 1, actual);
						//GetComponent<NetworkView>().RPC("RequestEnemiesOnLevel", RPCMode.Server, actual);
					}
				}
			}
			else if (!nextLevel && actual > 0) 
			{
				actual--;
				Vector3 newPos = new Vector3(190f + 250 * (actual + 1), 1f, 10f + 250 * (actual + 1));
				GameObject.FindGameObjectWithTag ("Player").transform.position = newPos;
				mg.actual = actual;
				
				if(Network.isServer)
					this.SetIsInDungeon(Network.player, true, (int)currentDungeon, actual);
				else
				{
					GetComponent<NetworkView>().RPC("SetIsInDungeon", RPCMode.Server, Network.player, true, (int)currentDungeon, actual);
					this.HideAndShowLevel(actual + 1, actual);
					//GetComponent<NetworkView>().RPC("RequestEnemiesOnLevel", RPCMode.Server, actual);
				}
				
				
				
			}
			else if (!nextLevel && actual == 0)
			{
				//GameObject.Find("SpawnManager").GetComponent<SpawnScript>().SpawnPlayer();
				//Utils.player.transform.position = new Vector3(125f,13f,160f);
				GameObject portal = GameObject.Find("Portal") as GameObject;
				Vector3 position = new Vector3(portal.transform.position.x, portal.transform.position.y - 3f, portal.transform.position.z - 5f);
				Utils.player.transform.position = position;
				
				this.resetActual();
			}
			else if (nextLevel && actual + 1 >= mg.depth) 
			{
				
				actual++;
				mg.actual = actual;
				if(Network.isServer)
				{
					GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewAdditiveLevel ("3_BossMap", Network.player, -1, true);
					this.SetIsInDungeon(Network.player, true, (int)currentDungeon, actual);
				}
				else
				{
					GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().GetComponent<NetworkView>().RPC("LoadNewAdditiveLevelOnServer", RPCMode.Server, "3_BossMap", Network.player, -1, true);
					GetComponent<NetworkView>().RPC("SetIsInDungeon", RPCMode.Server, Network.player, true, (int)currentDungeon, actual);
					mg.MazmorraCompleta[actual - 1].terrainParent.SetActive(false);
				}
				//GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewLevel ("3_BossMap");
				//this.HideAndShowLevel(actual - 1, actual);
				
			}
			//if(Network.isClient)
			//StartCoroutine ("OscurecerPantalla", GameObject.Find("Terrain").transform);
		}
	}
	
	void OnNetworkLoadedLevel(NetworkPlayer nPlayer)
	{
		Debug.Log ("ONNETWORKLOADEDLEVEL FUERA");
		if (Network.isServer)
		{
			Debug.Log ("ONNETWORKLOADEDLEVEL DENTRO");
			GameObject mapObject = GameObject.Find ("Dungeon") as GameObject;
			
			
			if (mapObject != null) {
				this.mapGenerator = mapObject.GetComponent<MapGenerator> ();
				
				if(mapGenerator.actual < mapGenerator.depth)
				{
					byte[] serializedMap = this.MapSerialize (mapGenerator.MazmorraCompleta [mapGenerator.actual]);
					//byte[] serializedChest = this.ChestDatabaseSerialize (mapGenerator.chestDatabase);
					
					GetComponent<NetworkView>().RPC ("SendMap", RPCMode.OthersBuffered, serializedMap, mapGenerator.depth, mapGenerator.actual, nPlayer.ToString());
					for(int i = 0; i < mapGenerator.MazmorraCompleta[0].enemyDatabase.Size; i++)
					{
						GetComponent<NetworkView>().RPC ("PlaceEnemiesNetwork", RPCMode.AllBuffered, mapGenerator.MazmorraCompleta[0].enemyDatabase.getPositionAt(i), "Enemigo",  0, i, mapGenerator.MazmorraCompleta[0].enemyDatabase.EnemyList[i].viewID, "Level_0",mapGenerator.MazmorraCompleta[0].enemyDatabase.EnemyList[i].enemyPath );
					}
				}
			}
		}
		else
		{
			GameObject mapObject = GameObject.Find ("Dungeon") as GameObject;
			
			
			if (mapObject != null) 
				this.mapGenerator = mapObject.GetComponent<MapGenerator> ();
		}
		/*else
		{
			GameObject mapObject = GameObject.Find ("Dungeon") as GameObject;
			
			
			if (mapObject != null)
			{
				MapGenerator mapGenerator = mapObject.GetComponent<MapGenerator> ();

				if(mapGenerator.MazmorraCompleta[0] == null)
				{
					this.networkView.RPC("ChangeMapLevel", RPCMode.Server, true, 0, Network.player);
				}
			}
		}*/
	}
	
	private byte[] MapSerialize(Map map) 
	{
		
		BinaryFormatter binFormatter = new BinaryFormatter();
		MemoryStream memStream = new MemoryStream();
		
		binFormatter.Serialize (memStream, map);
		byte[] serialized = memStream.ToArray ();
		
		memStream.Close ();
		
		return serialized;
	}
	
	private byte[] EnemiesDataBaseSerialize(EnemyDatabase eDB) 
	{
		
		BinaryFormatter binFormatter = new BinaryFormatter();
		MemoryStream memStream = new MemoryStream();
		
		binFormatter.Serialize (memStream, eDB);
		byte[] serialized = memStream.ToArray ();
		
		memStream.Close ();
		
		return serialized;
	}
	
	
	private byte[] ChestDatabaseSerialize(ChestDatabase chestDB) 
	{
		
		BinaryFormatter binFormatter = new BinaryFormatter();
		MemoryStream memStream = new MemoryStream();
		
		binFormatter.Serialize (memStream, chestDB);
		byte[] serialized = memStream.ToArray ();
		
		memStream.Close ();
		
		return serialized;
	}
	
	private Map DeserializeMap(byte[] map)
	{
		BinaryFormatter binFormatter = new BinaryFormatter(); 
		MemoryStream memStream = new MemoryStream();
		
		memStream.Write(map,0,map.Length); 
		
		memStream.Seek(0, SeekOrigin.Begin); 
		
		return (Map)binFormatter.Deserialize(memStream);
	}
	
	private EnemyDatabase DeserializeEnemyDB(byte[] eDB)
	{
		BinaryFormatter binFormatter = new BinaryFormatter(); 
		MemoryStream memStream = new MemoryStream();
		
		memStream.Write(eDB,0,eDB.Length); 
		
		memStream.Seek(0, SeekOrigin.Begin); 
		
		return (EnemyDatabase)binFormatter.Deserialize(memStream);
	}
	
	private ChestDatabase DeserializeChestDatabase(byte[] chestDB)
	{
		BinaryFormatter binFormatter = new BinaryFormatter(); 
		MemoryStream memStream = new MemoryStream();
		
		memStream.Write(chestDB,0,chestDB.Length); 
		
		memStream.Seek(0, SeekOrigin.Begin); 
		
		return (ChestDatabase)binFormatter.Deserialize(memStream);
	}
	
	
	/*void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		GameObject mapObject = GameObject.Find ("Dungeon") as GameObject;
		
		if (mapObject != null) {
			MapGenerator mapGenerator = mapObject.GetComponent<MapGenerator> ();
			byte[] serializedMap = this.MapSerialize (mapGenerator.MazmorraCompleta [0]);
			//byte[] serializedChest = this.ChestDatabaseSerialize (mapGenerator.chestDatabase);
			NetworkLevelLoading nLevel = GameObject.Find("LevelLoader").GetComponent<NetworkLevelLoading> ();
			nLevel.LoadNewAdditiveLevel("2_GameField", networkPlayer, (int)this.currentDungeon, inBoss);
			//nLevel.networkView.RPC ("LoadLevelAdditive", networkPlayer, "2_GameField", nLevel.lastLevelPrefix, networkPlayer, this.currentDungeon, inBoss);
			GetComponent<NetworkView>().RPC ("SendMap", networkPlayer, serializedMap, mapGenerator.depth, 0, "-1");
			for(int i = 0; i < mapGenerator.MazmorraCompleta[0].enemyDatabase.Size; i++)
			{
				GetComponent<NetworkView>().RPC ("PlaceEnemiesNetwork", RPCMode.All, mapGenerator.MazmorraCompleta[0].enemyDatabase.getPositionAt(i), "Enemigo",  0, i, mapGenerator.MazmorraCompleta[0].enemyDatabase.EnemyList[i].viewID, "Level_0", mapGenerator.MazmorraCompleta[0].enemyDatabase.EnemyList[i].enemyPath);
			}
			
		}
	}
	*/
	
	
	
	
	
	IEnumerator OscurecerPantalla() {
		GameObject negro = GameObject.Find ("BlackPanel");
		float alpha = negro.GetComponent<Image> ().color.a;
		/*while (alpha < 1) {
			alpha += 0.2f;
			negro.GetComponent<Image> ().color = new Vector4(negro.GetComponent<Image> ().color.r,
			                                                 negro.GetComponent<Image> ().color.g,
			                                                 negro.GetComponent<Image> ().color.b,
			                                                 alpha);
			yield return new WaitForSeconds (0.1f);
		}
		
		while (alpha > 0) {
			alpha -= 0.2f;
			negro.GetComponent<Image> ().color = new Vector4(negro.GetComponent<Image> ().color.r,
			                                                 negro.GetComponent<Image> ().color.g,
			                                                 negro.GetComponent<Image> ().color.b,
			                                                 alpha);
			yield return new WaitForSeconds (0.1f);
		}*/
		if(alpha < 1)
			alpha = 1;
		negro.GetComponent<Image> ().color = new Vector4(negro.GetComponent<Image> ().color.r,
		                                                 negro.GetComponent<Image> ().color.g,
		                                                 negro.GetComponent<Image> ().color.b,
		                                                 alpha);
		yield return new WaitForSeconds (1f);
		
		alpha = 0;
		negro.GetComponent<Image> ().color = new Vector4(negro.GetComponent<Image> ().color.r,
		                                                 negro.GetComponent<Image> ().color.g,
		                                                 negro.GetComponent<Image> ().color.b,
		                                                 alpha);
	}
	
	public void DecrementPlayers()
	{
		this.playersInDungeon--;
		
		if(this.playersInDungeon <= 0)
		{
			this.playersInDungeon = 0;
			//GetComponent<NetworkView>().RPC("DestroyDungeon", RPCMode.All);
		}
	}
	
	[RPC]
	void SetIsInDungeon(NetworkPlayer nPlayer, bool inDungeon, int dungeon, int level)
	{
		if(inDungeon)
			this.playersInDungeon++;
		else
			this.playersInDungeon--;
		
		this.playerDB.SetDungeon(nPlayer, (Utils.DungeonType)dungeon, level);
		
		if(this.playersInDungeon <= 0)
		{
			this.playersInDungeon = 0;
			//GetComponent<NetworkView>().RPC("DestroyDungeon", RPCMode.All);
		}
		
		GameObject dungeonObject = GameObject.Find ("Dungeon");
		
		
		if(dungeonObject)
		{
			MapGenerator mg = dungeonObject.GetComponent<MapGenerator> ();
			if(level >= mg.depth)
			{
				bossLoaded = true;
				inBoss = true;
			}
		}
	}


	public void DestroyDungeon()
	{
		if(Network.isServer)
			GetComponent<NetworkView>().RPC("DestroyDungeonOnNetwork", RPCMode.All);
	}
	
	[RPC]
	void DestroyDungeonOnNetwork()
	{
		GameObject dungeon = GameObject.Find ("Dungeon");
		
		if(dungeon)
		{
			Destroy(dungeon);
			this.currentDungeon = Utils.DungeonType.NONE;
			this.bossLoaded =  false;
			this.inBoss = false;
			this.playersInDungeon = 0;
			this.mapGenerator = null;

			if(this.isPlayerInDungeon)
			{
				GameObject portal = GameObject.Find("Portal") as GameObject;
				Vector3 position = new Vector3(portal.transform.position.x, portal.transform.position.y - 3f, portal.transform.position.z - 5f);
				Utils.player.transform.position = position;
			}
		}
	}
	
	[RPC]
	public void PlaceEnemiesNetwork(Vector3 pos, string name, int level, int index,NetworkViewID viewID, string parentName, string prefabPath)
	{
		GameObject parentObject = GameObject.Find(parentName);
		
		if(parentObject)
		{
			Transform parent = parentObject.transform;
			GameObject obj = Resources.Load<GameObject> (prefabPath);
			GameObject aux = Instantiate(obj, pos, Quaternion.identity) as GameObject;
			NetworkView nView;
			nView = aux.GetComponent<NetworkView>();
			nView.viewID = viewID;
			aux.name = name + "_" + level + "_" + index;
			aux.transform.SetParent(parent, false);
			if(mapGenerator)
			{
				mapGenerator.MazmorraCompleta[level].enemyDatabase.EnemyList[index].enemy = aux;
			}
		}
		
	}
	
	[RPC]
	public void ChangeMapLevel(bool goDown, int actualLevel, NetworkPlayer nPlayer)
	{
		GameObject  mapObject = GameObject.Find("Dungeon") as GameObject;
		MapGenerator mapGenerator = mapObject.GetComponent<MapGenerator>();
		if(mapGenerator.MazmorraCompleta == null)
			return;
		
		//	int actual = mapGenerator.actual;
		int depth = mapGenerator.depth;
		
		if (goDown && actualLevel < depth && Network.isServer)
		{
			if (mapGenerator.MazmorraCompleta[actualLevel] == null)
			{
				Debug.Log("NULL");
				mapGenerator.MazmorraCompleta[actualLevel] = MapGenerator.mapGenerator();
				
				PlaceEnemies pEnemies = mapObject.GetComponent<PlaceEnemies>();

				GameObject dungeonTerrain = null;

				switch(currentDungeon) {
					case Utils.DungeonType.DUNGEON1:
						dungeonTerrain = Resources.Load<GameObject> ("DungeonTerrain");
						break;
					case Utils.DungeonType.DUNGEON2:
						dungeonTerrain = Resources.Load<GameObject> ("IceDungeonTerrain");
						break;
				}
				GameObject instantiantedDungeon = Instantiate(dungeonTerrain, new Vector3(100 * actualLevel, 0, 100 * actualLevel), Quaternion.identity) as GameObject;
				instantiantedDungeon.transform.position = new Vector3(250 * (actualLevel + 1), 0, 250 * (actualLevel + 1));
				instantiantedDungeon.name = "Level_" + actualLevel;

				pEnemies.Place(instantiantedDungeon.transform, mapGenerator.MazmorraCompleta[actualLevel]);
				mapGenerator.GenerateChests(actualLevel, instantiantedDungeon);
				mapGenerator.MazmorraCompleta[actualLevel].terrainParent = instantiantedDungeon;
				mapGenerator.Render(mapGenerator.MazmorraCompleta[actualLevel].mapa, instantiantedDungeon.transform);

                pathFinder = instantiantedDungeon.GetComponent<Pathfinder>();
                pathFinder.CrearMapa(new Vector2(instantiantedDungeon.transform.position.x,
                                                 instantiantedDungeon.transform.position.z),
                                     new Vector2(instantiantedDungeon.transform.position.x + 200,
                                                 instantiantedDungeon.transform.position.z + 200));
			}
		}
		
		byte[] serializedMap = this.MapSerialize (mapGenerator.MazmorraCompleta [actualLevel]);
		//		byte[] serializedChest = this.ChestDatabaseSerialize (mapGenerator.chestDatabase);
		GetComponent<NetworkView>().RPC ("SendMap", RPCMode.AllBuffered, serializedMap, mapGenerator.depth, actualLevel, nPlayer.ToString());
		for(int i = 0; i < mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.Size; i++)
		{
			GetComponent<NetworkView>().RPC ("PlaceEnemiesNetwork", RPCMode.AllBuffered, mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.getPositionAt(i), "Enemigo", actualLevel, i, mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.EnemyList[i].viewID, "Level_" + actualLevel, mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.EnemyList[i].enemyPath);
		}

		//this.HidePlayerOnOtherLevel(nPlayer, actualLevel);
		/*if(nPlayer.ToString() == "0")
		{
			Vector3 newPos = new Vector3(10f + 250 * (actualLevel + 1), 1f, 10f + 250 * (actualLevel + 1));
			Utils.player.transform.position = newPos;
			//this.SendMap(serializedMap, mapGenerator.depth, actualLevel, serializedChest);
		}
		else
		{
			GetComponent<NetworkView>().RPC ("SendMap", nPlayer, serializedMap, mapGenerator.depth, actualLevel, nPlayer.ToString());
		}

		if(nPlayer == Network.player)
		{
			for(int i = 0; i < mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.Size; i++)
			{
				this.PlaceEnemiesNetwork(mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.getPositionAt(i), "Enemigo", actualLevel, i, mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.EnemyList[i].viewID, "Level_" + actualLevel, mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.EnemyList[i].enemyPath);
			}
		}
		else
		{
			for(int i = 0; i < mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.Size; i++)
			{
				GetComponent<NetworkView>().RPC ("PlaceEnemiesNetwork", nPlayer, mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.getPositionAt(i), "Enemigo", actualLevel, i, mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.EnemyList[i].viewID, "Level_" + actualLevel, mapGenerator.MazmorraCompleta[actualLevel].enemyDatabase.EnemyList[i].enemyPath);
			}
		}*/
	}
	
	
	
	
	public void resetActual()
	{
		GameObject dungeon = GameObject.Find ("Dungeon");
		
		if(Network.isServer)
			this.SetIsInDungeon(Network.player, false, (int)Utils.DungeonType.NONE, -1);
		else
			GetComponent<NetworkView>().RPC("SetIsInDungeon", RPCMode.Server, Network.player, false, (int)Utils.DungeonType.NONE, -1);
		
		if(dungeon)
		{
			MapGenerator mg = dungeon.GetComponent<MapGenerator> ();
			mg.actual = 0;
			this.inBoss = false;
			this.isPlayerInDungeon = false;
		}
	}
	
	[RPC]
	void SendMap(byte[] map, int depth, int actual, string nPlayer)
	{
		Debug.Log ("MAPA RECIBIDO");
		GameObject  mapObject = GameObject.Find("Dungeon") as GameObject;
		
		if(mapObject)
		{
			
			//mapGenerator = mapObject.GetComponent<MapGenerator>();
			GameObject dungeonTerrain;
			GameObject instantiantedDungeon;
			
			if(mapGenerator.MazmorraCompleta == null || mapGenerator.depth != depth)
				mapGenerator.MazmorraCompleta = new Map[depth];
			
			
			mapGenerator.MazmorraCompleta[actual] = this.DeserializeMap(map);
			//mapGenerator.chestDatabase = this.DeserializeChestDatabase(chests);
			mapGenerator.actual = actual;
			mapGenerator.depth = depth;
			
			
			if (actual == 0)
			{
				instantiantedDungeon = GameObject.Find("Level_0") as GameObject;
				instantiantedDungeon.name = "Level_0";
				instantiantedDungeon.transform.position = new Vector3(250 * (actual + 1), 0, 250 * (actual + 1));
			}
			else
			{
				dungeonTerrain = Resources.Load<GameObject> ("DungeonTerrain");
				instantiantedDungeon = Instantiate(dungeonTerrain, new Vector3(250 * (actual + 1), 0, 250 * (actual + 1)), Quaternion.identity) as GameObject;
				instantiantedDungeon.name = "Level_" + actual;
			}
			instantiantedDungeon.transform.SetParent(mapObject.transform, false);
			mapGenerator.MazmorraCompleta[actual].terrainParent = instantiantedDungeon;
			mapGenerator.Render(mapGenerator.MazmorraCompleta[actual].mapa, instantiantedDungeon.transform);
			//		mapGenerator.Render(mapGenerator.MazmorraCompleta[actual].mapa);
			mapGenerator.SpawnChest(actual, instantiantedDungeon.transform);

			if(Network.player.ToString() == nPlayer)
			{
				Vector3 newPos = new Vector3(-8f + 250 * (actual + 1), 1f, 10f + 250 * (actual + 1));
				Utils.player.transform.position = newPos;
			}
		}
	}
	
	[RPC]
	void RequestEnemiesOnLevel(int level, NetworkMessageInfo info)
	{
		if(this.mapGenerator)
		{
			byte[] eDB = this.EnemiesDataBaseSerialize(mapGenerator.MazmorraCompleta[level].enemyDatabase);
			//GetComponent<NetworkView>().RPC("SendEnemiesOnLevel", info.sender, level, eDB);
		}
	}
	
	[RPC]
	void SendEnemiesOnLevel(int level, byte[] eDB)
	{
		EnemyDatabase enemyDB = this.DeserializeEnemyDB(eDB);
		
		for(int i = 0; i < enemyDB.EnemyList.Length; i++)
		{
			if(enemyDB.EnemyList[i].isDead)
				Destroy(mapGenerator.MazmorraCompleta[level].enemyDatabase.EnemyList[i].enemy);
		}
	}
	
	public void HideAndShowLevel(int lHide, int lShow)
	{
		GameObject mapObject = GameObject.Find ("Dungeon") as GameObject;
		
		if (mapObject != null)
		{
			MapGenerator mapGenerator = mapObject.GetComponent<MapGenerator>();
			if(mapGenerator.MazmorraCompleta[lHide] != null && mapGenerator.MazmorraCompleta[lShow] != null)
			{
				mapGenerator.MazmorraCompleta[lHide].terrainParent.SetActive(false);
				mapGenerator.MazmorraCompleta[lShow].terrainParent.SetActive(true);
			}
		}
	}
}
