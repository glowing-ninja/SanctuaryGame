using UnityEngine;
using System.Collections;

public class PlayerDataClass
{
	//_____Variables start______
	private GameObject playerGameObject;
	private Utils.DungeonType currentDungeon;
	private int currentLevel;
	private NetworkPlayer networkPlayer;
	//_____Variables end______

	public PlayerDataClass(GameObject player, NetworkPlayer nPlayer, Utils.DungeonType dungeon = Utils.DungeonType.NONE, int level = -1)
	{
		this.playerGameObject = player;
		this.currentDungeon = dungeon;
		this.currentLevel = level;
		this.networkPlayer = nPlayer;
}

	public override string ToString() 
	{
		return "GameObjectName: " + this.playerGameObject.name + " CurrentDungeon: " + this.currentDungeon + 
			" CurrentLevel: " + this.currentLevel + " NetworkPlayer: " + this.networkPlayer.ToString() + "PlayerName: " + this.playerGameObject.GetComponent<PlayerName>().playerRealName;
	}

	
	public NetworkPlayer NetworkPlayer 
	{
		get 
		{
			return networkPlayer;
		}
	}

	public int CurrentLevel {
		get {
			return currentLevel;
		}
		set {
			currentLevel = value;
		}
	}

	public Utils.DungeonType CurrentDungeon {
		get {
			return currentDungeon;
		}
		set {
			currentDungeon = value;
		}
	}

	public GameObject PlayerGameObject {
		get {
			return playerGameObject;
		}
	}
}
