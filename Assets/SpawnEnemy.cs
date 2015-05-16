using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

	public GameObject boss;

	public void bossSpawn () {
		Vector3 pos = new Vector3 (0f ,0f, -8.5f);
		GameObject bb = GameObject.Instantiate (boss, pos, Quaternion.identity) as GameObject;
		bb.transform.SetParent(gameObject.transform.parent.parent, false);
	}
}
