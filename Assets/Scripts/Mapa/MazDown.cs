using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MazDown : MonoBehaviour {
	public bool mazDown;
	MapGenerator mg;
	Vector3 newPos;
	int actual;
	int depth;
	private LevelManager levelManager;

	void Start() 
	{
		this.levelManager = GameObject.Find ("GameManager").GetComponent<LevelManager> ();
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			if(other.GetComponent<NetworkView>().isMine)
			{
				levelManager.SwitchDungeonLevel(mazDown);
			}
		}
		
		/*actual = mg.actual;
		depth = mg.depth - 1;
			
		if (mazDown && actual < depth) {
				if (mg.MazmorraCompleta[actual +1] == null) {
					Debug.Log("NULL");
					mg.MazmorraCompleta[actual +1] = MapGenerator.mapGenerator();
				}
				actual++;
				newPos = new Vector3(10f, 1f, 10f);
			}
			if (mazDown && actual >= depth) {
				
				actual++;
				GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewLevel ("3_BossMap");
			}
			if (!mazDown && actual > 0) {
				actual--;
				newPos = new Vector3(190f, 1f, 190f);
			}
			if (!mazDown && actual == 0) {
				GameObject.Find ("LevelLoader").GetComponent<NetworkLevelLoading> ().LoadNewLevel ("1_MapaInicial");
			}
			mg.actual = actual;
			Debug.Log ("Actual: " + actual);
			StartCoroutine ("OscurecerPantalla");
			GameObject player = GameObject.Find(PlayerPrefs.GetString("playerName"));
			//player.networkView.RPC("SetEnable", RPCMode.Others, false);
		*/
	}
	

}
