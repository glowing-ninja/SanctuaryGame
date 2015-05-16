using UnityEngine;
using System.Collections;

public class SkillThrower : MonoBehaviour {



	public void SpawnSkillWithParent(string resource, Vector3 position,
	                                 Quaternion rotation, string owner,
	                                 int skillID)
	{
		GetComponent<NetworkView>().RPC("RPCSpawnSkillWithParent", RPCMode.All, resource, position, rotation, owner, skillID);
	}

	[RPC]
	protected GameObject RPCSpawnSkillWithParent (string resource, Vector3 position, Quaternion rotation,
	                                              string owner, int skillID)
	{
		GameObject elemento = Resources.Load(resource, typeof(GameObject)) as GameObject;
		
		GameObject instantiatedProjectile =  Instantiate(elemento, position, rotation) as GameObject;
		
		GameObject player = GameObject.Find (owner);
		instantiatedProjectile.GetComponent<SkillHandler> ().Init (player, skillID);

		if(instantiatedProjectile.GetComponent<Collider>())
		{
			Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(),
			                        gameObject.transform.root.GetComponent<Collider>());
		}
		
		instantiatedProjectile.transform.SetParent(GameObject.Find(owner).transform);
		return instantiatedProjectile;
	}

	public void SpawnSkillLookingAt(string resource, Vector3 position,
	                                Quaternion rotation, Vector3 lookAt,
	                                string owner, int skillID)
	{
		GetComponent<NetworkView>().RPC("RPCSpawnSkillLookingAt", RPCMode.All, resource, position, rotation,
		                lookAt, owner, skillID);
	}

	[RPC]
	protected void RPCSpawnSkillLookingAt (string resource, Vector3 position, Quaternion rotation, 
	                                       Vector3 lookAt, string owner, int skillID)
	{
		GameObject elemento = Resources.Load(resource, typeof(GameObject)) as GameObject;

		GameObject instantiatedProjectile =  Instantiate(elemento, position, rotation) as GameObject;
		
		instantiatedProjectile.transform.LookAt(lookAt);
		GameObject player = GameObject.Find (owner);
		instantiatedProjectile.GetComponent<SkillHandler> ().Init (player, skillID);

		Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(),
		                       gameObject.transform.GetComponent<Collider>());
	}


	public void SpawnSkillAt(string resource, Vector3 target, string owner, int skillID) {
		GetComponent<NetworkView>().RPC("RPCSpawnSkillAt", RPCMode.All, resource, target, owner, skillID);
	}
	
	[RPC]
	protected void RPCSpawnSkillAt (string resource, Vector3 target, string owner, int skillID)
	{
		GameObject elemento = Resources.Load(resource, typeof(GameObject)) as GameObject;

		GameObject instantiatedProjectile =  Instantiate(elemento, target, elemento.transform.rotation) as GameObject;

		GameObject player = GameObject.Find (owner);
		instantiatedProjectile.GetComponent<SkillHandler> ().Init (player, skillID);
	}
}
