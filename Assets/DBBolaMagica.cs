using UnityEngine;
using System.Collections;

public class DBBolaMagica : DirectionalSkill {
	
	public override bool Usar()
	{
		RaycastHit hit;
		
		Ray ray  = Camera .main .ScreenPointToRay (Input .mousePosition );
		if(Physics.Raycast (ray, out hit, Mathf.Infinity))
		{
			Vector3 tempPostion = hit.point;
			tempPostion.y = owner.transform.position.y + 1; //Para que no vaya contra el suelo
			
			if(Network.isServer)
				skillThrower.SpawnSkillLookingAt("Habilidad/Sp/MagicBall", owner.transform.Find("Thrower").transform.position, Quaternion.identity,
				                                 tempPostion, owner.name, skillID);
			else
				skillThrower.GetComponent<NetworkView>().RPC("RPCSpawnSkillLookingAt", RPCMode.All, "Habilidad/Sp/MagicBall", owner.transform.Find("Thrower").transform.position, Quaternion.identity,
				                             tempPostion, owner.name, skillID);
			if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 1) {
				owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.MAGIA);
			}
			
			return true;
		}
		return false;
	}
	
	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.cooldownTime = 1.5f;
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/SP_ICON_BOLAMAGICA");
		base.Init(newPlayer, newSkillThrower);
	}
}
