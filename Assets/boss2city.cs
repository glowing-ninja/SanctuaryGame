using UnityEngine;
using System.Collections;

public class boss2city : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			if(other.GetComponent<NetworkView>().isMine)
			{
				Vector3 pos = new Vector3 (77,0, 31);
				Utils.player.transform.position = pos;
			}
		}
	}
}
