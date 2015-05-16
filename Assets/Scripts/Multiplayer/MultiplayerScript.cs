using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// This script is attached to the MultiplayerManager
/// it is the foundation for our multiplayer system
/// 
/// This script is accessed by the CursorControl script
/// </summary>



public class MultiplayerScript : MonoSingleton<MultiplayerScript> {

	//_____Variables start______
	private bool useNAT = false;
	private int port;
	private string ipAddress;
	private int numberOfPlayers = 3;
	public string playerName;
	public string serverName;
	public string serverNameForClient;
	private PlayerDataBase playerDB;
	private int playerIndex = 0;
	//_____Variables end______

	void Start()
	{
		playerDB = GameObject.Find("MultiplayerManager").GetComponent<PlayerDataBase>();

	}

	public void crearPartidaOffline () {
		string name = GameObject.Find ("NuevoPJ").transform.Find ("if_Nombre").transform.Find ("Text").GetComponent<Text>().text;
		if (name.Equals("")) {
			playerName = "Player";
		} else
			playerName = name;
		
		PlayerPrefs.SetString("playerName", this.playerName);
		Utils.playerName = this.playerName;
		
		//Create server
		string port = GameObject.Find ("NuevoPJ").transform.Find ("if_Puerto").transform.Find ("Text").GetComponent<Text>().text;
		this.port = int.Parse(port);
		Network.InitializeServer(this.numberOfPlayers, this.port, this.useNAT);
		
		//Save the serverName using PlayerPrefs
		PlayerPrefs.SetString("serverName", serverName);
		
		GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewLevel ("4_Tutorial");
	}

	public void crearServidor () {
		//Ensure the player can't join a game with an empty name
		string name = GameObject.Find ("CrearServidor").transform.Find ("if_Nombre").transform.Find ("Text").GetComponent<Text>().text;
		if (name.Equals("")) {
			playerName = "Player";
		} else
			playerName = name;
		
		PlayerPrefs.SetString("playerName", this.playerName);
		Utils.playerName = this.playerName;

		//Create server
		string port = GameObject.Find ("CrearServidor").transform.Find ("if_Puerto").transform.Find ("Text").GetComponent<Text>().text;
		this.port = int.Parse(port);
		Network.InitializeServer(this.numberOfPlayers, this.port, this.useNAT);
		
		//Save the serverName using PlayerPrefs
		PlayerPrefs.SetString("serverName", serverName);

		GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewLevel ("1_MapaInicial");
	}

	public void unirseServidor () {
		string name = GameObject.Find ("UnirseServidor").transform.Find ("if_Nombre").transform.Find ("Text").GetComponent<Text>().text;
		if (name.Equals("")) {
			playerName = "Player";
		} else
			playerName = name;
		
		PlayerPrefs.SetString("playerName", this.playerName);
		Utils.playerName = this.playerName;
		
		string port = GameObject.Find ("UnirseServidor").transform.Find ("if_Puerto").transform.Find ("Text").GetComponent<Text>().text;
		this.port = int.Parse(port);
		string ip = GameObject.Find ("UnirseServidor").transform.Find ("if_Ip").transform.Find ("Text").GetComponent<Text>().text;
		this.ipAddress = ip;

		Network.Connect (this.ipAddress, this.port);

	}

	void OnDisconnectedFromServer()
	{
		//If a player loses the connection or leaves the scen then level is restarted on their computer

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for(int i = 0; i < players.Length; i++)
		{
			Destroy (players[i]);
		}
	}

