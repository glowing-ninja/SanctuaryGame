using UnityEngine;
using System.Collections;

public class OpenMenu : MonoBehaviour {

	public Transform menuImage;
	public bool opened = false;

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (opened) {
				menuImage.gameObject.SetActive(false);
				opened = false;
			}
			else if (!opened) {
				menuImage.gameObject.SetActive(true);
				opened = true;
			}
		}
	}
}
