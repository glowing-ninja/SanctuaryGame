using UnityEngine;
//using UnityEditor;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public Map[] MazmorraCompleta;
	public static int TAM = 5;
	public int actual;
	public int depth;
	public GameObject Dungeon;
	public GameObject mazDown;
	//public newPiezaMap[,] mapa;

	public GameObject wall;

	private Pathfinder pathFinder;

	public static bool randomBoolean () {
		if (Random.value >= 0.5f) {
			return true;
		}
		return false;
	}

	public static Map mapGenerator() {
		
		Map m = new Map();

		m.mapa [0, 0] = new newPiezaMap (true, false, false, true);
		m.mapa [0, TAM -1] = new newPiezaMap (false, true, false, true);
		m.mapa [TAM - 1, 0] = new newPiezaMap (true, false, true, false);
		m.mapa [TAM - 1, TAM - 1] = new newPiezaMap (false, true, true, false);
		
		int i, j;
		i = 0;
		for (j = 1; j < TAM-1; j++) {
			m.mapa [i, j] = new newPiezaMap ();
			m.mapa [i, j].left = false;
			
			do {
				m.mapa[i,j].top = randomBoolean();
				m.mapa[i,j].right = randomBoolean();
			} while (!m.mapa[i,j].top && !m.mapa[i,j].right);
		}
		
		
		j = 0;
		for (i = 1; i < TAM - 1; i++) {
			m.mapa [i, j] = new newPiezaMap ();
			m.mapa [i, j].down = false;
			
			do {
				m.mapa[i,j].top = randomBoolean();
				m.mapa[i,j].right = randomBoolean();
			} while (!m.mapa[i,j].top && !m.mapa[i,j].right);
		}
		
		for (i = 1; i < TAM - 1; i++) {
			for (j = 1; j < TAM - 1; j++) {
				m.mapa [i, j] = new newPiezaMap ();
				
				do {
					m.mapa[i,j].top = randomBoolean();
					m.mapa[i,j].right = randomBoolean();
				} while (!m.mapa[i,j].top && !m.mapa[i,j].right);
			}
		}
		
		i = TAM - 1;
		for (j = 1; j < TAM - 2; j++) {
			m.mapa [i, j] = new newPiezaMap ();
			
			if (!m.mapa [i - 1, j].right  || !m.mapa[i, j - 1].top)
				m.mapa [i, j].top = true;
			else
				m.mapa [i, j].top = randomBoolean();
		}
		
		j = TAM - 1;
		for (i = 1; i < TAM - 2; i++) {
			m.mapa [i, j] = new newPiezaMap ();
			
			if (!m.mapa [i, j - 1].top || !m.mapa[i, j - 1].right) 
				m.mapa[i,j].right = true;
			else
				m.mapa[i,j].right = randomBoolean();
		}
		
		m.mapa [TAM - 1, TAM - 2] = new newPiezaMap (true, true, true, false);
		m.mapa [TAM - 2, TAM - 1] = new newPiezaMap (false, true, true, true);

		float tpX = TAM-1, tpY = TAM-1;
		if (Random.Range(0,2) == 0) {
			tpX = TAM-1;
			tpY = Random.Range(0, TAM);
		}
		else {
			tpY = TAM-1;
			tpX = Random.Range(0, TAM);
		}
		Vector3 tpPos = new Vector3(tpX * 20 + 10, 0f, tpY * 20 + 10);
		m.SetTpPosition(tpPos);


		return m;
	}



	// Use this for initialization
	void Start () {
		actual = 0;
		//wall = Resources.Load<GameObject> ("Prefab/Wall");
		//GameObjectUtility.SetStaticEditorFlags (wall, StaticEditorFlags.NavigationStatic);
		if(Network.isServer)
		{
			//int lv = GameObject.FindGameObjectWithTag ("Player").GetComponent<Attributtes> ().level;
			int lv = 1;
			
			depth = Mathf.Max (2, 1 + lv / 3 );
			//depth = 1	;

			MazmorraCompleta = new Map[depth];
			GameObject terrain = GameObject.Find("Level_0") as GameObject;
			terrain.name = "Level_0";
			terrain.transform.position = new Vector3(Utils.mapOffset, 0, Utils.mapOffset);
			//newPiezaMap[,] mapa = new newPiezaMap[TAM, TAM];
			MazmorraCompleta[0] = mapGenerator ();
			Render (MazmorraCompleta [0].mapa, terrain.transform, MazmorraCompleta[0].GetTpPosition());

			// pathfinder
			pathFinder = terrain.GetComponent<Pathfinder>();
			pathFinder.CrearMapa(new Vector2(terrain.transform.position.x,
			                                 terrain.transform.position.z),
			                     new Vector2(terrain.transform.position.x+200,
			            					 terrain.transform.position.z+200));

			PlaceEnemies pEnemies = this.gameObject.GetComponent<PlaceEnemies>();
			pEnemies.Place(terrain.transform, MazmorraCompleta[0]);
			this.GenerateChests(0, terrain);
		}
	}

	public void Render(newPiezaMap[,] mapa, Transform parent, Vector3 positionTP) {
		int i, j;
		Debug.Log ("RENDER");
		GameObject auxObject = null;

		parent.SetParent(Dungeon.transform, false);

		for (i = 0; i < TAM; i++) {
			for (j = 0; j < TAM; j++) {
				Vector3 pos = new Vector3 (i * 20, 0, j * 20);
				if (!mapa [i, j].top)
				{
					auxObject = Instantiate (wall, pos + 20 * Vector3.forward, wall.transform.rotation) as GameObject;
					auxObject.transform.SetParent(parent, false);
				}
				if (!mapa [i, j].down)
				{
					auxObject = Instantiate (wall, pos, wall.transform.rotation) as GameObject;
					auxObject.transform.SetParent(parent, false);
				}
				if (!mapa [i, j].left)
				{
					auxObject = Instantiate (wall, pos, Quaternion.AngleAxis (-90, Vector3.up)) as GameObject;
					auxObject.transform.SetParent(parent, false);
				}
				if (!mapa [i, j].right)
				{
					auxObject = Instantiate (wall, pos + 20 * Vector3.right, Quaternion.AngleAxis (-90, Vector3.up)) as GameObject;
					auxObject.transform.SetParent(parent, false);
				}
			}
		}

		GameObject tp = Instantiate(mazDown, positionTP, mazDown.transform.rotation) as GameObject;
		tp.transform.SetParent(parent, false);
	}

	public void SpawnChest(int levelIndex, Transform parent)
	{
		if(MazmorraCompleta[levelIndex].chestDatabase != null)
		{
			int[] chestRarity = MazmorraCompleta[levelIndex].chestDatabase.getRarity();
			Vector3[] chestPosition = MazmorraCompleta[levelIndex].chestDatabase.getPosition();
			for (int i = 0; i < TAM / 5; i++) {
				ChestSpawn.Spawn(chestRarity[i], chestPosition[i], parent);
				//Instantiate(chest, new Vector3 (10 + Random.Range(0, TAM) * 20, 1, 10 + Random.Range(0, TAM) * 20), chest.transform.rotation);
			}
		}
	}

	void OnNetworkLoadedLevel(NetworkPlayer nPlayer)
	{
		if(actual < depth)
		{
			if(nPlayer.ToString() == Network.player.ToString())
			{
				if(actual != -1)
				{
					Utils.player.transform.position = new Vector3(15f + 250 * (actual + 1), 1, 10f + 250 * (actual + 1));
					

				}
			}
		}
		else if(actual >= depth  && depth != 0)
		{
			//Utils.player.transform.position = new Vector3(10f + 250f * (depth), 1, 440f);
		}

	}

	public void GenerateChests(int level, GameObject parent)
	{
		int i;
		MazmorraCompleta[level].chestDatabase = new ChestDatabase(TAM / 5);
		int[] chestRarity = MazmorraCompleta[level].chestDatabase.getRarity();
		for (i = 0; i < TAM / 5; i++) {
			MazmorraCompleta[level].chestDatabase.AddChest(i, 2, new Vector3 (10 + Random.Range(0, TAM) * 20, 0, 10 + Random.Range(0, TAM) * 20));
			ChestSpawn.Spawn(chestRarity[i], MazmorraCompleta[level].chestDatabase.getPositionAt(i), parent.transform);
			//Instantiate(chest, new Vector3 (10 + Random.Range(0, TAM) * 20, 1, 10 + Random.Range(0, TAM) * 20), chest.transform.rotation);
		}
	}
}
