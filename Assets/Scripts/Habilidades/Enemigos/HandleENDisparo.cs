using UnityEngine;
using System.Collections;

public class HandleENDisparo : SkillHandler {
	
	public float speed = 3.0f;
	public Vector3 lookAt = new Vector3(0,0,0);

	override protected void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}
	
	void  OnCollisionEnter(Collision col)
	{
		if(Network.isServer)
		{
			if (col.gameObject.tag == "Player")
			{
				if(col.gameObject.GetComponent<NetworkView>().owner.ToString() == Network.player.ToString())
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
		GameObject fb = gameObject.transform.GetChild (0).gameObject;
		fb.transform.parent = null;
		fb.GetComponent<ParticleSystem>().Stop ();
		Destroy (gameObject);
		Destroy (fb, 3f);
		
	}
	
	override public void Init(GameObject newPlayer, int skillID)
	{
		_strCoef = 1f;
		_dmgCoef = 1f;
		float str = newPlayer.GetComponent<ENEstadisticas> ().Fuerza;
		float dmg = newPlayer.GetComponent<ENEstadisticas> ().DañoBase;
		
		damage = (int)(str * _strCoef + dmg * _dmgCoef);
		this.skillID = skillID;
		
		Destroy (gameObject, 10f);
	}
}