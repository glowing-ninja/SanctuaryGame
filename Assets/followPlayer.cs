using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}



	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(Utils.player.transform.position, transform.position) < 10) {
			Vector3 lookAt = new Vector3(Utils.player.transform.position.x,
			                             transform.position.y,
			                             Utils.player.transform.position.z);
			transform.LookAt(lookAt);
		}
	}
}
