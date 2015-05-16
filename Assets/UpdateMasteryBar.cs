using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateMasteryBar : MonoBehaviour {
	
	public Image img_sword;
	public Text sword;
	public Image img_book;
	public Text book;
	public Image img_bow;
	public Text bow;
	public Image img_staff;
	public Text staff;
	public Image img_shield;
	public Text shield;

	// Use this for initialization
	void OnEnable () {
		StartCoroutine (updateMastery ());
	}

	public IEnumerator updateMastery () {
		while (true) {
			Attributtes atr = Utils.player.GetComponent<Attributtes> ();
			
			sword.GetComponent<Text> ().text = atr.masterys[(int)Utils.Stat.FUERZA] + "";
			img_sword.fillAmount = ((float)atr.masteryExp[0] / (float)atr.masteryExpTotal[0]);

			book.GetComponent<Text> ().text = atr.masterys[(int)Utils.Stat.MAGIA] + "";
			img_book.fillAmount = ((float)atr.masteryExp[1] / (float)atr.masteryExpTotal[1]);

			bow.GetComponent<Text> ().text = atr.masterys[(int)Utils.Stat.DESTREZA] + "";
			img_bow.fillAmount = ((float)atr.masteryExp[2] / (float)atr.masteryExpTotal[2]);

			staff.GetComponent<Text> ().text = atr.masterys[(int)Utils.Stat.CURA]+ "";
			img_staff.fillAmount = ((float)atr.masteryExp[3] / (float)atr.masteryExpTotal[3]);

			shield.GetComponent<Text> ().text = atr.masterys[(int)Utils.Stat.AGUANTE] + "";
			img_shield.fillAmount = ((float)atr.masteryExp[4] / (float)atr.masteryExpTotal[4]);

			yield return new WaitForSeconds (0.5f);
		}
	}
}
