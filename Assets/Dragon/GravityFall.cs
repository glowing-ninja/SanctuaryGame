using UnityEngine;
using System.Collections;

public class GravityFall : MonoBehaviour {

	public bool active = true;
	public ParticleSystem collision;
	
	// Update is called once per frame
	void Update () {
		if (active) {
			transform.Translate(Vector3.down * 0.25f);
			if (transform.position.y <= 0) {
				Debug.Log(Time.time);
				Instantiate(collision, transform.position, collision.transform.rotation);
				gameObject.GetComponent<ParticleSystem>().Stop();
				Destroy (gameObject, 2f);
				active = false;
			}
		}
	}
}
