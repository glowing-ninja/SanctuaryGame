using UnityEngine;
using System.Collections;

public class Habilidad {

	//_____Variables start______
	//public GameObject elemento;
	public string name = "<Nombre>";
	public string description = "<Descripción no implementada todavía>";
	public string skillType = "<Tipo no declarado>";

	public GameObject owner;
	public int damage;
	public float cooldownTime  = 3f;
	public float range = 0;
	public string resource = "";
	public int skillID;
	public SkillThrower skillThrower;
	private CooldownManager cooldownManager;
	public Sprite icon;


	public float strMultiplicator = 0f;
	public float spMultiplicator = 0f;
	public float dexMultiplicator = 0f;
	public float healMultiplicator = 0f;
	public float staMultiplicator = 0f;
	//_____Variables end________
	
	public virtual bool Usar()
	{
		return false;
	}

	public void setPlayer(GameObject p)
	{
		owner = p;
	}


	public float getDamage()
	{
		return damage;
	}

	protected bool isInRange(Vector3 target)
	{
		Debug.Log (Vector3.Distance (owner.transform.position, target));
		if (Vector3.Distance (owner.transform.position, target) <= range)
			return true;
		else 
			return false;
	}

	virtual public void Init(GameObject newPlayer, SkillThrower newSkillThrower)
	{
		this.owner = newPlayer;
		this.skillThrower = newSkillThrower;
		this.cooldownManager = new CooldownManager(this.cooldownTime); 

	}

	public void useWithCooldown()
	{
		this.cooldownManager.useWithCooldown(this);
	}
	
	public float cooldownTimeRemaining()
	{
		return this.cooldownManager.CooldownTimeRemaining();
	}

	public float CooldownPercentage() {
		return this.cooldownManager.CooldownPercentage ();
	}

	public float ImprovedCoolDown () {
		Attributtes atr = Utils.player.GetComponent<Attributtes>();
		int dexSt = atr.getTotalStat(Utils.Stat.DESTREZA);
		return cooldownTime * (1.0f - ((float)dexSt * 5f / atr.level / 100f));
	}

}
