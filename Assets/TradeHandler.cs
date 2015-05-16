using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TradeHandler : MonoBehaviour {


	public NetworkPlayer person2trade;
	public GameObject myOK;
	public GameObject yourOK;
	public GameObject confirmar;

	private Item item2recive;

	public Color green;
	public Color red;

	public void init (NetworkPlayer np) {
		person2trade = np;
	}
	
	public void onChange (Item i) {
		GetComponent<NetworkView>().RPC("updateTrade", person2trade, i);
	}

	[RPC]
	public void updateTrade (Item i) {
		transform.GetChild(1).GetComponent<ShowItemInTrade>().item = i;
	}

	public void onMyOkClicked () {
		myOK.GetComponent<Image> ().color = green;
		GetComponent<NetworkView> ().RPC ("onTheirOkClicked", person2trade);
		if (yourOK.GetComponent<Image> ().color == green)
			confirmar.GetComponent<Button> ().interactable = true;
	}

	[RPC]
	public void onTheirOkClicked () {
		yourOK.GetComponent<Image> ().color = green;
		if (yourOK.GetComponent<Image> ().color == green)
			confirmar.GetComponent<Button> ().interactable = true;
	}

	public void OnConfirmed () {
		Item i2r = transform.GetChild (1).GetComponent<ShowItemInTrade> ().item;
		generateSlots gs = GameObject.Find ("InventoryPanel").GetComponent<generateSlots> ();
		if (i2r != null) {
			gs.Add (i2r);
		}
		int i = transform.GetChild (0).GetComponent<ShowItemOnOver> ().i;
		if (i != -1) {
			gs.inventory[i] = null;
			gs.gameObject.transform.GetChild(0).GetChild(i + 1).GetComponent<ShowItemOnOver>().i = -1;
			gameObject.SetActive(false);
		}
	}
}
