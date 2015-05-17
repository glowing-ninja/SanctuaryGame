using UnityEngine;
using System.Collections;

public class BuyRandomItem : MonoBehaviour {
	
	public GameObject itemToolTip;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonClick() {
		InteractablePanel ip = null;
		for (int i = 0; i < 4; i++) {
			if (transform.parent.GetChild(i).GetComponent<InteractablePanel>().selected) {
				ip = transform.parent.GetChild(i).GetComponent<InteractablePanel>();
				break;
			}
		}
		if (ip != null) {
			Attributtes atr = Utils.player.GetComponent<Attributtes>();
			if (ip.totalGold <= atr.inventory.GetComponent<generateSlots>().Gold) {
				Item item = Item.ItemGenerator(ip.type, ip.subType, atr.level, ip.rarity+1);
				if (atr.inventory.GetComponent<generateSlots>().Add(item)) {
					atr.addGold(-ip.totalGold);
					//atr.inventory.GetComponent<generateSlots>().AddGold(-ip.totalGold);
					Vector3 pos = new Vector3 (0f, -300f);
					GameObject itp = Instantiate (itemToolTip, pos, Quaternion.identity) as GameObject;
					itp.transform.SetParent (transform.parent, false);
					itp.GetComponent<ItemToolTip> ().Show (item, true);
				}
			}
		}
	}
}
