using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillTooltip : MonoBehaviour {

	public void Init (Sprite icon, string name, string description, string type, float cd) {
		transform.FindChild ("Skill").GetComponent<Image> ().sprite = icon;
		transform.FindChild ("lb_Name").GetComponent<Text> ().text = name;
		transform.FindChild ("lb_Description").GetComponent<Text> ().text = description;
		transform.FindChild ("lb_Cd").GetComponent<Text> ().text = cd.ToString("F1")  + "s";
		transform.FindChild ("lb_Type").GetComponent<Text> ().text = type;
	}
}
