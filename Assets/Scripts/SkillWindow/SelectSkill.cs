using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectSkill : MonoBehaviour {

	public Sprite icon = null;
	public Habilidad skill;

	public GameObject skillToolTip;

	private GameObject instantiatedObject;

	public int pos = -1;

	void Start () {
		if (icon == null)
			icon = Resources.Load<Sprite> ("Sprites/no-item");
		GetComponent<Image> ().sprite = icon;
	}

	void Update () {
		if (Utils.skillWindowClick == Utils.ClickSystem.SELECTSKILL) {
			if (Utils.pos2Skill != pos && pos != -1) {
				transform.Find("skill-icon").GetComponent<Image>().color = Color.white;
			}
		}
	}

	// Use this for initialization
	public void select () {
		if (skill != null && Utils.pos2Skill > 0) {
			Utils.player.GetComponent<Teclado> ().SetSkill (skill, Utils.pos2Skill);
			Utils.pos2Skill = -1;
		}
	}

	public void OnSelect () {
		if (Utils.skillWindowClick == Utils.ClickSystem.SELECTSKILL) {
			transform.Find("skill-icon").GetComponent<Image> ().color = Color.green;
			Utils.pos2Skill = this.pos;
		}
	}
	
	public void Show () {
		if (skill != null) {
			instantiatedObject = Instantiate (skillToolTip) as GameObject;
			instantiatedObject.GetComponent<SkillTooltip> ().Init(icon, skill.name, skill.description, skill.skillType, skill.ImprovedCoolDown());
			instantiatedObject.transform.SetParent (transform.parent.parent, false);
		}
	}

	public void Hide () {
		Destroy (instantiatedObject);
	}
}