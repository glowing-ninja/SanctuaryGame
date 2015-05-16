using UnityEngine;
using System.Collections;

public class H2AreaSanadora : SelfSkill {

	public H2AreaSanadora () {	
		this.name = "Área sanadora";
		this.description = "Crea un área de sanación rodeando al personaje que cura a todo aliado cercano.";
		this.skillType = "Uso: Automático sobre el propio personaje";
		this.cooldownTime = 10f;
	}

	public override bool Usar()
	{
		Vector3 pos = new Vector3 (owner.transform.position.x, owner.transform.position.y + 0.01f, owner.transform.position.z);
		skillThrower.SpawnSkillAt("Habilidad/Heal/AreaSanadora", pos, owner.name, skillID);

		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 3) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.CURA);
		}

		return true;
	}

	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.skillID = Random.Range (0, 10000);
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/HEAL_ICON_AREASANADORA");
		base.Init(newPlayer, newSkillThrower);
	}
}
