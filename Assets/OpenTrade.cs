using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenTrade : MonoBehaviour {

	public GameObject tradeUI;

	public void sendTradeRequest() {
		GameObject mpm = GameObject.Find ("MultiplayerManager");
		GameObject tm = GameObject.Find("TradeManager");
		PlayerDataBase pdb = mpm.GetComponent<PlayerDataBase>();
		NetworkPlayer np = pdb.getNetworkPlayer(transform.parent.GetComponent<Text>().text);
		if (np != null) {
			tm.GetComponent<NetworkView>().RPC("OpenTradeUI", np);
			tm.GetComponent<TradeHandler>().init (np);
			Camera.main.GetComponent<CloseUI>().tradeUI.SetActive(true);
			Utils.inventoryClick = Utils.ClickSystem.TRADE;
		}
	}

	[RPC]
	public void OpenTradeUI() {
		Camera.main.GetComponent<CloseUI>().tradeUI.SetActive(true);
		Utils.inventoryClick = Utils.ClickSystem.TRADE;
	}

	public void cancelTradeRequest() {
		PlayerDataBase pdb = GameObject.Find("MultiplayerManager").GetComponent<PlayerDataBase>();
		NetworkPlayer np = pdb.getNetworkPlayer(transform.parent.GetComponent<Text>().text);
		if (np != null) {
			Utils.player.GetComponent<NetworkView>().RPC("CloseTradeUI", np);
			Camera.main.GetComponent<CloseUI>().tradeUI.SetActive(false);
		}
	}

	[RPC]
	public void CloseTradeUI() {
		Camera.main.GetComponent<CloseUI>().tradeUI.SetActive(false);
	}
}
