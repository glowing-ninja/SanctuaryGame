using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class F2Torbellino : CAoESkill {

	public F2Torbellino () {
		this.name = "Torbellino";
		this.description = "Gira sobre tí mismo para hacer daño a los enemigos cercanos.";
		this.skillType = "Uso: Automático sobre el propio personaje";
		this.cooldownTime = 5f;
	}

	public override bool Usar()
	{
		skillThrower.SpawnSkillWithParent("Habilidad/Str/TorbellinoSkill", owner.transform.position, Quaternion.identity, owner.name, skillID);

		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 0) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.FUERZA);
		}
		return true;
	}

	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.skillID = Random.Range (0, 10000);
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/STR_ICON_TORBELLINO");
		base.Init(newPlayer, newSkillThrower);
	}
}
