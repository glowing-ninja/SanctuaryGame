using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

	public Transform center;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (center.position, Vector3.up, 10f);
	}
}
