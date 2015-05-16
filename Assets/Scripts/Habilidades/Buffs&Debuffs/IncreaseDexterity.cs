using UnityEngine;
using System.Collections;

public class IncreaseDexterity : Buff {


	override protected IEnumerator Apply()
	{
		Attributtes attributtes = gameObject.GetComponent<Attributtes>();

		attributtes.IncreaseDexterity((int)amount);

		yield return new WaitForSeconds(this.secondsToWait);

		attributtes.DecreaseDexterity((int)amount);

		Destroy (this);
	}


}
//Network.RemoveRPCs(objectImDestroying.GetComponent(NetworkView)); 