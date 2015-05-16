using UnityEngine;
using System.Collections;

public class ENDBGolpe : Habilidad
{

	public override bool Usar()
	{
		GameObject t = owner.GetComponent<ENComportamiento> ().GetTarget ();
		Vector3 tempPostion = t.transform.position;
		tempPostion.y = tempPostion.y + 1; //Para que no vaya contra el suelo
		GameObject m = owner.GetComponent<ENComportamiento> ().GetModelo ();
        if (Network.isServer)
            skillThrower.SpawnSkillAt("Habilidad/FireBall", tempPostion, owner.name, skillID);
				
		return true;
	}
	
	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.cooldownTime = 1.5f;
		base.Init(newPlayer, newSkillThrower);
	}
}
