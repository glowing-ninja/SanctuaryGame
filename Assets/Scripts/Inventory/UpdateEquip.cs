using UnityEngine;
using System.Collections;

public class UpdateEquip : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		GetItems();
	}

	public void GetItems() {
		for (int i = 0; i < transform.childCount - 2; i++) {
			transform.GetChild(i).GetComponent<EquipItemScript>().UpdateEquip();
		}
	}
}
