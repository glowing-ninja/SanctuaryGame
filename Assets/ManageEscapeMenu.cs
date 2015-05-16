using UnityEngine;
using System.Collections;

public class ManageEscapeMenu : MonoBehaviour {

	private GameObject CurrentMenu;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public GameObject GetCurrentMenu()
	{
		return this.CurrentMenu;
	}

	public void SetCurrentMenu(GameObject current)
	{
		this.CurrentMenu = current;
	}
}
