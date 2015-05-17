using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class selectColor : MonoBehaviour {
	public Color color;
	public bool padre = false;

	void Start () {
		if (padre) {
			Color a = new Color();
			for (int i = 1; i < transform.childCount; i++) {
				a = transform.GetChild(i).GetComponent<Image>().color;
				a.a = 0.5f;
				transform.GetChild(i).GetComponent<Image>().color = a;
			}
			transform.GetComponent<selectColor>().color = GetComponent<Image>().color;
		}
	}

	public void OnClick () {
		Color a = new Color();
		for (int i = 0; i < transform.parent.childCount; i++) {
			a = transform.parent.GetChild(i).GetComponent<Image>().color;
			a.a = 0.5f;
			transform.parent.GetChild(i).GetComponent<Image>().color = a;
		}
		a = GetComponent<Image>().color;
		a.a = 1f;
		GetComponent<Image>().color = a;
		transform.parent.GetComponent<selectColor>().color = GetComponent<Image>().color;
	}
}
