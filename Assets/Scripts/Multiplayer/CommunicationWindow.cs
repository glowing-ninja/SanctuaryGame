using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CommunicationWindow : MonoBehaviour {

	private List<string> chatEntries = new List<string>();
	private InputField chatInput;
	private Text chatWindow;
	private bool writingMessage = false;
	private string message = "";
	private string playerName = "";
	private const int maxEntries = 50;

	public GameObject MainCamera;

	// Use this for initialization
	void Start () 
	{
		this.chatInput = GameObject.Find("if_chatInput").GetComponent<InputField>();
		this.chatWindow = GameObject.Find("lb_chat").GetComponent<Text>();
		this.playerName = Utils.playerName;
		this.chatInput.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Return) && !this.writingMessage)
		{
			this.chatInput.gameObject.SetActive(true);
			this.chatInput.Select();
			this.chatInput.ActivateInputField();
			this.writingMessage = true;
			this.DisableInputs();
		}
		else
		if(Input.GetKeyDown(KeyCode.Return) && this.writingMessage)
		{
			this.writingMessage = false;
			this.message = this.chatInput.text;
			this.chatInput.DeactivateInputField();
			this.chatInput.gameObject.SetActive(false);
			this.EnableInputs();

			if(this.message != "")
			{
				GetComponent<NetworkView>().RPC("SendMessageToEveryone", RPCMode.All,
				                this.message, this.playerName);
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape) && this.writingMessage)
		{
			this.writingMessage = false;
			this.chatInput.DeactivateInputField();
			this.chatInput.gameObject.SetActive(false);
			this.EnableInputs();
		}
	}

	[RPC]
	void SendMessageToEveryone(string message, string pName)
	{


		if(this.chatEntries.Count < maxEntries)
		{
			this.chatWindow.text = this.chatWindow.text + pName + ": " + message + "\n";
		}
		else
		{
			this.chatWindow.text = this.chatWindow.text.Remove(0, this.chatEntries[0].ToString().Length) + pName + ": " + message + "\n";
			this.chatEntries.RemoveAt(0);
		}

		this.chatEntries.Add(pName + ": " + message + "\n");
	}

	void DisableInputs()
	{
		Utils.player.GetComponent<TPController> ().enabled = false;
		Utils.player.GetComponent<followMouse> ().enabled = false;
		Utils.player.GetComponent<Teclado> ().enabled = false;
		this.MainCamera.GetComponent<CloseUI> ().enabled = false;

	}

	void EnableInputs()
	{
		Utils.player.GetComponent<TPController> ().enabled = true;
		Utils.player.GetComponent<followMouse> ().enabled = true;
		Utils.player.GetComponent<Teclado> ().enabled = true;
		this.MainCamera.GetComponent<CloseUI> ().enabled = true;
	}
}
