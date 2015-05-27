using UnityEngine;
using System.Collections;

public class HandleRatExplosion : SkillHandler
{
    public float speed = 3.0f;
    public Vector3 lookAt = new Vector3(0, 0, 0);

    override protected void Update()
    {
    }

    void OnTriggerEnter(Collider col)
    {
        if (Network.isServer)
        {
            if (col.tag == "Player")
            {
                if (col.gameObject.GetComponent<NetworkView>().owner.ToString() == Network.player.ToString())
                {
                    Attributtes est = ((Attributtes)col.gameObject.GetComponent("Attributtes"));
                    est.doDamage(this.damage);
                }
                else
                {
                    col.gameObject.GetComponent<NetworkView>().RPC("SendDoDamage", col.gameObject.GetComponent<NetworkView>().owner, this.damage);
                }
            }
        }
    }

    override public void Init(GameObject newPlayer, int skillID)
    {
        _strCoef = 1f;
        _dmgCoef = 2f;
        float str = newPlayer.GetComponent<ENEstadisticas>().Fuerza;
        float dmg = newPlayer.GetComponent<ENEstadisticas>().DañoBase;

        damage = (int)(str * _strCoef + dmg * _dmgCoef);
        this.skillID = skillID;

        Destroy(gameObject, 1.5f);
    }

}
