using UnityEngine;
using System.Collections;

public class EnemyMovementUpdate : MonoBehaviour {
	
	
	
	//_____Variables start______
	private Vector3 lastPostion;
	private Quaternion lastRotation;
	private Transform positionTransform;
	private Transform rotationTransform;

	//_____Variables end______
	
	// Use this for initialization
	void Start () 
	{
		this.positionTransform = transform;
		this.rotationTransform = gameObject.transform.FindChild("Enemigo") as Transform;

		
		if(GetComponent<NetworkView>().isMine == true)
		{
			//Ensure that everyone sees the player at the correct location
			//the moment the spawn
			//			this.SendSameLevel();
			//networkView.RPC ("updateMovement", RPCMode.Others, 
			//    this.positionTransform.position, this.rotationTransform.rotation);
		}
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the player has moved at all then fire off an RPC to update the players
		//position and rotation across the network
		if(Vector3.Distance(this.positionTransform.position, this.lastPostion) >= 0.1)
		{
			//Capture the player's position before the RPC is fired off and use this
			//to determine if the player has moved in the if statement above
			this.lastPostion = this.positionTransform.position;
			
			//			this.SendSameLevel();
			//networkView.RPC ("updateMovement", RPCMode.Others, 
			// this.positionTransform.position, this.rotationTransform.rotation);
		}
		
		if(Quaternion.Angle(this.rotationTransform.rotation, this.lastRotation) >= 1)
		{
			//Capture the player's position before the RPC is fired off and use this
			//to determine if the player has turned in the if statement above
			this.lastRotation = this.rotationTransform.rotation;
			
			//   		this.SendSameLevel();
			//networkView.RPC ("updateMovement", RPCMode.Others, 
			//this.positionTransform.position, this.rotationTransform.rotation);
			
		}
	}
	
	[RPC]
	void updateMovement(Vector3 newPosition, Quaternion newRotation)
	{
		if(positionTransform != null)
		{
			this.positionTransform.position = newPosition;
			this.rotationTransform.rotation = newRotation;
		}
	}
	
	/*void SendSameLevel()
	{
		for(int i = 0; i < this.playerDB.playersList.Count; i++)
		{
			if(this.playerDB.playersList[i].NetworkPlayer != Network.player && this.playerDB.playersList[i].CurrentLevel == this.level)
			{
				networkView.RPC ("updateMovement", this.playerDB.playersList[i].NetworkPlayer, 
				                 this.positionTransform.position, this.rotationTransform.rotation);
			}
		}
	}*/
}
