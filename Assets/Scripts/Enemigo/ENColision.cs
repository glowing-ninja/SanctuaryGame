using UnityEngine;
using System.Collections;

public class ENColision : MonoBehaviour {

	private ENEstadisticas stats;
	// Use this for initialization
	void Start () {
		stats = GetComponentInParent<ENEstadisticas> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
