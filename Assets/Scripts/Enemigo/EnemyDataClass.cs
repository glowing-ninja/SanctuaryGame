using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class EnemyDataClass 
{
	private float positionX;
	private float positionY;
	private float positionZ;
	public bool isDead;
	public string enemyPath;
	public int level;

	[NonSerialized]
	public NetworkViewID viewID;
	[NonSerialized]
	public GameObject enemy;
	//Necesitaremos un tipo

	public EnemyDataClass(Vector3 newPosition, NetworkViewID nViewID)
	{
		this.positionX = newPosition.x;
		this.positionY = newPosition.y;
		this.positionZ = newPosition.z;
		this.viewID = nViewID;
		this.isDead = false;
	}


	public Vector3 getPosition()
	{
		return new Vector3(this.positionX, this.positionY,
		                   this.positionZ);
	}

	public void setPath(string path)
	{
		this.enemyPath = path;
	}
}
