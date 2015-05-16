using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatDesglose : MonoBehaviour {

	public Text basicStat;
	public Text equipStat;
	public Text masteryStat;
	public Text bonusStat;

	private Attributtes atr;

	public void ShowStatDesglose (Utils.Stat st) {
		atr = GameObject.Find (PlayerPrefs.GetString ("playerName")).GetComponent<Attributtes> ();
		basicStat.GetComponent<Text>().text = atr.stats[(int)st] + "";
		equipStat.GetComponent<Text>().text = atr.getTotalEquip(st) + "";
		masteryStat.GetComponent<Text>().text = atr.getTotalMastery(st) + "";
		bonusStat.GetComponent<Text>().text = atr.bonusStats[(int)st] + "";
	}
}
