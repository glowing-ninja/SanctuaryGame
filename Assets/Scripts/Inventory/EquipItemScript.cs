using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EquipItemScript : MonoBehaviour {
	
	public GameObject itemToolTip;

	public Utils.ItemType type;
	public Equip equip;
	GameObject itp;

	public Item tmp;
	public Image img;
	
	
	void OnEnable () {
		img = transform.GetChild(0).GetComponent<Image>();
	}

	public void UpdateEquip() {
		//equip = GameObject.Find(PlayerPrefs.GetString("playerName")).GetComponent<Attributtes>().equipamiento;
		equip = transform.parent.parent.GetComponent<PlayerEquip> ().equipment;
		switch(this.type) {
		case Utils.ItemType.WEAPON :
			this.tmp = equip.weapon; break;
		case Utils.ItemType.CHEST:
			this.tmp = equip.chest; break;
		case Utils.ItemType.LEGS:
			this.tmp = equip.legs; break;
		case Utils.ItemType.BOOTS:
			this.tmp = equip.boots; break;
		default:
			this.tmp = null; break;
		}

		
		img = transform.GetChild(0).GetComponent<Image>();

		if (this.tmp == null) {
			img.enabled = false;
		}
		else {
			img.enabled = true;
			
			img.sprite = this.tmp.icon;
		}
	}
	
	public void Show() {
		if (this.tmp != null) {
			Vector3 pos = transform.position;
			pos.y += 150;
			itp = Instantiate (itemToolTip, pos, Quaternion.identity) as GameObject;
			itp.transform.parent = transform.parent.transform;
			itp.GetComponent<ItemToolTip>().Show(tmp, false);
		}
	}
	public void Hide() {
		if (this.tmp != null) {
			Destroy(itp);
		}
	}
	
}