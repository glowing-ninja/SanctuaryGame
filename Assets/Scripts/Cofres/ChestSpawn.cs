using UnityEngine;
using System.Collections;

public class ChestSpawn : MonoBehaviour {
	
	public static GameObject chest;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void Spawn(int rarity, Vector3 pos, Transform parent) {
		if (rarity == 1) {
			chest = Resources.Load <GameObject>("Prefab/ChestCooper");
		} else if (rarity == 2) {
			chest = Resources.Load <GameObject>("Prefab/ChestSilver");
		} else {
			chest = Resources.Load <GameObject>("Prefab/ChestGold");
		}
		GameObject chestObject = Instantiate (chest, pos + new Vector3(0,0.4f,0), chest.transform.rotation) as GameObject;
		chestObject.transform.SetParent(parent, false);
	}
}
