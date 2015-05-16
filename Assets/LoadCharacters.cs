using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadCharacters : MonoBehaviour {

	public GameObject p1;
	public GameObject p2;
	public GameObject p3;

	// Use this for initialization
	void OnEnable () {
		Utils.PlayerData[] players = GameObject.Find ("BD").GetComponent<SQLite> ().GetAllPlayers ();
		if (players [0] != null) {
			p1.transform.GetChild(0).GetComponent<Text>().text = players[0].name;
			p1.transform.GetChild(1).GetComponent<Text>().text = "Lv: " + players[0].level;
		}
		if (players [1] != null) {
			p2.transform.GetChild(0).GetComponent<Text>().text = players[1].name;
			p2.transform.GetChild(1).GetComponent<Text>().text = "Lv: " + players[1].level;
			
		}
		if (players [2] != null) {
			p3.transform.GetChild(0).GetComponent<Text>().text = players[2].name;
			p3.transform.GetChild(1).GetComponent<Text>().text = "Lv: " + players[2].level;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
