using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KickPlayers : MonoBehaviour {

	//_____Variables start______
	private MultiplayerScript mScript;
	private PlayerDataBase playerDB;
	//_____Variables end______



	// Use this for initialization
	void Start ()
	{
		GameObject mManager = GameObject.Find("MultiplayerManager") as GameObject;
		this.mScript = mManager.GetComponent<MultiplayerScript>();
		this.playerDB = mManager.GetComponent<PlayerDataBase>();
	}
	
	public void FillMenu(GameObject kickMenu)
	{
		int index = 0;
		GameObject auxGO;

		for(index = 1; index < this.playerDB.playersList.Count; index++)
		{
			auxGO = kickMenu.transform.FindChild("InfoPlayer" + index ).gameObject;
			auxGO.SetActive(true);
			auxGO.transform.FindChild("PlayerName").GetComponent<Text>().text = playerDB.playersList[index].PlayerGameObject.GetComponent<PlayerName>().playerRealName;
			auxGO.transform.FindChild("ID").GetComponent<Text>().text = index.ToString();
		}

		for(index = index; index < 4; index++)
		{
			kickMenu.transform.FindChild("InfoPlayer" + index).gameObject.SetActive(false);
		}
	}

	public void KickPlayerAt(GameObject index)
	{
		this.mScript.KickPlayer(int.Parse(index.GetComponent<Text>().text));
	}

	public void Disconnect()
	{
		if(Network.isServer)
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}
		else
			Network.Disconnect();
		
	}

}
