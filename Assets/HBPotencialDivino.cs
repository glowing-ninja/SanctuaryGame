using UnityEngine;
using System.Collections;

public class HBPotencialDivino : TargetSkill {
	
	private float damageRetardadoTime = 0.1f;
	
	public HBPotencialDivino () {
		this.resource = "Habilidad/Heal/Castigo";
		this.name = "Potencial Divino";
	}



	override public bool UseOnEnemy (GameObject target) {
		int healSt = owner.GetComponent<Attributtes>().getTotalStat(Utils.Stat.CURA);
		this.damage = (int)(1.0f * healSt);

		if (Network.isServer) {
			target.AddComponent<BuffDamageRetardado>();
			target.GetComponent<BuffDamageRetardado>().Init(this.damage, damageRetardadoTime, skillID, owner);
		}
		else
			target.transform.parent.parent.GetComponent<NetworkView>().RPC("ApplyBuff", RPCMode.Server, this.damage);

		return true;
	}

	override public bool UseOnAlli (GameObject target) {
		int healSt = owner.GetComponent<Attributtes>().getTotalStat(Utils.Stat.CURA);
		this.damage = (int)(1.0f * healSt);
		target.AddComponent<BuffDamageRetardado>();
		target.GetComponent<BuffDamageRetardado>().Init (this.damage, damageRetardadoTime, skillID, owner);

		this.damage = (int)(2.0f * healSt);
		if (Network.isServer) {	
			target.AddComponent<BuffDamageRetardado>();
			target.GetComponent<BuffDamageRetardado>().Init (-this.damage, damageRetardadoTime, skillID, owner);
		}
		else 
			target.GetComponent<NetworkView>().RPC("Heal", target.GetComponent<NetworkView>().owner, this.damage, skillID);


		return true;
	}

	override public void MasteryUp () {
		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 3) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.CURA);
		}
	}
	
	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.cooldownTime = 2f;
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/HEAL_ICON_POTENCIALDIVINO");
		base.Init(newPlayer, newSkillThrower);
	}
}
