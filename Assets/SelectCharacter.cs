using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectCharacter : MonoBehaviour {
	public GameObject p1;
	public GameObject p2;
	public GameObject p3;

	public bool selected;

	public InputField playerName;

	void Start () {
		if (selected)
			SelectThis ();
	}

	public void SelectThis () {
		p1.GetComponent<Image> ().color = Color.white;
		p2.GetComponent<Image> ().color = Color.white;
		p3.GetComponent<Image> ().color = Color.white;

		this.GetComponent<Image> ().color = Color.blue;

		if (transform.GetChild (0).GetComponent<Text> ().text != "Nuevo") {
			playerName.text = transform.GetChild (0).GetComponent<Text> ().text;
			playerName.interactable = false;
		} else {
			playerName.text = "";
			playerName.interactable = true;
		}
	}
}
