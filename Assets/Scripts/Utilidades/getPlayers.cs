using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class getPlayers : MonoBehaviour {

	public GameObject otherPlayer;
	public List<PlayerDataClass> players;
	private List<GameObject> UIList;

	// Use this for initialization
	void Start () {

		this.UIList = new List<GameObject>();

		GameObject mManager = GameObject.Find("MultiplayerManager") as GameObject;
		MultiplayerScript mScript = mManager.GetComponent<MultiplayerScript>();
		players = mManager.GetComponent<PlayerDataBase>().playersList;
		//GameObject.Find(PlayerPrefs.GetString("playerName")).GetComponent<HealthTransform>().enabled = true;
		//GameObject.Find(PlayerPrefs.GetString("playerName")).GetComponent<ExperienceTransform>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	/*	GameObject[] pl = GameObject.FindGameObjectsWithTag ("Player") as GameObject[];
		foreach (GameObject go in pl) {
			if (!go.networkView.isMine && !players.Contains(go)) {
				players.Add(go);
				actualizarUI();
			}
		}
		//networkView.RPC("avisarConexion", RPCMode.AllBuffered);*/
	}

	void OnConnectedToServer() {

	}

	public void UpdateName(GameObject player, string name)
	{
		for(int i = 0; i < this.UIList.Count; i++)
		{
			if(this.UIList[i].name == player.name + "UI")
			{
				GameObject auxUI = UIList[i];
				Text auxText = auxUI.GetComponent<Text>();
				PlayerName pNameScript = player.GetComponent<PlayerName>();
				auxText.text = pNameScript.playerRealName;

				//UIList[i].GetComponent<Text>().text = player.GetComponent<PlayerName>().playerRealName;
			}
		}
	}

	public void actualizarUI(GameObject player)
	{
		GameObject auxUI = Instantiate(otherPlayer) as GameObject;
		auxUI.name = player.name + "UI";
		auxUI.GetComponent<Text>().text = player.GetComponent<PlayerName>().playerRealName;
		auxUI.transform.position = new Vector3(97f, -90f - 50f * this.UIList.Count, 0f);
		auxUI.transform.SetParent(this.transform, false);
		this.UIList.Add(auxUI);
	}

	public void actualizarUI () {

		for(int i = this.UIList.Count; i < this.players.Count; i++)
		{
			if(players[i].NetworkPlayer != Network.player)
			{
				GameObject auxUI = Instantiate(otherPlayer) as GameObject;
				auxUI.name = players[i].PlayerGameObject.name + "UI";
				auxUI.GetComponent<Text>().text = players[i].PlayerGameObject.GetComponent<PlayerName>().playerRealName;
				auxUI.transform.position = new Vector3(97f, -90f - 50f * i, 0f);
				auxUI.transform.SetParent(this.transform, false);
				this.UIList.Add(auxUI);
			}
		}

		/*foreach (PlayerDataClass playerDC in players) {
			GameObject i = Instantiate(otherPlayer) as GameObject;
			i.name = playerDC.PlayerGameObject.name + "UI";
			i.GetComponent<Text>().text = playerDC.PlayerGameObject.GetComponent<PlayerName>().playerRealName;
			i.transform.position = new Vector3(97f, -90f - 50f * iter, 0f);
			i.transform.SetParent(this.transform, false);
			iter++;
		}*/
	}

	public void DeletePlayerUI(string player)
	{
		bool deleted = false;
		int position = -1;

		for(int i = 0; i < this.UIList.Count; i++)
		{
			if(!deleted)
			{
				if(this.UIList[i].name == player + "UI")
				{
					deleted = true;
					position = i;
				}
			}
			else
			{
				this.UIList[i].transform.SetParent(null, false);
				this.UIList[i].transform.position = new Vector3(97f, -90f - 50f * (i - 1), 0f);
				this.UIList[i].transform.SetParent(this.transform, false);
			}
		}

		if(position != -1)
		{
			Destroy(this.UIList[position]);
			this.UIList.RemoveAt(position);
		}
	}


}
