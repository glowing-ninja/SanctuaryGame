using UnityEngine;
using System.Collections;

public class MRU : MonoBehaviour {

	float v0 = 10f;
	float angle = 80.0f;
	float angleRadians;
	float x0;
	float y0;
	float x;
	float y;
	float t0;
	float g = 9.81f;

	public GameObject target;

	void Start () {
		//x0 = transform.position.x;
		//y0 = transform.position.z;
		//t0 = Time.time;
		//angleRadians = angle * Mathf.PI / 180f;
	}


	public void setTarget (GameObject target) {
		this.target = target;
	}

	void Update () {

		/*x = x0 + v0 * Mathf.Cos (angleRadians) * (Time.time - t0);
		y = y0 + v0 * Mathf.Sin (angleRadians) * (Time.time - t0) - (g / 2) * Mathf.Pow (Time.time - t0, 2);
		Debug.Log (x + "," + y);
		transform.position = new Vector3 (x, y, transform.position.z);*/
		transform.LookAt(new Vector3(target.transform.position.x,
		                             target.transform.position.y + 0.5f,
		                             target.transform.position.z));
		transform.Translate (Vector3.forward * v0 * Time.deltaTime);
	}

	void  OnCollisionEnter(Collision col) {
		if(Network.isServer) {
			if (col.gameObject.tag == "Player") {
				if(col.gameObject.GetComponent<NetworkView>().owner.ToString() == Network.player.ToString()) {
					Attributtes est = ((Attributtes)col.gameObject.GetComponent("Attributtes"));
					est.doDamage(25);
				}
				else {
					col.gameObject.GetComponent<NetworkView>().RPC("SendDoDamage", col.gameObject.GetComponent<NetworkView>().owner, 25);
				}
				
			}
			
		}
		GameObject fb = gameObject.transform.GetChild (0).gameObject;
		fb.transform.parent = null;
		fb.GetComponent<ParticleSystem>().Stop ();
		Destroy (gameObject);
		Destroy (fb, 3f);
		
	}
}
