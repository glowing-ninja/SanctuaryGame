using UnityEngine;
using System.Collections;

public class NetworkLevelLoading : MonoSingleton<NetworkLevelLoading>
{

	public string[] supportedNetworkLevels  = {"Escena1", "Escena2"};
	public string disconnectedLevel  = "Escena1";
	public int lastLevelPrefix = 0;
	public string currentLevel = "";

	override public void Init()
	{
		GetComponent<NetworkView>().group = 1;
	}

	public void LoadNewLevel(string level)
	{
		if (Network.isServer) {
		//	this.lastLevelPrefix++;
		//	this.currentLevel = level;

			GetComponent<NetworkView>().RPC ("LoadLevel", RPCMode.AllBuffered, level, this.lastLevelPrefix);
		}
	}

	public void LoadNewAdditiveLevel(string level, NetworkPlayer nPlayer, int nextDungeon, bool bossRoom)
	{
		if (Network.isServer) {


			LevelManager lManager = GameObject.Find("GameManager").GetComponent<LevelManager>();

			if(lManager.currentDungeon == Utils.DungeonType.NONE)
			{

				GetComponent<NetworkView>().RPC ("LoadLevelAdditive", RPCMode.All, level, this.lastLevelPrefix, nPlayer, nextDungeon, bossRoom);
			}
			else 
			if(nPlayer != Network.player)
			{
				GetComponent<NetworkView>().RPC ("LoadLevelAdditive", nPlayer, level, this.lastLevelPrefix, nPlayer, nextDungeon, bossRoom);
			}
			else
			{
				StartCoroutine( this.LoadLevelAdditive(level, this.lastLevelPrefix, nPlayer, nextDungeon, bossRoom));
			}
		}
	}

	[RPC]
	public void LoadNewAdditiveLevelOnServer(string level, NetworkPlayer nPlayer, int nextDungeon, bool bossRoom)
	{
		if (Network.isServer)
		{

			
			GetComponent<NetworkView>().RPC ("LoadLevelAdditive", RPCMode.All, level, this.lastLevelPrefix, nPlayer, nextDungeon, bossRoom);
		}
	}

	[RPC]
	IEnumerator LoadLevel(string level, int levelPrefix)
	{
		this.lastLevelPrefix = levelPrefix;
		this.currentLevel = level;

		// No enviamos mas datos en el canal por defecto,
		// al cargar el nivel muchos objetos seran borrados
		Network.SetSendingEnabled(0, false);

		// Paramos de recivir, primero debemos cargar el nivel.
		// Una vez cargado, podemos usar RPC y otros metodos
		Network.isMessageQueueRunning = false;

		//Todas las network viewas cargadas en el nivel tendran un prefijo en su NetworkViewID
		//Esto previene que las updates antiguas de los clientes se cuelen en la nueva escena
		Network.SetLevelPrefix(levelPrefix);
		Application.LoadLevel(level);
		yield return 0;
		yield return 0;

		//Permitir el trafico de datos de nuevo
		Network.isMessageQueueRunning = true;

		//Una vez cargado podemos empezar a enviar datos a los clientes
		Network.SetSendingEnabled(0, true);

		//Puede que sea necesario hacer cambios en algunos gameObject al cargar
		//un nuevo nivel
		GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach(GameObject go in objects)
		{
			go.SendMessage("OnNetworkLoadedLevel", Network.player,SendMessageOptions.DontRequireReceiver);
		}
	}

	[RPC]
	IEnumerator LoadLevelAdditive(string level, int levelPrefix, NetworkPlayer nPlayer, int nextDungeon, bool bossRoom)
	{
		this.lastLevelPrefix = levelPrefix;
		this.currentLevel = level;
		
		// No enviamos mas datos en el canal por defecto,
		// al cargar el nivel muchos objetos seran borrados
		Network.SetSendingEnabled(0, false);
		
		// Paramos de recivir, primero debemos cargar el nivel.
		// Una vez cargado, podemos usar RPC y otros metodos
		Network.isMessageQueueRunning = false;
		
		//Todas las network viewas cargadas en el nivel tendran un prefijo en su NetworkViewID
		//Esto previene que las updates antiguas de los clientes se cuelen en la nueva escena
		Network.SetLevelPrefix(levelPrefix);

		if(!bossRoom)
		{
			GameObject.Find("GameManager").GetComponent<LevelManager>().currentDungeon = (Utils.DungeonType) nextDungeon;
			Application.LoadLevelAdditive(level);
		}
		else
		{
			LevelManager lManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
			if(!lManager.bossLoaded)
				Application.LoadLevelAdditive(level);

			lManager.bossLoaded = true;

			if(Network.isServer)
				GetComponent<NetworkView>().RPC("EnterBossLevel", RPCMode.All);
		}
		yield return 0;
		yield return 0;
		
		//Permitir el trafico de datos de nuevo
		Network.isMessageQueueRunning = true;
		
		//Una vez cargado podemos empezar a enviar datos a los clientes
		Network.SetSendingEnabled(0, true);
		
		//Puede que sea necesario hacer cambios en algunos gameObject al cargar
		//un nuevo nivel
		GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach(GameObject go in objects)
		{

			go.SendMessage("OnNetworkLoadedLevel", nPlayer,SendMessageOptions.DontRequireReceiver);
		}
	}


	[RPC]
	void EnterBossLevel()
	{
		MapGenerator mapGenerator = GameObject.Find("Dungeon").GetComponent<MapGenerator>();
		int depth = mapGenerator.depth + 1;
		mapGenerator.actual = depth;
		Utils.player.transform.position = new Vector3(250f * (depth), 1, 250f * depth - 60f);
	}

	void OnDisconnectedFromServer ()
	{
		Destroy(GameObject.Find("GameManager"));
		Destroy(GameObject.Find("SpawnManager"));
		Destroy(GameObject.Find("Canvas"));
		Destroy(GameObject.Find("Main Camera"));
		Destroy(GameObject.Find("EventSystem"));
		Application.LoadLevel(this.disconnectedLevel);
	}
}
