using UnityEngine;
using System.Collections;

public class enterMaz : MonoBehaviour {

	public Utils.DungeonType typeDungeon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player")
			if(other.gameObject.GetComponent<NetworkView>().isMine)
			{
				GameObject.Find("GameManager").GetComponent<LevelManager>().EnterDungeon(typeDungeon);
				//GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewAdditiveLevel ("2_GameField", Network.player);
			}
	}
}
