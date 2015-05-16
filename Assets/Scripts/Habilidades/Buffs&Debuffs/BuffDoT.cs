using UnityEngine;
using System.Collections;

public class BuffDoT : MonoBehaviour {
	public bool isOnArea = true;

	public void DamageOverTime(int cantidad, float tiempo, float maxTiempo) {
		Destroy (this, maxTiempo);
		StartCoroutine (Damage(cantidad, tiempo));
	}

	public IEnumerator Damage(int cantidad, float tiempo) {
		while (isOnArea) {
			gameObject.GetComponent<ENEstadisticas>().ApplyDamage(cantidad, Utils.Element.FUEGO, 30f);
			yield return new WaitForSeconds(tiempo);
		}
	}
}
