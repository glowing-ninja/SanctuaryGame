using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnableSellWindow : MonoBehaviour {

	public ShowItemOnOver item;
	public generateSlots gs;

	void OnEnable () {
		gs = GameObject.Find ("InventoryPanel").GetComponent<generateSlots> ();
		item = transform.FindChild ("SellSlot").GetComponent<ShowItemOnOver> ();
		Utils.inventoryClick = Utils.ClickSystem.SELL;
	}

	void OnDisable () {
		Utils.inventoryClick = Utils.ClickSystem.EQUIP;
	}
	
	public void preparacionVender (int i) {
		item.i = i;
		transform.FindChild ("lb_gold").GetComponent<Text> ().text = gs.inventory [item.i].gold + "";
	}

	public void ClickVender () {
		if (item.i != -1) {
			gs.AddGold (gs.inventory[item.i].gold);
			gs.inventory[item.i] = null;
			gs.gameObject.transform.GetChild(0).GetChild(item.i + 1).GetComponent<ShowItemOnOver>().i = -1;
			transform.FindChild ("lb_gold").GetComponent<Text> ().text = "0";
			item.i = -1;
		}
	}
}
