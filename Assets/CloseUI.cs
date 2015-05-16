using UnityEngine;
using System.Collections;

public class CloseUI : MonoBehaviour {

	public GameObject[] panels;
	public KeyCode[] buttons;
	public GameObject menuEscape;
	private bool isPanelOpen = false;
	private bool isMenuEscapeOpen = false;

	public GameObject tradeUI;


	void Start()
	{
		if(!Network.isServer)
		{
			GameObject aux = menuEscape.transform.GetChild(0).FindChild("bt_cerrarMazmorra").gameObject;
			aux.SetActive(false);

			aux = menuEscape.transform.GetChild(0).FindChild("bt_explusarJugadores").gameObject;
			aux.SetActive(false);

		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			foreach (GameObject go in panels) {
				go.SetActive(false);
			}

			if(isPanelOpen)
				isPanelOpen = false;
			else if(isMenuEscapeOpen)
			{
				menuEscape.SetActive(false);
				if(menuEscape.GetComponent<ManageEscapeMenu>().GetCurrentMenu() != null)
				{
					menuEscape.GetComponent<ManageEscapeMenu>().GetCurrentMenu().SetActive(false);
					menuEscape.GetComponent<ManageEscapeMenu>().SetCurrentMenu(null);
				}
				isMenuEscapeOpen = false;
			}
			else
			{
				menuEscape.SetActive(true);
				isMenuEscapeOpen = true;
			}
				
		}

		if(!menuEscape.activeInHierarchy)
		{
			if (Input.GetKeyDown (buttons[0])) {
				if (panels[0].activeInHierarchy)
				{
					panels[0].SetActive(false);
					isPanelOpen = false;
				}
				else
				{
					isPanelOpen = true;
					panels[0].SetActive(true);
				}
			}
			if (Input.GetKeyDown (buttons[1])) {
				if (panels[1].activeInHierarchy)
				{
					panels[1].SetActive(false);
					isPanelOpen = false;

				}
				else
				{
					panels[1].SetActive(true);
					isPanelOpen = true;
				}
			}
			else if (Input.GetKeyDown (buttons[2])) {
				if (panels[2].activeInHierarchy)
				{
					panels[2].SetActive(false);
					isPanelOpen = false;
				}
				else
				{
					panels[2].SetActive(true);
					isPanelOpen = true;
				}
			}
			else if (Input.GetKeyDown (buttons[3])) {
				if (panels[3].activeInHierarchy)
				{
					panels[3].SetActive(false);
					isPanelOpen = false;
				}
				else
				{
					panels[3].SetActive(true);
					isPanelOpen = true;
				}
			}
			else if (Input.GetKeyDown (buttons[4])) {
				if (panels[4].activeInHierarchy)
				{
					panels[4].SetActive(false);
					isPanelOpen = false;
				}
				else
				{
					panels[4].SetActive(true);
					isPanelOpen = true;
				}
			}
		}
	}

	public bool IsPanelOpen {
		get {
			return isPanelOpen;
		}
		set {
			isPanelOpen = value;
		}
	}
}
