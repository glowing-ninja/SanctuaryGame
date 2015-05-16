using UnityEngine;
using System.Collections;

public class HandleMagicBall : SkillHandler {
	
	public float speed = 4.0f;
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
		_spCoef = 0.2f;
		_dmgCoef = 0.3f;
        jugador = newPlayer;
		float sp = newPlayer.GetComponent<Attributtes> ().stats [(int)Utils.Stat.MAGIA];
		float dmg = newPlayer.GetComponent<Attributtes> ().getTotalDamage ();
		
		damage = (int)(sp * _spCoef + dmg * _dmgCoef);
		this.skillID = skillID;
		
		Destroy (gameObject, 10f);
	}
}