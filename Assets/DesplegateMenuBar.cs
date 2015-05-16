using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DesplegateMenuBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	
	public void OnPointerEnter(PointerEventData data) {
		transform.Translate (new Vector3 (-52, 0f));
	}
	
	public void OnPointerExit(PointerEventData data) {
		transform.Translate (new Vector3 (52f, 0f));
	}
}
