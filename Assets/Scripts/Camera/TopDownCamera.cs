using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour {

	public float height = 20.0f;
	public float distance = 15.0f;
	public Transform target;
	public float horizontalOffset = 20.0f;
	public float verticalOffset = 0f;

	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
		{
			height = Mathf.Min(height + 0.5f, 10);
			distance = height;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
		{
			height = Mathf.Max(height - 0.5f, 6);
			distance = height;
		}

		
		if (target != null ) {
			Vector3 newPos = target.position;
			newPos.y += height;
			newPos.z -= distance;
			transform.position = newPos;
			
			
			transform.LookAt(target.position);
		}
	}
}
