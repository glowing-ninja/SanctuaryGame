using UnityEngine;
using System.Collections;

public class SwitchScene : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player" && Network.isServer)
		{
			NetworkLevelLoading nLevelLoading =
				GameObject.Find("LevelLoader").GetComponent<NetworkLevelLoading>();

			if(nLevelLoading)
			{
				nLevelLoading.LoadNewLevel("Escena2");
			}

		}

		if(col.gameObject.tag == "Player" && Network.isClient)
		{
			Debug.Log("SOT CLIENT");
		}
	}
}
