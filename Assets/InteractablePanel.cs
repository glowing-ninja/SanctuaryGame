using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InteractablePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public void OnPointerEnter(PointerEventData data) {
		Color temp = Color.gray;
		temp.a = 0.5f;
		GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, temp, 0.2f);
	}
	
	public void OnPointerExit(PointerEventData data) {
		Color temp = Color.white;
		temp.a = 0.5f;
		GetComponent<Image>().color = temp;
	}
}
