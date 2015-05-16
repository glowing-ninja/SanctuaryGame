using UnityEngine;
using System.Collections;

public class BuffDamageRetardado : Buff {

	override protected IEnumerator Apply() {
		yield return new WaitForSeconds(duration);

		if (gameObject.tag == "Enemy")
			//gameObject.GetComponent<ENEstadisticas> ().ApplyDamage (gameObject, cantidad, Utils.Element.FUEGO, 30f);
			gameObject.GetComponentInParent<ENEstadisticas> ().ApplyDamage (owner, amount, Utils.Element.NONE, 0f);
		
		else if (gameObject.tag == "Player")
			gameObject.GetComponent<Attributtes> ().doDamage ((int)amount);
	}
}
