using UnityEngine;
using System.Collections;

/// <summary>
/// Movement update.
/// This script is attached to the player and it ensures that
/// every players position, rotation, and scale, are kept up to date across
/// the network
/// 
/// This script is closely based on a script written by M2H
/// </summary>

public class MovementUpdate : MonoBehaviour {
	
	//_____Variables start______
	private Vector3 lastPostion;
	private Quaternion lastRotation;
	private Transform myTransform;
	//_____Variables end______
	
	// Use this for initialization
	void Start () 
	{

		if(GetComponent<NetworkView>().isMine == true)
		{
			
			this.myTransform = transform;
			
			//Ensure that everyone sees the player at the correct location
			//the moment the spawn
			GetComponent<NetworkView>().RPC ("updateMovement", RPCMode.Others, 
			                 this.myTransform.position, this.myTransform.rotation);
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
		if(Vector3.Distance(this.myTransform.position, this.lastPostion) >= 0.1)
		{
			//Capture the player's position before the RPC is fired off and use this
			//to determine if the player has moved in the if statement above
			this.lastPostion = this.myTransform.position;
			
			GetComponent<NetworkView>().RPC ("updateMovement", RPCMode.Others, 
			                 this.myTransform.position, this.myTransform.rotation);
		}
		
		if(Quaternion.Angle(this.myTransform.rotation, this.lastRotation) >= 15)
		{
			//Capture the player's position before the RPC is fired off and use this
			//to determine if the player has turned in the if statement above
			this.lastRotation = this.myTransform.rotation;
			
			GetComponent<NetworkView>().RPC ("updateMovement", RPCMode.Others, 
			                 this.myTransform.position, this.myTransform.rotation);
			
		}
	}
	
	[RPC]
	void updateMovement(Vector3 newPosition, Quaternion newRotation)
	{
		transform.position = newPosition;
		transform.rotation = newRotation;
	}
	/*
	[RPC]
	void serversUpdateMovement(NetworkPlayer nPlayer, int currentLevel, Vector3 newPosition, Quaternion newRotation)
	{


		for(int i = 0; i < this.playerDB.playersList.Count; i++)
		{
			if(nPlayer.ToString() != this.playerDB.playersList[i].NetworkPlayer.ToString() && currentLevel == this.playerDB.playersList[i].CurrentLevel)
			{
				if(playerDB.playersList[i].NetworkPlayer.ToString() == "0" && Network.isServer)
				{
					transform.position = newPosition;
					transform.rotation = newRotation;
				}
				else
				{
					networkView.RPC ("updateMovement", playerDB.playersList[i].NetworkPlayer, 
					                 newPosition, newRotation);
				}
			}
		}
	}
	*/
}
