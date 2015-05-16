using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowName : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text> ().text = PlayerPrefs.GetString ("playerName");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
