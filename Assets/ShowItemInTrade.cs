using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ShowItemInTrade : MonoBehaviour {
	
	public GameObject itemToolTip;
	public Item item;
	
	GameObject itp;

	Image img;
	
	
	// Use this for initialization
	void Awake () {
		item = null;
		img = transform.GetChild(0).GetComponent<Image>();
	}
	
	void Update() {
		if (GameObject.Find(Utils.objectPlayerName) != null) {
			if (item == null) {
				img.enabled = false;
			}
			else {
				img.enabled = true;
				img.sprite = item.icon;
			}
		}
	}
	
	public void Show() {
		if (item != null) {
			Vector3 pos = transform.position;
			pos.y += 150;
			pos.x -= 100;
			itp = Instantiate (itemToolTip, pos, Quaternion.identity) as GameObject;
			//itp.transform.SetParent(inventory.transform, true);
			itp.GetComponent<ItemToolTip>().Show(item, false);
		}
	}
	public void Hide() {
		if (item != null) {
			Destroy(itp);
		}
	}
	
	public void Equipar() {

	}
}
