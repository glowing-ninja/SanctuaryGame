using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tragaperras : MonoBehaviour {

	public GameObject realCanvas;
	public GameObject falseCanvas;

	public GameObject realCamera;
	public GameObject falseCamera;

	public GameObject c1;
	public GameObject c2;
	public GameObject c3;

	public GameObject btJugar;

	private bool enMarcha = false;
	public int cilindrosEnMarcha = 0;

	public GameObject lbWin;
	public GameObject itemToolTip;
	private Attributtes atr;
	public GameObject lbNoGold;
	public GameObject lbPlayerGold;

	void Start () {
		//StartCoroutine (GoTragaperras());
	}

	void Update () {
		if (enMarcha && cilindrosEnMarcha == 0) {
			if (c1.transform.rotation == c2.transform.rotation && c1.transform.rotation == c3.transform.rotation) {
				// PREMIO
				lbWin.SetActive(true);
				lbWin.GetComponent<Text>().text = "Premio";

				//if (ip.totalGold <= atr.inventory.GetComponent<generateSlots>().Gold) {
				Item item = Item.ItemGenerator(2, atr.level);
				//Item item = Item.ItemGenerator(ip.type, ip.subType, atr.level, ip.rarity+1);
				if (atr.inventory.GetComponent<generateSlots>().Add(item)) {
					//atr.addGold(-ip.totalGold);
					//atr.inventory.GetComponent<generateSlots>().AddGold(-ip.totalGold);
					//Vector3 pos = new Vector3 (0f, -300f);
					//GameObject itp = Instantiate (itemToolTip, pos, Quaternion.identity) as GameObject;
					GameObject itp = Instantiate (itemToolTip);
					itp.transform.SetParent (falseCanvas.transform, false);
					itp.GetComponent<ItemToolTip> ().Show (item, true);
				}
				//}
			}
			else {
				// YOU FAIL
				lbWin.SetActive(true);
				lbWin.GetComponent<Text>().text = "Has perdido";
			}
			Invoke("hideLabel", 2f);
			enMarcha = false;
			btJugar.SetActive(true);
		}
	}

	public void hideLabel () {
		lbWin.SetActive(false);
	}

	public IEnumerator GoTragaperras () {
		c1.GetComponent<RotateTragaperras> ().Go ();
		yield return new WaitForSeconds (0.5f);
		c2.GetComponent<RotateTragaperras> ().Go ();
		yield return new WaitForSeconds (0.5f);
		c3.GetComponent<RotateTragaperras> ().Go ();
	}

	public void enchufarTragaperras () {
		if (!enMarcha) {
			atr.addGold(200);
			if (100 <= atr.inventory.GetComponent<generateSlots>().Gold) {
				atr.addGold(-100);
				lbPlayerGold.GetComponent<Text>().text = atr.getGold() + "";
				cilindrosEnMarcha = 3;
				StartCoroutine (GoTragaperras());
				btJugar.SetActive(false);
				enMarcha = true;
			}
			else {
				lbNoGold.SetActive(true);
			}
		}
	}

	public void On () {
		atr = Utils.player.GetComponent<Attributtes>();
		Utils.player.SetActive(false);
		realCamera.SetActive(false);
		realCanvas.SetActive(false);
		falseCamera.SetActive(true);
		falseCanvas.SetActive(true);
		lbPlayerGold.GetComponent<Text>().text = atr.getGold() + "";
	}

	public void Off () {
		Utils.player.SetActive(true);
		realCamera.SetActive(true);
		realCanvas.SetActive(true);
		falseCamera.SetActive(false);
		falseCanvas.SetActive(false);
		lbNoGold.SetActive(false);
	}
}
