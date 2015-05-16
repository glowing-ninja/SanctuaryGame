using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExperienceTransform : MonoBehaviour {
	
	private RectTransform expTransform;
	private Text expText;
	private Text maxExpText;
	
	private float cacheY;
	private float minXValue;
	private float maxXValue;
	
	private Image visualExp;
	
	private Attributtes atr;
	
	private bool onCD = false;
	private float coolDown = 0.05f;
	
	void Awake () {
		if (!GetComponent<NetworkView>().isMine)
			enabled = false;
	}

	// Use this for initialization
	void Start () {
		visualExp = GameObject.Find ("Exp").GetComponent<Image> ();
		expTransform = visualExp.gameObject.GetComponent<RectTransform>();
		
		expText = GameObject.Find ("lb_Exp").GetComponent<Text>();
		maxExpText = GameObject.Find("lb_ExpMax").GetComponent<Text>();
		
		atr = gameObject.GetComponent<Attributtes> ();
		cacheY = expTransform.position.y;
		minXValue = -expTransform.rect.width / 2;
		maxXValue = expTransform.rect.width / 2;
		
		HandleExp ();
	}
	
	public void HandleExp () {
		
		float currentXValue = MapValues (atr.expActual, 0, atr.expTotal, minXValue, maxXValue);
		expTransform.position = new Vector3 (currentXValue, cacheY);

		
		
		expText.text = atr.expActual + "";
		maxExpText.text = " / " + atr.expTotal;
	}
	
	private float MapValues (float x, float inMin, float inMax, float outMin, float outMax ) {
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

	void OnTriggerStay (Collider other) {
		if (other.name == "ExpCollider") {
			if (!onCD) {
				StartCoroutine(getExp ());
				atr.addExp(50 * atr.level);
			}
		}
	}
	
	IEnumerator getExp() {
		onCD = true;
		yield return new WaitForSeconds(coolDown);
		onCD = false;
	}
}
