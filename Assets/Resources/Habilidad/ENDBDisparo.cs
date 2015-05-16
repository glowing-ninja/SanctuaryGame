using UnityEngine;
using System.Collections;

public class ENDBDisparo : DirectionalSkill {
	
	public override bool Usar()
	{
		GameObject t = owner.GetComponent<ENComportamiento> ().GetTarget ();
		Vector3 tempPostion = t.transform.position;
		tempPostion.y = tempPostion.y + 1; //Para que no vaya contra el suelo
		GameObject m = owner.GetComponent<ENComportamiento> ().GetModelo ();
		if(Network.isServer)
				skillThrower.SpawnSkillLookingAt("Habilidad/FireBall", m.transform.Find("Thrower").transform.position,
			                                 Quaternion.identity, tempPostion, owner.name, skillID);
			//skillThrower.SpawnSkillLookingAt("Habilidad/FireBall", owner.transform.Find("Thrower").transform.position,
			//                                 Quaternion.identity, tempPostion, owner.name, skillID);
		//else
		//	skillThrower.networkView.RPC("SendSkillLookingAt", RPCMode.Server, "Habilidad/FireBall", owner.transform.position, Quaternion.identity,
		//	                             tempPostion);

		return true;
	}
	
	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.cooldownTime = 1f;
		base.Init(newPlayer, newSkillThrower);
	}
}
