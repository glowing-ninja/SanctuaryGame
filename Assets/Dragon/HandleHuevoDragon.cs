using UnityEngine;
using System.Collections;

public class HandleHuevoDragon : MonoBehaviour {

	public GameObject DragonInvocable;
	public float TimeToInvoke = 10f;
	
	// Update is called once per frame
	void Update () {
		TimeToInvoke -= Time.deltaTime;
		if (TimeToInvoke <= 0) {
			Vector3 pos = new Vector3(this.transform.position.x, DragonInvocable.transform.position.y, this.transform.position.z);
			GameObject inst = Instantiate(DragonInvocable, pos, Quaternion.identity) as GameObject;
			inst.transform.parent = this.transform.parent;
			Destroy(gameObject);
		}
	}
}
