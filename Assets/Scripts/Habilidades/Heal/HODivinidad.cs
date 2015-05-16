using UnityEngine;
using System.Collections;

public class HODivinidad : CAoESkill {

	public HODivinidad () {
		this.name = "Divinidad";
		this.skillType = "Uso: Sobre el puntero del ratón";
		this.cooldownTime = 20f;
	}
	public override bool Usar()
	{

		Vector3 pos = this.getPosition ();
		skillThrower.SpawnSkillAt("Habilidad/Heal/Divinidad", pos, owner.name, skillID);

		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 3) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.CURA);
		}

		return true;
	}

	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.skillID = Random.Range (0, 10000);
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/HEAL_ICON_DIVINIDAD");
		base.Init(newPlayer, newSkillThrower);
	}
}