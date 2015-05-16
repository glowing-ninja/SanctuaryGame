using UnityEngine;
using System.Collections;

public class TargetSkill : Habilidad {

	protected GameObject getMouseGameObject()
	{
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			if(hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
				return hit.collider.gameObject;
		}
		return null;
	}
	
	protected GameObject getTarget(string targetTag)
	{
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			if(hit.collider.tag == targetTag)
				return hit.collider.gameObject;
		}
		return null;
	}

	public override bool Usar() {
		bool used = false;
		GameObject target = getMouseGameObject();
		if (target != null) {
			if (target.tag == "Player") {
				used = UseOnAlli(target);
			}
			if (target.tag == "Enemy") {
				used = UseOnEnemy(target);
			}
			Vector3 tempPosition = target.transform.position;
			if (resource != "")
				skillThrower.SpawnSkillAt(resource, tempPosition, owner.name, skillID);

			MasteryUp ();
		}
		return used;
	}

	virtual public bool UseOnAlli (GameObject target) {
		return false;
	}

	virtual public bool UseOnEnemy (GameObject target) {
		return false;
	}

	virtual public void MasteryUp () {

	}

}
