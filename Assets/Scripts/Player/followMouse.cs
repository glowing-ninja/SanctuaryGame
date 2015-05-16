using UnityEngine;
using System.Collections;

public class followMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(!GetComponent<NetworkView>().isMine)
			enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 mousePosition = new Vector3();
		if(Physics.Raycast(ray, out hit))
		{
			mousePosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
			//if (Vector3.Distance(mousePosition, transform.position) < 2);
				transform.LookAt(mousePosition);
		}
	}
}
