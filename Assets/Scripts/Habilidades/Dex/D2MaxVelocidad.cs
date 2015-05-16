using UnityEngine;
using System.Collections;

public class D2MaxVelocidad : SelfSkill {

	/*float duration = 1f	;


	void Awake()
	{
		Destroy(gameObject, duration);
	}

	override public bool Usar()
	{
		//if (Time.time < this.nextAttackTime)
			//return;

		damage = (int)(1.0f * 5.0f);
		skillThrower.SpawnSkillWithParent("MaxSpeed", owner.transform.position, Quaternion.identity, owner.name);
		owner.networkView.RPC("ApplyBuff", RPCMode.All, "IncreaseDexterity", this.damage, this.duration);
		//IncreaseDexterity iDexterity = player.AddComponent<IncreaseDexterity>();
		
		//iDexterity.Init(this.Damage, this.duration); 
		//this.nextAttackTime = Time.time + cooldownTime;
		return true;
	}

	/*override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		base.Init(newPlayer, newSkillThrower);
		this.cooldownTime = 30f;
	}*/
}
