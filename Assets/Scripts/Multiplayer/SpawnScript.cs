using UnityEngine;
using System.Collections;

/// <summary>
/// Spawn script.
/// This script is attached to the SpawnManager and it allows the player
/// to spawn into the multiplayer game
/// 
/// This script is accessed by the FireBlaster script in determining
/// which team the player is on
/// This script is accesed by HealthAndDamage script
/// This script is accessed by AssignHealth script to see if firtsSpawn 
/// is true
/// </summary>


public class SpawnScript : MonoSingleton<SpawnScript> {
	
	
	//_____Variables start______
	//The player prefabs are connected to thes in the inspector
	public Transform player;
	public Transform instantiatedPlayer;
	private int teamPlayer = 0;
	
	//Used to capture spawn points
	private GameObject[] spawnPoints;
	
	//Used in determinating wheter the player is destroyed
	public bool iAmDestroyed = false;
	
	//Used to determinate if the player has spawn for the first time
	private bool firtsSpawn = true;
	//_____Variables end______
	
	
	public void OnNetworkLoadedLevel() {
		if(this.firtsSpawn)
			SpawnPlayer ();
	}
	
	public void SpawnPlayer()
	{
		//Find spawn points and place a reference to them in the array.
		this.spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		
		//Randomly select one
		GameObject randomSpawnPoint = this.spawnPoints[Random.Range(0, this.spawnPoints.Length)];
		
		if(this.firtsSpawn)
		{
			//Instantiate the player in the selected spawn point
			instantiatedPlayer = Network.Instantiate(this.player, randomSpawnPoint.transform.position,
			                                         randomSpawnPoint.transform.rotation, this.teamPlayer) as Transform;
			this.firtsSpawn = false;
			Utils.player = instantiatedPlayer.gameObject;

		}
		else
		{
			this.instantiatedPlayer.position = randomSpawnPoint.transform.position;
		}		
	}
}
