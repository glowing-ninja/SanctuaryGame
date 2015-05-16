using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UiMenuScript : MonoBehaviour, IPointerClickHandler {

	public GameObject ui;
	public GameObject MainCamera;

	public void OnPointerClick (PointerEventData data) {
		if (ui.activeInHierarchy) {
			ui.SetActive(false);
			//Indicamos que se hemos abierto un panel
			this.MainCamera.GetComponent<CloseUI>().IsPanelOpen = false;
		}
		else {
			ui.SetActive(true);
			//Indicamos que hemos cerrado un panel
			this.MainCamera.GetComponent<CloseUI>().IsPanelOpen = true;
		}
	}
}
