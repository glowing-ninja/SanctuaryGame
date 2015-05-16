using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour {
	public int rarity;
	private bool delete = false;
	public GameObject itemToolTip;
	private GameObject canvas;
	private GameObject player;

	// Use this for initialization


	void OnMouseDown() {
		player = GameObject.FindGameObjectWithTag ("Player");
		if (Vector3.Distance (transform.position, player.transform.position) < 5f) {
			if (!delete) {
				canvas = GameObject.Find("Canvas");
				delete = true;
				Attributtes playerStats = player.GetComponent<Attributtes> ();
				int lv = playerStats.level;
				Item i = Item.ItemGenerator (rarity, lv);

				this.GetComponent<Animator> ().SetBool ("Abrir", true);
				Vector3 pos = new Vector3 (Screen.width / 2f, Screen.height / 1.2f);
				GameObject itp = Instantiate (itemToolTip, pos, Quaternion.identity) as GameObject;
				itp.transform.SetParent (canvas.transform);
				itp.GetComponent<ItemToolTip> ().Show (i, true);
				DestroyObject(gameObject, 2f);

				GameObject.Find("InventoryPanel").GetComponent<generateSlots>().Add (i);
			}
		}
	}

}
