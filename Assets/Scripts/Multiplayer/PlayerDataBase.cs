using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDataBase : MonoBehaviour {

	//_____Variables start______
	public List<PlayerDataClass> playersList;
	//Used for debug
	public bool showData = false;
	//_____Variables end______


	void Awake()
	{
		this.playersList = new List<PlayerDataClass>();
	}


	void Update()
	{
		if(this.showData)
		{
			for(int i = 0; i < this.playersList.Count; i++)
			{
				Debug.Log(this.playersList[i].ToString());
			}

			this.showData = false;
		}

	}

	public int AddNewEntry(GameObject player, NetworkPlayer nPlayer)
	{
		this.playersList.Add(new PlayerDataClass(player, nPlayer));

		return playersList.Count - 1;
	}

	public void RemoveEntry(NetworkPlayer nPlayer)
	{
		for(int i = 0; i < this.playersList.Count; i++)
		{
			if(this.playersList[i].NetworkPlayer.ToString() == nPlayer.ToString())
			{
				this.playersList.RemoveAt(i);
				i = this.playersList.Count + 1;
			}
		}
	}

	public void SetDungeon(NetworkPlayer nPlayer, Utils.DungeonType dungeon, int level)
	{
		for(int i = 0; i < this.playersList.Count; i++)
		{
			if(this.playersList[i].NetworkPlayer.ToString() == nPlayer.ToString())
			{
				this.playersList[i].CurrentDungeon = dungeon;
				this.playersList[i].CurrentLevel = level;
			}
		}
	}

	public PlayerDataClass GetData(NetworkPlayer nPlayer)
	{
		for(int i = 0; i < this.playersList.Count; i++)
		{
			if(this.playersList[i].NetworkPlayer.ToString() == nPlayer.ToString())
			{
				return this.playersList[i];
			}
		}

		return null;
	}

	public NetworkPlayer getNetworkPlayer (string playerName) {
		NetworkPlayer tmp = new NetworkPlayer();
		for(int i = 0; i < this.playersList.Count; i++)
		{
			if(this.playersList[i].PlayerGameObject.GetComponent<PlayerName>().playerRealName == playerName)
			{
				tmp = this.playersList[i].NetworkPlayer;
				i = this.playersList.Count + 1;
			}
		}
		return tmp;
	}

	public void RestartBD()
	{
		this.playersList = new List<PlayerDataClass>();
	}
}
