using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour {
	protected float secondsToWait = 0f;
	protected float duration = 0f;
	protected float amount = 0f;
	public int id;
	public GameObject owner;

	/*public virtual void Init (float amount, float duration) {
		this.id = -1;
		this.amount = amount;
		this.secondsToWait = duration; 
		StartCoroutine ("Apply");
	}

	public virtual void Init (float amount, float duration, int id) {
		this.id = id;
		this.amount = amount;
		this.secondsToWait = duration; 
		StartCoroutine ("Apply");
	}*/

	public virtual void Init (float amount, float duration, int id, GameObject owner) {
		this.id = id;
		this.amount = amount;
		this.secondsToWait = duration; 
		StartCoroutine ("Apply");
		this.owner = owner;
	}

	virtual protected IEnumerator Apply() {
		yield return new WaitForSeconds(this.secondsToWait);
	}

	virtual public void Revert () {

	}

	virtual public System.Type Type() {
		return typeof(Buff);
	}
}