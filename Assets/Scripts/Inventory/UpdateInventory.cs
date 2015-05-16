using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateInventory : MonoBehaviour {

	void OnEnable() {
		generateSlots inventory = transform.parent.GetComponent<generateSlots> ();
		for (int i = 1; i <= inventory.total_slots; i++) {
			if (inventory.inventory[i-1] != null) {
				transform.GetChild(i).GetComponent<ShowItemOnOver>().i = i-1;
				transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().enabled = true;
				transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = inventory.inventory[i-1].icon;
			}
		}
		transform.GetChild (0).GetComponent<Text> ().text = inventory.Gold + "";
	}
}
