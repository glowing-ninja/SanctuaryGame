using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
		GetComponent<NetworkView>().RPC("updateTrade", person2trade, ItemSerialize(i));
	}

	[RPC]
	public void updateTrade (byte[] i) {
		transform.GetChild(1).GetComponent<ShowItemInTrade>().item = DeserializeItem(i);
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

	private byte[] ItemSerialize(Item item) 
	{
		
		BinaryFormatter binFormatter = new BinaryFormatter();
		MemoryStream memStream = new MemoryStream();
		
		binFormatter.Serialize (memStream, item);
		byte[] serialized = memStream.ToArray ();
		
		memStream.Close ();
		
		return serialized;
	}
	
	private Item DeserializeItem(byte[] item)
	{
		BinaryFormatter binFormatter = new BinaryFormatter(); 
		MemoryStream memStream = new MemoryStream();
		
		memStream.Write(item,0,item.Length); 
		
		memStream.Seek(0, SeekOrigin.Begin); 

		Item tmp = (Item)binFormatter.Deserialize(memStream);

		switch(tmp.type) {
		case 0: // arma
			switch (tmp.subType) {
			case (int)Utils.WeaponType.ESPADA:
				tmp.icon = Resources.Load<Sprite>("Sprites/sword-icon");
				break;
			case (int)Utils.WeaponType.LIBRO:
				tmp.icon = Resources.Load<Sprite>("Sprites/book-icon");	
				break;
			case (int)Utils.WeaponType.ARCO:
				tmp.icon = Resources.Load<Sprite>("Sprites/bow-icon");
				break;
			case (int)Utils.WeaponType.BASTON:
				tmp.icon = Resources.Load<Sprite>("Sprites/staff-icon");
				break;
			case (int)Utils.WeaponType.ESCUDO:
				tmp.icon = Resources.Load<Sprite>("Sprites/shield-icon");
				break;
			}
			break;
		case 1:
			switch (tmp.subType) {
			case 0:
				tmp.icon = Resources.Load<Sprite>("Sprites/chest-icon");
				break;
			case 1:
				tmp.icon = Resources.Load<Sprite>("Sprites/legs-icon");	
				break;
			case 2:
				tmp.icon = Resources.Load<Sprite>("Sprites/boots-icon");
				break;
			}
			break;
		}
		
		return tmp;
	}
}
