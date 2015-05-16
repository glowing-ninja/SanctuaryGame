using UnityEngine;
using System.Collections;

public class H1Castigo : TargetSkill {

	private float damageRetardadoTime = 0.1f;

	public H1Castigo () {
		this.resource = "Habilidad/Heal/Castigo";
		this.name = "Castigo";
		this.skillType = "Uso: Sobre un jugador o un enemigo";
		this.cooldownTime = 2f;
	}

	override public bool UseOnEnemy (GameObject target) {
		int healSt = owner.GetComponent<Attributtes>().getTotalStat(Utils.Stat.CURA);
		this.damage = (int)(0.2f * healSt);
		if (Network.isServer) {
			target.AddComponent<BuffDamageRetardado>();
			target.GetComponent<BuffDamageRetardado>().Init(this.damage, damageRetardadoTime, skillID, owner);
		}
		else
			target.transform.parent.parent.GetComponent<NetworkView>().RPC("ApplyBuff", RPCMode.Server, this.damage);
		return true;
	}

	override public void MasteryUp () {
		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 3) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.CURA);
		}
	}

	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/HEAL_ICON_CASTIGO");
		base.Init(newPlayer, newSkillThrower);
	}
}
