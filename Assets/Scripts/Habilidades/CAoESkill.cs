using UnityEngine;
using System.Collections;


public class CAoESkill : Habilidad {


	protected GameObject instantiatedProjectile;

	// Use this for initialization
	void Start () {
	
	}

	protected Vector3 getPosition()
	{
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			return hit.point;
		}
		return new Vector3();
	}
	


}
