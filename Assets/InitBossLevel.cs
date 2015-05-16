using UnityEngine;
using System.Collections;

public class InitBossLevel : MonoBehaviour
{

	void Awake()
	{
		GameObject obj = GameObject.Find("Dungeon") as GameObject;
		int depth = obj.GetComponent<MapGenerator>().depth + 1;
		this.gameObject.transform.position = new Vector3(depth * Utils.mapOffset, 0f, depth * Utils.mapOffset);
		this.gameObject.transform.SetParent(obj.transform, false);
	}
}
