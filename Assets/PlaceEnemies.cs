using UnityEngine;
using System.Collections;

public class PlaceEnemies  : MonoBehaviour{
	
	public int numItems = 20;
	public bool esquinas;
	public bool paredes;
	public bool centro = true;
	public string[] enemyPaths;

	void Awake()
	{
		//obj = Resources.Load<GameObject> ("Prefab/ENEstatico");
	}

	public void Place (Transform parent, Map mapa) {
		//Map mapa = GetComponent<MapGenerator> ().MazmorraCompleta[GetComponent<MapGenerator> ().actual];
		Vector3 pos = new Vector3();
		mapa.enemyDatabase = new EnemyDatabase(numItems);
		NetworkViewID auxViewID;
		int pathIndex = 0;


		if (centro) {
			for (int i = 0; i < numItems; i++) {
				int x = Random.Range(0, 10);
				int z = Random.Range(0, 10);
				pos = new Vector3 (x * 20 + Random.Range (6, 16), 0.0f, z * 20 + Random.Range (6, 16));
				auxViewID = Network.AllocateViewID();
				mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
				mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
			}
		}
		if (paredes) {
			for (int i = 0; i < numItems / 2; i++) {
				int x = Random.Range(0, 10);
				int z = Random.Range(0, 10);
				
				bool left, bot, top, right;
				if (x == 0) left = false;
				else left = mapa.mapa[x-1, z].right;
				if (z== 0) bot = false;
				else bot = mapa.mapa[x, z-1].top;
				top = mapa.mapa[x,z].top;
				right = mapa.mapa[x,z].right;
				
				if (!left) {
					pos = new Vector3 (20 * x + 5, 0.0f, 20 * z + Random.Range(5,16));
					auxViewID = Network.AllocateViewID();
					mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
					mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
				}
				if (!right) {
					pos = new Vector3 (20 * x + 15, 0.0f, 20 * z + Random.Range(5,16));
					auxViewID = Network.AllocateViewID();
					mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
					mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
				}
				if (!top) {
					pos = new Vector3 (20 * Random.Range(5,16) + 5, 0.0f, 20 * z + 15);
					auxViewID = Network.AllocateViewID();
					mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
					mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
				}
				if (!bot) {
					pos = new Vector3 (20 * Random.Range(5,16) + 5, 0.0f, 20 * z + 5);
					auxViewID = Network.AllocateViewID();
					mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
					mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
				}
				//pos = new Vector3 (Random.Range (5, 16) * Random.Range (1, 10), 0.5f, Random.Range (5, 16) * Random.Range (1, 10));
				
			}
		}
		if (esquinas) {
			for (int i = 0; i < numItems; i++) {
				int x = Random.Range(0, 10);
				int z = Random.Range(0, 10);
				
				bool left, bot, top, right;
				
				if (x == 0) left = false;
				else left = mapa.mapa[x-1, z].right;
				if (z== 0) bot = false;
				else bot = mapa.mapa[x, z-1].top;
				top = mapa.mapa[x,z].top;
				right = mapa.mapa[x,z].right;
				
				
				if (!left && !top) {
					pos = new Vector3 (20 * x + 5, 0.0f, 20 * z + 15);
					auxViewID = Network.AllocateViewID();
					mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
					mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
				}
				if (!left && !bot) {
					pos = new Vector3 (20 * x + 5, 0.0f, 20 * z + 5);
					auxViewID = Network.AllocateViewID();
					mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
					mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
				}
				if (!bot && !right) {
					pos = new Vector3 (20 * x + 15, 0.0f, 20 * z + 5);
					auxViewID = Network.AllocateViewID();
					mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
					mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
				}
				if (!top && !right) {
					pos = new Vector3 (20 * x + 15, 0.0f, 20 * z + 15);
					auxViewID = Network.AllocateViewID();
					mapa.enemyDatabase.AddEnemy(i, pos, auxViewID);
					mapa.enemyDatabase.EnemyList[i].enemyPath = this.enemyPaths[(int)Random.Range(0, this.enemyPaths.Length)];
				}
				//pos = new Vector3 (Random.Range (5, 16) * Random.Range (1, 10), 0.5f, Random.Range (5, 16) * Random.Range (1, 10));
				
			}
		}
}


}
