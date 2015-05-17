using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour {

	public Color[] rareza = new Color[5];
	public Color[] rarezaIn = new Color[5];
	public Sprite[] stat = new Sprite[5];
	public Sprite[] affinitys = new Sprite[5];
	public Sprite[] weapons = new Sprite[5];
	public Sprite[] armors = new Sprite[3];
	public Sprite[] attr = new Sprite[2];

	private bool hide = false;
	// Use this for initialization

	void Update() {
		if (hide) 
			GetComponent<CanvasGroup> ().alpha -= 0.01f;
		if (GetComponent<CanvasGroup> ().alpha <= 0.05)
			DestroyObject(gameObject);
	}

	public void Show (Item i, bool hide) {
		GetComponent<CanvasGroup> ().alpha = 1.5f;
		this.hide = hide;

		transform.Find("Border").GetComponent<Image>().color = rareza[i.rarity-1];
		GetComponent<Image>().color = rarezaIn[i.rarity-1];


		transform.Find("lb_name").GetComponent<Text>().text = i.name;
		transform.Find("lb_level").GetComponent<Text>().text = "Nivel: " + i.level;

		if (i.GetType() == typeof(Weapon)) {
			transform.Find("img_armor").GetComponent<Image>().sprite = attr[0];
			transform.Find("img_armor").transform.Find("Text").GetComponent<Text>().text = i.damage.ToString();
			switch((i as Weapon).getWeapon()) {
			case 0: transform.Find("img_type").GetComponent<Image>().sprite = weapons[0];break;
			case 1: transform.Find("img_type").GetComponent<Image>().sprite = weapons[1];break;
			case 2: transform.Find("img_type").GetComponent<Image>().sprite = weapons[2];break;
			case 3: transform.Find("img_type").GetComponent<Image>().sprite = weapons[3];break;
			case 4: transform.Find("img_type").GetComponent<Image>().sprite = weapons[4];break;
			}
		}
		else {
			transform.Find("img_armor").GetComponent<Image>().sprite = attr[1];
			transform.Find("img_armor").transform.Find("Text").GetComponent<Text>().text = i.armor.ToString();
			if (i.GetType() == typeof(Chest)) {
				transform.Find("img_type").GetComponent<Image>().sprite = armors[0];
			}
			else if (i.GetType() == typeof(Legs)) {
				transform.Find("img_type").GetComponent<Image>().sprite = armors[1];
			}
			else {
				transform.Find("img_type").GetComponent<Image>().sprite = armors[2];
			}
		}

		if (i.rarity >= 2) {
			transform.Find("img_stat1").GetComponent<Image>().enabled = true;
			transform.Find("img_stat1").GetComponent<Image>().sprite = stat[i.isStat1];
			transform.Find("img_stat1").transform.Find("lb_stat1").GetComponent<Text>().enabled = true;
			transform.Find("img_stat1").transform.Find("lb_stat1").GetComponent<Text>().text = i.stats[i.isStat1].ToString();
		}
		if (i.rarity >= 4) {
			transform.Find("img_stat2").GetComponent<Image>().enabled = true;
			transform.Find("img_stat2").GetComponent<Image>().sprite = stat[i.isStat2];
			transform.Find("img_stat2").transform.Find("lb_stat2").GetComponent<Text>().enabled = true;
			transform.Find("img_stat2").transform.Find("lb_stat2").GetComponent<Text>().text = i.stats[i.isStat2].ToString();
		}

		if (i.isAffinity != Utils.Element.NONE) {
			transform.Find("img_affinity").GetComponent<Image>().enabled = true;
			transform.Find("img_affinity").GetComponent<Image>().sprite = affinitys[(int)i.isAffinity];
			transform.Find("img_affinity").transform.Find("lb_affinity").GetComponent<Text>().enabled = true;
			transform.Find("img_affinity").transform.Find("lb_affinity").GetComponent<Text>().text = i.affinity + "%";
		}


	}
}
