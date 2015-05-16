using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InitUI : MonoBehaviour {

	public bool isMenu;

	public Button bCrearPartida;
	public Button bUnirsePartida;

	// Use this for initialization
	void Start () 
	{
		if (isMenu) {
			MultiplayerScript mScript = GameObject.Find ("MultiplayerManager").GetComponent<MultiplayerScript> ();

			bCrearPartida.onClick.AddListener (() => {
				mScript.crearServidor ();});
			bUnirsePartida.onClick.AddListener (() => {
				mScript.unirseServidor ();});
		} else {
			SQLite sqlScript = GameObject.Find("BD").GetComponent<SQLite>();
			bCrearPartida.onClick.AddListener (() => {
				sqlScript.updatePlayer ();});
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
