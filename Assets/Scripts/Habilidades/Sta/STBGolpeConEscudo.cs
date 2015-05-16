using UnityEngine;
using System.Collections;

public class STBGolpeConEscudo : TargetSkill {

	float damageRetardadoTime = 0.1f;

	public STBGolpeConEscudo () {
		//this.resource = "Habilidad/Heal/Castigo";
		this.name = "Golpe con Escudo";
		this.skillType = "Uso: Sobre un jugador o un enemigo";
		this.cooldownTime = 2f;
	}
	
	override public bool UseOnEnemy (GameObject target) {
		int fuerzaSt = owner.GetComponent<Attributtes>().getTotalStat(Utils.Stat.FUERZA);
		int dmg = owner.GetComponent<Attributtes>().getTotalDamage();
		damage = (int)(0.1f * fuerzaSt + 0.2f * dmg);
		target.AddComponent<BuffDamageRetardado>();
		target.GetComponent<BuffDamageRetardado>().Init(damage, damageRetardadoTime, skillID, owner);
		return true;
	}
	
	override public void MasteryUp () {
		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 4) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.AGUANTE);
		}
	}
	
	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.skillID = Random.Range (0, 10000);
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/STA_ICON_GOLPECONESCUDO");
		base.Init(newPlayer, newSkillThrower);
	}
}
