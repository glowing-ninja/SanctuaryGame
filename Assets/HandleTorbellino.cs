using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandleTorbellino : SkillHandler {

	float tic = 0f;
	protected float duration = 4f;
	List<GameObject> targets;
	private Transform parent;

	void Awake()
	{
		if(!GetComponent<NetworkView>().isMine)
		{
			enabled = false;
			Destroy(gameObject, 5f);
		}
	}

	
	void Start () {
		targets = new List<GameObject> ();
	}

	
	// Update is called once per frame
	override protected void Update ()
	{
		
		tic = tic - Time.deltaTime;
		if (tic < 0f) 
		{
			for(int i = 0; i < targets.Count; i++)
			{
				if (targets[i] != null)
					targets[i].GetComponentInParent<ENEstadisticas>().ApplyDamage(jugador, damage, Utils.Element.FUEGO, 0f);
			}
			tic = 1f;
		}
		
		this.duration = this.duration - Time.deltaTime;
		if (this.duration <= 0f) 
		{	
			this.tic = 0f;
			this.duration = 5f;
			Destroy (gameObject);//Terminamos habilidad
		}
	}
	
	//Guardar objetos al entrar, aplicar el daño en el update, borrarlos al salir
	void OnTriggerEnter(Collider col)
	{
		if(GetComponent<NetworkView>().isMine)
		{
			//if(!targets.Contains(col.gameObject))
			if (col.gameObject.tag == "Enemy") 
			{
				if(!targets.Contains(col.gameObject))
					targets.Add (col.gameObject);
			}
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(GetComponent<NetworkView>().isMine)
			targets.Remove (col.gameObject);
	}
	
	override public void Init(GameObject newPlayer, int skillID)
	{
		_strCoef = 0.2f;
		_dmgCoef = 0.3f;
        jugador = newPlayer;
		float str = newPlayer.GetComponent<Attributtes> ().stats [(int)Utils.Stat.FUERZA];
		float dmg = newPlayer.GetComponent<Attributtes> ().getTotalDamage ();
		
		damage = (int)(str * _healCoef + dmg * _dmgCoef);
	}
}
