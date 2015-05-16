using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthTransform : MonoBehaviour {

	private RectTransform healthTransform;
	private Text healthText;
	private Text maxHealthText;

	private float cacheY;
	private float minXValue;
	private float maxXValue;

	private Image visualHealth;

	private Attributtes atr;

	private bool onCD = false;
	private float coolDown = 1f;

	void Awake () {
		if (!GetComponent<NetworkView>().isMine) {
			//enabled = false;
		}
	}

	void Start() {
		
		atr = gameObject.GetComponent<Attributtes> ();
		visualHealth = GameObject.Find ("Health").GetComponent<Image> ();
		healthTransform = visualHealth.gameObject.GetComponent<RectTransform>();
		
		healthText = GameObject.Find ("lb_Hp").GetComponent<Text>();
		maxHealthText = GameObject.Find("lb_HpMax").GetComponent<Text>();
		
		cacheY = healthTransform.position.y;
		minXValue = 172 - healthTransform.rect.width;
		maxXValue = 172;
		
		HandleHealth (true);
	}

	public void HandleHealth (bool recovery) {
		if (GetComponent<NetworkView>().isMine) {
			atr.MaxHealth = atr.getTotalStat (Utils.Stat.AGUANTE) * Attributtes.VIDA_POR_STA;
			if (recovery) {
				atr.health = atr.MaxHealth;
			}	
			if (atr.health > atr.MaxHealth)
				atr.health = atr.MaxHealth;

			healthText.text = atr.health + "";
			maxHealthText.text = " / " + atr.MaxHealth;

			float currentXValue = MapValues (atr.health, 0, atr.MaxHealth, minXValue, maxXValue);
			healthTransform.position = new Vector3 (currentXValue, cacheY);

			if (atr.health > atr.MaxHealth / 2) {
				visualHealth.color = new Color32 ((byte)MapValues (atr.health, atr.MaxHealth / 2, atr.MaxHealth, 255, 0), 255, 0, 255);
			} else {
				visualHealth.color = new Color32 (255, (byte)MapValues (atr.health, 0, atr.MaxHealth / 2, 0, 255), 0, 255);
			}

			int[] totalStats = new int[5];
			totalStats [(int)Utils.Stat.FUERZA] = atr.getTotalStat (Utils.Stat.FUERZA);
			totalStats [(int)Utils.Stat.MAGIA] = atr.getTotalStat (Utils.Stat.MAGIA);
			totalStats [(int)Utils.Stat.DESTREZA] = atr.getTotalStat (Utils.Stat.DESTREZA);
			totalStats [(int)Utils.Stat.CURA] = atr.getTotalStat (Utils.Stat.CURA);
			totalStats [(int)Utils.Stat.AGUANTE] = atr.getTotalStat (Utils.Stat.AGUANTE);
			
			GetComponent<NetworkView>().RPC("UpdateOthersStats", RPCMode.Others, Utils.player.name, totalStats, atr.health, atr.MaxHealth);

		}
	}

	private float MapValues (float x, float inMin, float inMax, float outMin, float outMax ) {
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

	void OnTriggerStay (Collider other) {
		if (other.name == "Damage") {
			if (!onCD && atr.health > 0) {
				StartCoroutine(doDamage ());
				atr.doDamage(10);
			}
		}
		if (other.name == "Heal") {
			if (!onCD && atr.health < atr.MaxHealth) {
				StartCoroutine(doDamage ());
				atr.doDamage(-10);
			}
		}
	}

	IEnumerator doDamage() {
		onCD = true;
		yield return new WaitForSeconds(coolDown);
		onCD = false;
	}

	[RPC]
	void UpdateOthersStats(string playerName, int[] newStats, int health, int maxhealth)
	{
		atr = gameObject.GetComponent<Attributtes> ();
		if (name == playerName) {
			atr.stats = newStats;
			atr.health = health;
			atr.MaxHealth = maxhealth;
			GameObject[] partyUI = GameObject.FindGameObjectsWithTag ("PartyUI") as GameObject[];
			foreach (GameObject player in partyUI) {
				if (player.name == playerName + "UI") {
					player.transform.GetChild(0).GetComponent<UpdatePartyHealth>().UpdateHealth(playerName);
				}
			}
		}
	}
}
