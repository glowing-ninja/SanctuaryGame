using UnityEngine;
using System.Collections;

public class RotateTragaperras : MonoBehaviour {

	float RSpeed = 0f;
	bool stop = true;


	public void Go () {
		RSpeed = 10f;
		stop = false;
	}

	public void Stop () {
		stop = true;
	}

	void Update () {
		if (RSpeed > 0) {
			transform.Rotate(Vector3.forward * RSpeed);
		}
		if (stop && RSpeed > 0) {
			if (RSpeed <= 0.5f && ((transform.eulerAngles.z - 19 ) % 60) < 1f && (transform.eulerAngles.z - 19 > 0)) {
				RSpeed = 0;
			} else if (RSpeed > 0.5f) {
				RSpeed -= 0.05f;
			}
		}
	}
}
