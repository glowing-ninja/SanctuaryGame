using UnityEngine;
using System.Collections;

public class D1AtaqueRapido : SelfSkill {
	
	float duration = 4f;

	public D1AtaqueRapido () {
		name = "Ataque rápido";
		description = "Aumenta tu destreza en (5 * destreza) durante 4 segundos";
		this.skillType = "Uso: Automático sobre el propio personaje";
		this.cooldownTime = 30f;
	}

	public override bool Usar()
	{
		int dex = owner.GetComponent<Attributtes> ().getTotalStat (Utils.Stat.DESTREZA);
		damage = (int)(5.0f * dex);
		IncreaseDexterity iDex = owner.AddComponent<IncreaseDexterity>();
		iDex.Init(this.damage, this.duration, skillID, owner); 

		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 2) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.DESTREZA);
		}

		return true;
	}

	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/DEX_ICON_ATAQUERAPIDO");
		base.Init(newPlayer, newSkillThrower);
	}
}
