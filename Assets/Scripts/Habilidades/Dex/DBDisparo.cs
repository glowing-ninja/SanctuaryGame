using UnityEngine;
using System.Collections;

public class DBDisparo : DirectionalSkill {

	public override bool Usar()
	{
		RaycastHit hit;
		stopAnimation();
		Ray ray  = Camera .main .ScreenPointToRay (Input .mousePosition );
		if(Physics.Raycast (ray, out hit, Mathf.Infinity))
		{
			Vector3 tempPostion = hit.point;
			tempPostion.y = owner.transform.position.y + 1; //Para que no vaya contra el suelo

			if(Network.isServer)
				skillThrower.SpawnSkillLookingAt("Habilidad/Dex/Disparo", owner.transform.Find("Thrower").transform.position, Quaternion.identity,
				                                 tempPostion, owner.name, skillID);
			else
				skillThrower.GetComponent<NetworkView>().RPC("RPCSpawnSkillLookingAt", RPCMode.All, "Habilidad/Dex/Disparo", owner.transform.Find("Thrower").transform.position, Quaternion.identity,
				                             tempPostion, owner.name, skillID);

			if (owner.GetComponent<Attributtes>().equipamiento.GetComponent<PlayerEquip>().equipment.weapon.subType == 2) {
				owner.GetComponent<Attributtes>().MasteryUp(Utils.Stat.DESTREZA);
			}

			return true;
		}
		return false;
	}

	public void stopAnimation() {
		Utils.player.GetComponent<Animator>().SetBool("Bow", true);
		followMouse fm = Utils.player.GetComponent<followMouse>();
		fm.enabled = false;
	}


	override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.cooldownTime = 1.5f;
		this.icon = Resources.Load<Sprite> ("Sprites/SKILL_ICON/DEX_ICON_DISPARO");
		base.Init(newPlayer, newSkillThrower);
	}
}
