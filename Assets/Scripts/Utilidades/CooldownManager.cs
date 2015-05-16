using UnityEngine;
using System.Collections;

public class CooldownManager
{
	private float nextAttackTime = 0f;
	private float cooldownTime = 0f;
	private float improvedCooldown = 0f;


	public CooldownManager(float cd)
	{
		this.cooldownTime = cd;
		this.improvedCooldown = cd;
		this.nextAttackTime = Time.time + 0.5f;
	}

	public void resetAttackTime()
	{
		nextAttackTime = 0f;
	}

	public float CooldownPercentage()
	{
		if (Time.time >= nextAttackTime)
			return 1f;
		
		float startTime = nextAttackTime - improvedCooldown;
		return (Time.time - startTime) / (nextAttackTime - startTime);
	}

	public float CooldownTimeRemaining()
	{
		return improvedCooldown - CooldownPercentage () * improvedCooldown;
	}

	public bool isOnCooldown()
	{
		if (Time.time >= this.nextAttackTime)
		{
			this.nextAttackTime = Time.time + improvedCooldown;
			return true;
		}
		return false;

	}

	//Devolvemos true indicando si se ha usado o no
	public bool useWithCooldown(Habilidad skill)
	{
		if (Time.time < this.nextAttackTime) {
			return false;
		}

		//Aqui usar la habilidad
		if (skill.Usar()) {
			Attributtes atr = Utils.player.GetComponent<Attributtes>();
			int dexSt = atr.getTotalStat(Utils.Stat.DESTREZA);
			this.improvedCooldown = cooldownTime * (1.0f - ((float)dexSt * 5f / atr.level / 100f));
			this.nextAttackTime = Time.time + this.improvedCooldown;
			return true;
		}
		return false;
	}

}
