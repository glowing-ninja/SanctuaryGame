using UnityEngine;
using System.Collections;

public class HandleAliento : MonoBehaviour {

	ArrayList targets;
	float timeBetweenTicks = 0.5f;
	float time2tick = 0.0f;
	float damage = 10f;
	// Use this for initialization
	void Start () {
		targets = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		if (time2tick <= 0f) {
			foreach (GameObject o in targets) {
				o.GetComponent<Attributtes>().doDamage((int)damage);
			}
			time2tick = timeBetweenTicks;
		}
		else
			time2tick -= Time.deltaTime;
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player" && !targets.Contains(other.gameObject)) {
			targets.Add(other.gameObject);
		}
	}

	void OnTriggerExit (Collider other) {
		if (targets.Contains(other.gameObject)) {
			targets.Remove(other.gameObject);
		}
	}
}
