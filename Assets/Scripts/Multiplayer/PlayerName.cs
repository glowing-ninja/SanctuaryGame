using UnityEngine;
using System.Collections;

/// <summary>
/// Player name.
/// This script incorporates the player's name into the game
/// This scrpit is attached to the player
/// </summary>

public class PlayerName : MonoBehaviour {


	//_____Variables start______
	public string playerName;
	public string playerRealName = "";
	private PlayerDataBase playerDatabase;
	private int positioNDB = -1;
	private bool setPlayerFlag = false;
	//_____Variables end______

	void Start()
	{

		GameObject mManager = GameObject.Find("MultiplayerManager") as GameObject;
		MultiplayerScript mScript = mManager.GetComponent<MultiplayerScript>();
		playerDatabase = mManager.GetComponent<PlayerDataBase>();

		if(GetComponent<NetworkView>().isMine)
		{
			Camera.main.GetComponent<TopDownCamera>().target = this.transform;
			this.playerRealName = Utils.playerName;
			GetComponent<NetworkView>().RPC("SendRealName", RPCMode.AllBuffered, Utils.playerName);
		}

		if(Network.isServer)
		{
			gameObject.name = "Player" + mScript.AllocatePlayerIndex();
			GetComponent<NetworkView>().RPC("UpdateMyNameEverywhere", RPCMode.AllBuffered, gameObject.name);
		}
		//When the player spawns into the game retrieve their name from
		//PlayerPrefs and ensure this name is not the same as any other
		//players name
		/*if(networkView.isMine)
		{
			Camera.main.GetComponent<TopDownCamera>().target = this.transform;

			this.playerName = PlayerPrefs.GetString("playerName");

			//Check if any players in the game already have the same name
			//if they do assign a random number as their name and save it
			//Cambiar, no me gusta
			GameObject[] objs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
			foreach(GameObject objNameCheck in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if(this.playerName == objNameCheck.name)
				{
					float randomNumber = Random.Range(0, 1000);
					this.playerName = "(" + randomNumber.ToString() + ")";

					PlayerPrefs.SetString("playerName", this.playerName);
				}
			}
			Utils.objectPlayerName = this.playerName.Clone().ToString();
			this.playerName = "-1";

			//Send out an RPC to ensure this player's name is slapped onto 
			//their GameObject accross the network. This will be important
			//for hit detection
			networkView.RPC("UpdateMyNameEverywhere", RPCMode.AllBuffered, Utils.objectPlayerName);
			gameObject.name = Utils.objectPlayerName;
			StartCoroutine("sendName", Utils.objectPlayerName);
		}*/


	}

	/*void UpdateLocalGameManager(string pName)
	{
		//Tell the PlayerDatabase script to append this player's name to the list
		GameObject gameManager = GameObject.Find("GameManager");
		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();

		dataScript.nameSet = true;
		dataScript.playerName = pName;
	}*/

	[RPC]
	void UpdateMyNameEverywhere(string pName)
	{
		//Change the player's gameObject name to their actual player name
		gameObject.name = pName;

		if(playerDatabase == null)
		{
			GameObject mManager = GameObject.Find("MultiplayerManager") as GameObject;
			MultiplayerScript mScript = mManager.GetComponent<MultiplayerScript>();
			playerDatabase = mManager.GetComponent<PlayerDataBase>();
		}

		this.positioNDB = playerDatabase.AddNewEntry(this.gameObject, this.gameObject.GetComponent<NetworkView>().owner);

		if(!GetComponent<NetworkView>().isMine)
		{
			getPlayers gP = GameObject.Find ("Canvas").GetComponent<getPlayers> ();
			//gP.players.Add (gameObject);
			gP.actualizarUI (this.gameObject);
			if(this.playerRealName == "")
			{
				this.setPlayerFlag = true;
			}
		}
		else
		{
			PlayerPrefs.SetString("playerName", pName);
			Utils.objectPlayerName = pName;
		}
	}

	[RPC]
	void SendRealName(string rName)
	{
		this.playerRealName = rName;
		if(this.setPlayerFlag)
		{
			this.setPlayerFlag = false;
		
			if(!GetComponent<NetworkView>().isMine)
			{
				getPlayers gP = GameObject.Find ("Canvas").GetComponent<getPlayers> ();
				//gP.players.Add (gameObject);
				gP.UpdateName (this.gameObject, rName);
			}

		}
	}

	IEnumerator sendName(string pName)
	{
		while(this.playerName != pName)
		{
			GetComponent<NetworkView>().RPC("UpdateMyNameEverywhere", RPCMode.AllBuffered, pName);
			yield return new WaitForSeconds(0.5f);
		}
	}
}
