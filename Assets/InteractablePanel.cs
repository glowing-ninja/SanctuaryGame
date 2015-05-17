using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InteractablePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public bool selected = false;
	public int type;
	public int subType;
	public int basicGold;
	public int totalGold;
	public int rarity;

	public void OnPointerEnter(PointerEventData data) {
		if (!selected) {
			Color temp = Color.gray;
			temp.a = 0.5f;
			GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, temp, 0.2f);
		}
	}
	
	public void OnPointerExit(PointerEventData data) {
		if (!selected) {
			Color temp = Color.white;
			temp.a = 0.5f;
			GetComponent<Image>().color = temp;
		}
	}

	public void OnPointerDown () {
		Color temp = new Color();

		if (selected) {
			rarity = (rarity + 1) % 3;
			if (rarity == 0) {
				temp = Color.green;
				temp.a = 0.5f;
				totalGold = basicGold;
			}
			else if (rarity == 1) {
				temp = Color.blue;
				temp.a = 0.5f;
				totalGold = 6 * basicGold;
			}
			else if (rarity == 2) {
				temp = Color.yellow;
				temp.a = 0.5f;
				totalGold = 18 * basicGold;
			}
		}
		else {
			for (int i = 0; i < 4; i ++) {
				temp = Color.white;
				temp.a = 0.5f;
				transform.parent.GetChild(i).GetComponent<Image>().color = temp;
				transform.parent.GetChild(i).GetComponent<InteractablePanel>().selected = false;
			}
			selected = true;
			rarity = 0;
			temp = Color.green;
			temp.a = 0.5f;
			totalGold = basicGold;
		}
		GetComponent<Image>().color = temp;
		transform.parent.GetChild(5).GetComponent<Text>().text = totalGold + "";
	}
}
