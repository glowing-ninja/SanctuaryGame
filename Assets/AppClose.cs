using UnityEngine;
using System.Collections;

public class AppClose : MonoBehaviour {

	public void Close() {
		GameObject.Find("BD").GetComponent<SQLite>().updatePlayer();
		Application.Quit();
	}
}
