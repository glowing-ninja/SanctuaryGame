using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateStats : MonoBehaviour {

	public Text dmg;
	public Text armor;

	public Text str;
	public Text sp;
	public Text dex;
	public Text heal;
	public Text sta;

	void OnEnable () {
		StartCoroutine (updateStats ());
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator updateStats () {
		while (true) {
			Attributtes atr = Utils.player.GetComponent<Attributtes> ();
			
			dmg.GetComponent<Text> ().text = atr.getTotalDamage() + "";
			armor.GetComponent<Text> ().text = atr.getTotalArmor() + "";

			str.GetComponent<Text> ().text = atr.getTotalStat (Utils.Stat.FUERZA) + "";
			sp.GetComponent<Text> ().text = atr.getTotalStat (Utils.Stat.MAGIA) + "";
			dex.GetComponent<Text> ().text = atr.getTotalStat (Utils.Stat.DESTREZA) + "";
			heal.GetComponent<Text> ().text = atr.getTotalStat (Utils.Stat.CURA) + "";
			sta.GetComponent<Text> ().text = atr.getTotalStat (Utils.Stat.AGUANTE) + "";

			yield return new WaitForSeconds (0.5f);
		}
	}
}
