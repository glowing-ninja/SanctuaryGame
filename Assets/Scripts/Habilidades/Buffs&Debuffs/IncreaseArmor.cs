using UnityEngine;
using System.Collections;

public class IncreaseArmor : Buff {
	
	
	override protected IEnumerator Apply()
	{
		Attributtes attributtes = gameObject.GetComponent<Attributtes>();

		attributtes.IncreaseArmorBuff((int)amount);

		yield return new WaitForSeconds(this.secondsToWait);

		attributtes.DecreaseArmorbuff((int)amount);

		Destroy (this);
	}
}