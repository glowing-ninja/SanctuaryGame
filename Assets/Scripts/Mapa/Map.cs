using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Map {
	
	public int TAM = 10;
	public newPiezaMap[,] mapa;
	public ChestDatabase chestDatabase;
	public EnemyDatabase enemyDatabase;
	[NonSerialized]
	public GameObject terrainParent;
	
	
	public Map () {
		mapa = new newPiezaMap[TAM, TAM];
	}
}
