using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InitUI : MonoBehaviour {

	public bool isMenu;

	public Button bCrearPartida;
	public Button bUnirsePartida;
	public Button bCrearPJ;
	public Button bUnirsePJ;

	// Use this for initialization
	void Start () 
	{
		if (isMenu) {
			MultiplayerScript mScript = GameObject.Find ("MultiplayerManager").GetComponent<MultiplayerScript> ();

			bCrearPartida.onClick.AddListener (() => {
				mScript.crearServidor ();});
			bUnirsePartida.onClick.AddListener (() => {
				mScript.unirseServidor ();});
			bCrearPJ.onClick.AddListener (() => {
				mScript.crearPartidaOffline ();});
			bUnirsePJ.onClick.AddListener (() => {
				mScript.unirseServidorCrearNuevo ();});
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
