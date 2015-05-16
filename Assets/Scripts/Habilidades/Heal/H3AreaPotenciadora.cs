using UnityEngine;
using System.Collections;

public class H3AreaPotenciadora : CAoESkill {

	public H3AreaPotenciadora () {
		this.name = "Área potenciadora";
		this.description = "Invoca un área en el suelo que potencia a los jugadores aliados pero explota si golpea a un enemigo.";
		this.skillType = "Uso: Sobre el puntero del ratón";
		this.cooldownTime = 10f;
	}

	public override bool Usar()
	{
		Vector3 pos = this.getPosition () + new Vector3 (0, 0.01f, 0);;
		skillThrower.SpawnSkillAt("Habilidad/Heal/AreaPotenciadora", pos, owner.name, skillID);

		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 3) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.CURA);
		}

		return true;
	}

	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.skillID = Random.Range (0, 10000);
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/HEAL_ICON_AREAPOTENCIADORA");
		base.Init(newPlayer, newSkillThrower);
	}
}