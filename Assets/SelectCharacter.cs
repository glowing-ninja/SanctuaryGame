using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectCharacter : MonoBehaviour, IPointerDownHandler {
	public GameObject p1;
	public GameObject p2;
	public GameObject p3;

	public bool selected;
	public Color selectedColor;
	public Color normalColor;

	public InputField playerName;
	public Button btCrear;

	public GameObject nuevoPJ;
	public GameObject crearServidor;

	void Start () {
		if (transform.GetChild (0).GetComponent<Text> ().text != "Nuevo") {
			GetComponent<Image> ().color = normalColor;
		}
		/*if (selected)
			SelectThis ();*/
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (transform.GetChild (0).GetComponent<Text> ().text != "Nuevo") {
			if (p1.GetComponent<EventTrigger>().isActiveAndEnabled)
				p1.GetComponent<Image> ().color = normalColor;
			if (p2.GetComponent<EventTrigger>().isActiveAndEnabled)
				p2.GetComponent<Image> ().color = normalColor;
			if (p3.GetComponent<EventTrigger>().isActiveAndEnabled)
				p3.GetComponent<Image> ().color = normalColor;
			
			this.GetComponent<Image> ().color = selectedColor;

			playerName.text = transform.GetChild (0).GetComponent<Text> ().text;
			btCrear.interactable = true;
		} else {
			playerName.text = "";
			nuevoPJ.SetActive(true);
			crearServidor.SetActive(false);
		}
	}


	/*public void SelectThis () {
		if (p1.GetComponent<EventTrigger>().isActiveAndEnabled)
			p1.GetComponent<Image> ().color = normalColor;
		if (p2.GetComponent<EventTrigger>().isActiveAndEnabled)
			p2.GetComponent<Image> ().color = normalColor;
		if (p3.GetComponent<EventTrigger>().isActiveAndEnabled)
			p3.GetComponent<Image> ().color = normalColor;

		this.GetComponent<Image> ().color = selectedColor;

		if (transform.GetChild (0).GetComponent<Text> ().text != "Nuevo") {
			playerName.text = transform.GetChild (0).GetComponent<Text> ().text;
			playerName.interactable = false;
			btCrear.interactable = true;
		} else {
			playerName.text = "";
			playerName.interactable = true;
		}
	}*/
}
