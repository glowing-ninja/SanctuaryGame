using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdatePartyHealth : MonoBehaviour {

	private RectTransform healthTransform;
	private Text healthText;
	private Text maxHealthText;
	
	private float cacheY;
	private float minXValue;
	private float maxXValue;
	
	private Image visualHealth;

	void Start() {		
		visualHealth = transform.GetChild(0).FindChild("ActualHealth").GetComponent<Image> ();
		healthTransform = visualHealth.gameObject.GetComponent<RectTransform>();
		
		healthText = transform.FindChild("lb_ActualHealth").GetComponent<Text>();
		maxHealthText = transform.FindChild("lb_TotalHealth").GetComponent<Text>();
		
		cacheY = healthTransform.position.y;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		maxXValue = healthTransform.position.x;
	}

	public void UpdateHealth (string playerName) {
		Attributtes atr = GameObject.Find (playerName).GetComponent<Attributtes> ();
		HandleHealth (atr);
	}
	
	public void HandleHealth (Attributtes atr) {
		
		healthText.text = atr.health + "";
		maxHealthText.text = " / " + atr.MaxHealth;
		
		float currentXValue = MapValues (atr.health, 0, atr.MaxHealth, minXValue, maxXValue);
		healthTransform.position = new Vector3 (currentXValue, cacheY);
		
		if (atr.health > atr.MaxHealth / 2) {
			visualHealth.color = new Color32 ((byte)MapValues (atr.health, atr.MaxHealth / 2, atr.MaxHealth, 255, 0), 255, 0, 255);
		} else {
			visualHealth.color = new Color32 (255, (byte)MapValues (atr.health, 0, atr.MaxHealth / 2, 0, 255), 0, 255);
		}
	}
	
	private float MapValues (float x, float inMin, float inMax, float outMin, float outMax ) {
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}