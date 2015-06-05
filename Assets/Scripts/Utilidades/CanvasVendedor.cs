using UnityEngine;
using System.Collections;

public class CanvasVendedor : MonoBehaviour {

    public Transform anclaCanvas;

    //public Vector3 posAnclaCanvas;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 posCamera = Camera.main.WorldToScreenPoint(anclaCanvas.position);
        transform.position = posCamera;
	}
}
