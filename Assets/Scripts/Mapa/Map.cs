using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Map {
	
	public int TAM = 10;
	public newPiezaMap[,] mapa;
	public ChestDatabase chestDatabase;
	public EnemyDatabase enemyDatabase;
	public float tpX;
	public float tpY;
	public float tpZ;

	[NonSerialized]
	public GameObject terrainParent;
	
	
	public Map () {
		mapa = new newPiezaMap[TAM, TAM];
	}

	public Vector3 GetTpPosition()
	{
		return new Vector3(this.tpX, this.tpZ, this.tpY);
	}

	public void SetTpPosition(Vector3 position)
	{
		this.tpX = position.x;
		this.tpY = position.y;
		this.tpY = position.z;
	}

}
