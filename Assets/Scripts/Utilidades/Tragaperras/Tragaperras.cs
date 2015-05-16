using UnityEngine;
using System.Collections;

public class Tragaperras : MonoBehaviour {

	public GameObject c1;
	public GameObject c2;
	public GameObject c3;

	void Start () {
		StartCoroutine (GoTragaperras());
	}

	public IEnumerator GoTragaperras () {
		c1.GetComponent<RotateTragaperras> ().Go ();
		yield return new WaitForSeconds (0.5f);
		c2.GetComponent<RotateTragaperras> ().Go ();
		yield return new WaitForSeconds (0.5f);
		c3.GetComponent<RotateTragaperras> ().Go ();
	}

	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			c1.GetComponent<RotateTragaperras> ().Stop ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			c2.GetComponent<RotateTragaperras> ().Stop ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			c3.GetComponent<RotateTragaperras> ().Stop ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			StartCoroutine (GoTragaperras());
		}
	}
}