	void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		GetComponent<NetworkView>().RPC ("TellPlayerServerName", networkPlayer, this.serverName);


	}

	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		//When the player leaves the server delete them across the network along with their RPCs
		//so that other players no longer see them
		LevelManager lManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
		PlayerDataClass playerDC = playerDB.GetData(networkPlayer);
		GetComponent<NetworkView>().RPC ("UpdateUI", RPCMode.All, playerDC.PlayerGameObject.name);

		if(playerDC.CurrentDungeon != Utils.DungeonType.NONE)
		{
			lManager.DecrementPlayers();
		}

		gameObject.GetComponent<PlayerDataBase>().RemoveEntry(networkPlayer);
		Network.RemoveRPCs (networkPlayer);
		Network.DestroyPlayerObjects (networkPlayer);
	}




	[RPC]
	void UpdateUI(string playerName)
	{
		getPlayers gP = GameObject.Find ("Canvas").GetComponent<getPlayers> ();
		gP.DeletePlayerUI(playerName);
	}

	//Used to tell the MultiplayerScript in connected players the server name.
	//Otherwise players connecting wouldn't be able to see the name of the server.
	[RPC]//Buscar que es esto
	void TellPlayerServerName(string servername)
	{
		this.serverName = servername;
	}


	void HidePlayerOnOtherLevel(NetworkPlayer nPlayer, int level)
	{
		PlayerDataBase playerDB = gameObject.GetComponent<PlayerDataBase>();
		List<PlayerDataClass> pList = playerDB.playersList;
		int size = pList.Count;
		PlayerDataClass pData = null;
		GameObject playerGameObject = null;

		//Buscamos el gameobject asociado al networkPlayer indicado
		for(int i = 0; i < size; i++)
		{
			pData = pList[i];
			if(pData.NetworkPlayer.ToString() == nPlayer.ToString())
			{
				playerGameObject = pList[i].PlayerGameObject;
			}
		}

	
		for(int i = 0; i < size; i++)
		{
			pData = pList[i];
			if(pData.NetworkPlayer.ToString() != nPlayer.ToString())
			{
				if(pData.CurrentLevel != level)
				{
					
					if(playerGameObject.GetComponent<NetworkView>().owner.ToString() == "0")
					{
						pData.PlayerGameObject.SetActive(false);
					}
					else
					{
						GetComponent<NetworkView>().RPC("SetPlayerEnable", playerGameObject.GetComponent<NetworkView>().owner, pData.NetworkPlayer, false, pData.CurrentLevel + 1);
					}
					
					if(pData.NetworkPlayer.ToString() == "0")
					{
						playerGameObject.SetActive(false);
						
					}
					else
					{
						GetComponent<NetworkView>().RPC("SetPlayerEnable", pData.NetworkPlayer, playerGameObject.GetComponent<NetworkView>().owner, false, level + 2);
					}
				}
				else
				{

					if(playerGameObject.GetComponent<NetworkView>().owner.ToString() == "0")
					{
						pData.PlayerGameObject.SetActive(true);
					}
					else
					{
						GetComponent<NetworkView>().RPC("SetPlayerEnable", playerGameObject.GetComponent<NetworkView>().owner, pData.NetworkPlayer, true, pData.CurrentLevel + 1);
					}
					
					if(pData.NetworkPlayer.ToString() == "0")
					{
						playerGameObject.SetActive(true);
					}
					else
					{
						GetComponent<NetworkView>().RPC("SetPlayerEnable", pData.NetworkPlayer, playerGameObject.GetComponent<NetworkView>().owner, true, level + 2);
					}
				}
			}
		}
	}

	[RPC]
	void SetPlayerEnable(NetworkPlayer nPlayer, bool enable, int level)
	{
		PlayerDataBase playerDB = gameObject.GetComponent<PlayerDataBase>();
		List<PlayerDataClass> pList = playerDB.playersList;
		int size = pList.Count;
		PlayerDataClass pData = null;

		//Buscamos el gameobject asociado al networkPlayer indicado
		for(int i = 0; i < size; i++)
		{
			pData = pList[i];
			if(pData.NetworkPlayer.ToString() == nPlayer.ToString())
			{
				pData.PlayerGameObject.SetActive(enable);
			}
		}
	}


		
	[RPC]
	public void serversUpdateMovement(NetworkPlayer nPlayer, int currentLevel, Vector3 newPosition, Quaternion newRotation)
	{
		PlayerDataClass playerToMove = null;

		for(int i = 0; i < this.playerDB.playersList.Count; i++)
		{
			if(nPlayer.ToString() == this.playerDB.playersList[i].NetworkPlayer.ToString())
			{
				playerToMove = this.playerDB.playersList[i];
				i = this.playerDB.playersList.Count + 1;
			}
		}

		if(playerToMove != null)
		{
			for(int i = 0; i < this.playerDB.playersList.Count; i++)
			{
				if(nPlayer.ToString() != this.playerDB.playersList[i].NetworkPlayer.ToString() && currentLevel == this.playerDB.playersList[i].CurrentLevel)
				{
					if(playerDB.playersList[i].NetworkPlayer.ToString() == "0" && Network.isServer)
					{
						playerToMove.PlayerGameObject.transform.position = newPosition;
						playerToMove.PlayerGameObject.transform.rotation = newRotation;
					}
					else
					{
						playerToMove.PlayerGameObject.GetComponent<NetworkView>().RPC ("updateMovement", playerDB.playersList[i].NetworkPlayer, 
						                 newPosition, newRotation);
					}
				}
			}
		}
	}

	public int AllocatePlayerIndex()
	{
		this.playerIndex++;
		return this.playerIndex - 1;
	}

	public void KickPlayer(int index)
	{
		Network.CloseConnection(playerDB.playersList[index].NetworkPlayer, false);
	}
}
