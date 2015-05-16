using UnityEngine;
using System.Collections;

public class decorate : MonoBehaviour {

	public GameObject obj;
	public int numItems;
	public bool esquinas;
	public bool paredes;
	public bool centro;

	// Use this for initialization
	void Start () {
		Map mapa = GetComponent<MapGenerator> ().MazmorraCompleta[GetComponent<MapGenerator> ().actual];
		Vector3 pos = new Vector3();
		Quaternion rot = obj.transform.rotation;
		if (centro) {
			for (int i = 0; i < numItems; i++) {
				int x = Random.Range(0, 10);
				int z = Random.Range(0, 10);
				pos = new Vector3 (x * 20 + Random.Range (6, 16), 0.0f, z * 20 + Random.Range (6, 16));
				Instantiate (obj, pos, rot);
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
					Instantiate (obj, pos, rot);
				}
				if (!right) {
					pos = new Vector3 (20 * x + 15, 0.0f, 20 * z + Random.Range(5,16));
					Instantiate (obj, pos, rot);
				}
				if (!top) {
					pos = new Vector3 (20 * Random.Range(5,16) + 5, 0.0f, 20 * z + 15);
					Instantiate (obj, pos, rot);
				}
				if (!bot) {
					pos = new Vector3 (20 * Random.Range(5,16) + 5, 0.0f, 20 * z + 5);
					Instantiate (obj, pos, rot);
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
					Instantiate (obj, pos, rot);
				}
				if (!left && !bot) {
					pos = new Vector3 (20 * x + 5, 0.0f, 20 * z + 5);
					Instantiate (obj, pos, rot);
				}
				if (!bot && !right) {
					pos = new Vector3 (20 * x + 15, 0.0f, 20 * z + 5);
					Instantiate (obj, pos, rot);
				}
				if (!top && !right) {
					pos = new Vector3 (20 * x + 15, 0.0f, 20 * z + 15);
					Instantiate (obj, pos, rot);
				}
				//pos = new Vector3 (Random.Range (5, 16) * Random.Range (1, 10), 0.5f, Random.Range (5, 16) * Random.Range (1, 10));
				
			}
		}
	}
}
