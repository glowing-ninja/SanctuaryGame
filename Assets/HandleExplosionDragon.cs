using UnityEngine;
using System.Collections;

public class HandleExplosionDragon : MonoBehaviour {


	void OnTriggerEnter (Collider other) {
		if (Network.isServer) {
			if (other.gameObject.tag == "Player") {
				other.gameObject.GetComponent<Attributtes>().doDamage(9999);
			}
		}
	}
}
