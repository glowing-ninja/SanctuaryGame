using UnityEngine;
using System.Collections;

public class ST1DefensaAbsoluta :SelfSkill {

	float duration = 5.0f	;

	public ST1DefensaAbsoluta () {
		this.name = "Defensa absoluta";
		this.description = "Aumenta tu armadura durante 5 segundos en 5.0 * aguante.";
		this.skillType = "Uso: Automático sobre el propio personaje";
		this.cooldownTime = 13f;
	}

	public override bool Usar()
	{
		int aguante = owner.GetComponent<Attributtes> ().getTotalStat (Utils.Stat.AGUANTE);
		damage = (int)(aguante * 5.0f);
		IncreaseArmor iDefense = owner.AddComponent<IncreaseArmor>();

		iDefense.Init(damage, duration, skillID, owner);

		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 4) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.CURA);
		}
		return true;
	}

	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.skillID = Random.Range (0, 10000);
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/STA_ICON_DEFENSAABSOLUTA");
		base.Init(newPlayer, newSkillThrower);
	}
}
