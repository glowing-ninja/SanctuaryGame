using UnityEngine;
using System.Collections;

public class BuffHealthModifier : Buff {

	public override void Init (float amount, float duration, int id, GameObject owner) {
		this.id = id;
		this.amount = amount;
		this.secondsToWait = duration;
		this.owner = owner;
		if (gameObject.tag == "Player")
			doHealing ((int)this.amount);
		if (gameObject.tag == "Enemy")
			doDamage ((int)this.amount);

	}

	public bool doDamage(int dmg) {
		return gameObject.GetComponentInParent<ENEstadisticas> ().ApplyDamage (dmg, Utils.Element.FUEGO, 30f);
		Destroy (this);
	}

    public bool doDamage(int dmg, GameObject jugador)
    {
        return gameObject.GetComponentInParent<ENEstadisticas>().ApplyDamage(dmg, Utils.Element.FUEGO, 30f);
        Destroy(this);
    }

	public void doHealing(int dmg) {
		gameObject.GetComponent<Attributtes>().doDamage(-dmg);
		Destroy (this);
	}
}
