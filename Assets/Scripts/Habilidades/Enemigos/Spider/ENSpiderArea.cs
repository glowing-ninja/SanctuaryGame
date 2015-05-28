using UnityEngine;
using System.Collections;

public class ENSpiderArea : SelfSkill
{
    public override bool Usar()
    {
        GameObject t = owner.GetComponent<ENComportamiento>().GetTarget();
        Vector3 tempPostion = t.transform.position;
        tempPostion.y = tempPostion.y + 1; //Para que no vaya contra el suelo
        GameObject m = owner.GetComponent<ENComportamiento>().GetModelo();
        if (Network.isServer)
            skillThrower.SpawnSkillLookingAt("Habilidad/Enemigos/Spider/SpiderArea", m.transform.position,
                                         Quaternion.identity, tempPostion, owner.name, skillID);

        return true;
    }

    override public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
    {
        this.cooldownTime = 5f;
        base.Init(newPlayer, newSkillThrower);
    }

}
