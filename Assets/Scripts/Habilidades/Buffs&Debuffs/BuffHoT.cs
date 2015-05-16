using UnityEngine;
using System.Collections;

public class BuffHoT : Buff {
	public bool isOnArea = true;

	public void HealOverTime(int cantidad, float tiempo, float maxTiempo) {
		Destroy (this, maxTiempo);
		StartCoroutine (Heal(cantidad, tiempo));
	}

	public IEnumerator Heal(int cantidad, float tiempo) {
		while (isOnArea) {
			gameObject.GetComponent<Attributtes>().doDamage(-cantidad);
			yield return new WaitForSeconds(tiempo);
		}
	}
}
