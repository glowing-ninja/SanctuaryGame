using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickToSelectSkill : MonoBehaviour {

	public Sprite icon = null;
	public Habilidad skill;
	
	public GameObject skillToolTip;
	
	private GameObject instantiatedObject;
	
	void Start () {
		if (icon == null)
			icon = Resources.Load<Sprite> ("Sprites/no-item");
		GetComponent<Image> ().sprite = icon;
	}
	public void OnSelect () {
		if (Utils.skillWindowClick == Utils.ClickSystem.SELECTSKILL) {
			GetComponent<Image> ().color = Color.green;
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
