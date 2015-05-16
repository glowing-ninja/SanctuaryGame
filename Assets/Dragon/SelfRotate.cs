using UnityEngine;
using System.Collections;

public class SelfRotate : MonoBehaviour {

	public GameObject particle;

	void Start() {
		StartCoroutine(createFireBall());
	}
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward);
	}

	public IEnumerator createFireBall() {
		yield return new WaitForSeconds (2.5f);
		GameObject inst = Instantiate(particle) as GameObject;
		inst.transform.SetParent(transform.parent,false);
		Destroy(transform.parent.gameObject, 1f);
	}
}
