using UnityEngine;
using System.Collections;

public class AutoSave : MonoBehaviour {

	public float time2wait = 10f;

	// Use this for initialization
	void Start () {
		StartCoroutine (Save ());
	}

	public IEnumerator Save () {
		SQLite bd = GameObject.Find ("BD").GetComponent<SQLite> ();
		while (true) {
			yield return new WaitForSeconds(time2wait);
			bd.updatePlayer();
		}
	}
}
