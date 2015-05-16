using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{

	/*void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		if(Network.isServer)
		{
			GameObject mManager = GameObject.Find("MultiplayerManager") as GameObject;
			MultiplayerScript mScript = mManager.GetComponent<MultiplayerScript>();
			PlayerDataBase playerDatabase = mManager.GetComponent<PlayerDataBase>();
			gameObject.name = "Player" + mScript.AllocatePlayerIndex();
			networkView.RPC("UpdateMyNameEverywhere", RPCMode.AllBuffered, gameObject.name);
			playerDatabase.AddNewEntry(this.gameObject, this.gameObject.networkView.owner);
		}
	}

	[RPC]
	void UpdateMyNameEverywhere(string pName)
	{
		//Change the player's gameObject name to their actual player name
		gameObject.name = pName;

		if(!networkView.isMine)
		{
			getPlayers gP = GameObject.Find ("Canvas").GetComponent<getPlayers> ();
			gP.players.Add (gameObject);
			gP.actualizarUI ();
		}
		else
		{
			PlayerPrefs.SetString("playerName", pName);
			Utils.objectPlayerName = pName;
		}
	}*/
}
