﻿using UnityEngine;
using System.Collections;

public class FBMachaque : TargetSkill {

	float damageRetardadoTime = 0.1f;

	public FBMachaque () {
		//this.resource = "Habilidad/Heal/Castigo";
		this.name = "Machaque";
		this.skillType = "Uso: Sobre un jugador o un enemigo";
		this.cooldownTime = 1.5f;
	}
	
	override public bool UseOnEnemy (GameObject target) {
		stopAnimation();
		int fuerzaSt = owner.GetComponent<Attributtes>().getTotalStat(Utils.Stat.FUERZA);
		int dmg = owner.GetComponent<Attributtes>().getTotalDamage();
		damage = (int)(0.6f * fuerzaSt + 0.4f * dmg);
		if (Network.isServer) {
			target.AddComponent<BuffDamageRetardado>();
			target.GetComponent<BuffDamageRetardado>().Init(this.damage, damageRetardadoTime, skillID, owner);
		}
		//else
			//target.transform.parent.parent.GetComponent<NetworkView>().RPC("ApplyBuff", RPCMode.Server, this.damage);
		return true;
	}

	public void stopAnimation() {
		Utils.player.GetComponent<Animator>().SetBool("Sword", true);
		followMouse fm = Utils.player.GetComponent<followMouse>();
		fm.enabled = false;
	}
	
	override public void MasteryUp () {
		if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 4) {
			owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.AGUANTE);
		}
	}


	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.cooldownTime = 1.5f;
		this.skillID = Random.Range (0, 10000);
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/STR_ICON_MACHAQUE");
		base.Init(newPlayer, newSkillThrower);
	}
}
