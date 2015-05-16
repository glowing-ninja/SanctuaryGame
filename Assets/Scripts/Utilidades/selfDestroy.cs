using UnityEngine;
using System.Collections;

public class selfDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DestroyText() {
		Destroy (transform.parent.gameObject);
	}
}
