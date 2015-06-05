using UnityEngine;
using System.Collections;

public class HandleDisparo : SkillHandler {

	public float speed = 8.0f;
	public Vector3 lookAt = new Vector3(0,0,0);

	override protected void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(Network.isServer)
		{
			if (col.gameObject.tag == "Enemy")
			{
				ENEstadisticas estEnemigo = col.gameObject.GetComponentInParent<ENEstadisticas>();
				//estEnemigo.ApplyDamage(this.damage, Utils.Element.NONE, 0f);
				estEnemigo.ApplyDamage(jugador, this.damage, Utils.Element.NONE, 0f);
			
            }
		}
		Destroy (gameObject);
	}
	
	override public void Init(GameObject newPlayer, int skillID)
	{
		_strCoef = 0.3f;
		_dmgCoef = 0.2f;
		jugador = newPlayer;
		float str = newPlayer.GetComponent<Attributtes> ().getTotalStat (Utils.Stat.FUERZA);
		float dmg = newPlayer.GetComponent<Attributtes> ().getTotalDamage ();

		damage = (int)(str * _strCoef + dmg * _dmgCoef);
		this.skillID = skillID;

		Destroy (gameObject, 10f);
	}
}