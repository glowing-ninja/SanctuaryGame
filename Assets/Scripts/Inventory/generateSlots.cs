using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class generateSlots : MonoBehaviour {

	public GameObject slot;
	public int itemPorFila = 6;
	public int total_slots = 18;
	public Item[] inventory;
	public Image inventoryPanel;

	public int Gold = 0;

	void Awake () {
		GameObject it;
		Vector3 pos;
		inventory = new Item[total_slots];
		Debug.Log("GENERADO INVENTARIO");
		
		for (int i = 0; i < total_slots / itemPorFila; i++ ) {
			for (int j = 0; j < itemPorFila; j++ ) {
				it = Instantiate (slot) as GameObject;
				it.transform.position = new Vector3 (30 + j * 48, - 50 - i * 48, 0);
				it.transform.SetParent(inventoryPanel.transform, false);
			}
		}
	}

	public void AddGold(int g) {
		if (Gold < 999999) {
			if (Gold + g > 999999) {
				Gold = 999999;
			}
			else {
				Gold += g;
			}
			if (inventoryPanel.IsActive())
				inventoryPanel.transform.GetChild(0).GetComponent<Text>().text = Gold + "";
		}
	}

	public bool Add(Item i) {
		if (getCount() < total_slots) {
			for (int j = 0; j < total_slots; j++) {
				if (inventory[j] == null) {
					inventory[j] = i;
					if (inventoryPanel.IsActive())
						inventoryPanel.transform.GetChild(j+1).GetComponent<ShowItemOnOver>().i = j;
					j = total_slots + 1;
				}
			}
			return true;
		}
		return false;
	}

	public int getCount() {
		int cont = 0;
		for (int i = 0; i < total_slots; i++ ) {
			if (inventory[i] != null)
				cont++;
		}
		return cont;
	}
}
