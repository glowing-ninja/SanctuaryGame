using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SpawnDesgloseStat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Utils.Stat st;
	public GameObject desgloseWindow;
	private GameObject inst;

	public void OnPointerEnter(PointerEventData data) {
		inst = Instantiate (desgloseWindow) as GameObject;
		inst.transform.SetParent (transform.parent.parent.parent.parent, false);
		inst.GetComponent<StatDesglose> ().ShowStatDesglose (st);
	}
	
	public void OnPointerExit(PointerEventData data) {
		Destroy (inst);
	}
}
