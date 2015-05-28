using UnityEngine;
using System.Collections;

public class ENRatExplosion : SelfSkill
{
    public override bool Usar()
    {
        GameObject t = owner.GetComponent<ENComportamiento>().GetTarget();
        Vector3 tempPostion = t.transform.position;
        tempPostion.y = tempPostion.y + 1; //Para que no vaya contra el suelo
        GameObject m = owner.GetComponent<ENComportamiento>().GetModelo();
        if (Network.isServer)
            skillThrower.SpawnSkillLookingAt("Habilidad/Enemigos/Rat/RatExplosion", m.transform.position,
                                         Quaternion.identity, tempPostion, owner.name, skillID);

        return true;
    }

    override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
    {
        this.cooldownTime = 5f;
        base.Init(newPlayer, newSkillThrower);
    }
	
}
