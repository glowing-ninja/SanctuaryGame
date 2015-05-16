using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ShowItemOnOver : MonoBehaviour {

	public GameObject itemToolTip;
	private GameObject inventory;

	GameObject itp;
	
	public int i = -1;
	Image img;


	// Use this for initialization
	void Awake () {
		i = -1;
		inventory = GameObject.Find("InventoryPanel");
		img = transform.GetChild(0).GetComponent<Image>();
	}

	void Update() {
		if (GameObject.Find(Utils.objectPlayerName) != null) {
			if (i == -1) {
				img.enabled = false;
			}
			else {
				img.enabled = true;
				Item item = inventory.GetComponent<generateSlots>().inventory[i];
				img.sprite = item.icon;
			}
		}
	}

	public void Show() {
		if (i != -1) {
			Vector3 pos = transform.position;
			pos.y += 150;
			pos.x -= 100;
			itp = Instantiate (itemToolTip, pos, Quaternion.identity) as GameObject;
			itp.transform.SetParent(inventory.transform, true);
			Item item = inventory.GetComponent<generateSlots>().inventory[i];
			itp.GetComponent<ItemToolTip>().Show(item, false);
		}
	}
	public void Hide() {
		if (i != -1) {
			Destroy(itp);
		}
	}

	public void Equipar() {
		if (i != -1) {
			//if (GameObject.Find(PlayerPrefs.GetString("playerName")).GetComponent<Attributtes>().Equipar(itemPrev))
			//	Destroy(itp);
			//else
			//	i = itemPrev;

			if (Utils.inventoryClick == Utils.ClickSystem.TRADE) {
				int itemPrev = i;
				i = -1;
				if (Utils.player.GetComponent<Attributtes>().Equipar(itemPrev))
					Destroy(itp);
				else
					i = itemPrev;
			}
			else if (Utils.inventoryClick == Utils.ClickSystem.SELL) {
				GameObject.Find("SellPanel").GetComponent<EnableSellWindow>().preparacionVender(i); 
			}
			else if (Utils.inventoryClick == Utils.ClickSystem.EQUIP) {
				GameObject.Find("TradeBG").transform.GetChild(0).GetComponent<ShowItemOnOver>().i = i;
				GameObject.Find("TradeBG").GetComponent<TradeHandler>().onChange(inventory.GetComponent<generateSlots>().inventory[i]);
			}

		}
	}
}
