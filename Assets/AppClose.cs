﻿using UnityEngine;
using System.Collections;

public class AppClose : MonoBehaviour {

	public void Close() {
		if (Utils.player != null)
			GameObject.Find("BD").GetComponent<SQLite>().updatePlayer();
		Application.Quit();
	}
}
