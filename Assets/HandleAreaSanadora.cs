using UnityEngine;
using System.Collections;

public class HandleAreaSanadora : SkillHandler {

	float healModifier = 0.2f;
	
	public float maxTime = 5f;
	private float timeAlive = 0f;
	
	private float time4Tick = 0.5f;
	private float time2Next = 0.0f;
	
	private ArrayList afectados;

	//void Awake () {
	//	if (!networkView.isMine)
	//		enabled = false;
	//}

	void Start () {
		this.afectados = new ArrayList ();
		Destroy (gameObject, maxTime);
	}

	void Update () {
		time2Next -= Time.deltaTime;
		if (time2Next <= 0) {
			time2Next = time4Tick;
			foreach (GameObject go in afectados) {
				if (go.tag == "Player") {
					if (!go.GetComponent<NetworkView>().isMine)
						go.GetComponent<NetworkView>().RPC("Heal", go.GetComponent<NetworkView>().owner, heal, 9999);
					if (go.GetComponent<NetworkView>().isMine) {
						go.AddComponent<BuffHealthModifier>();
						go.GetComponent<BuffHealthModifier>().doHealing(heal);
					}
				}
			}
			time2Next = time4Tick;
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			afectados.Add(other.gameObject);
		}
	}
	
	
	public void OnTriggerExit(Collider other) {
		if (afectados.Contains(other.gameObject)) {
			afectados.Remove(other.gameObject);
		}
	}

	override public void Init(GameObject newPlayer, int skillID)
	{
		_healCoef = 0.3f;
		_dmgCoef = 0.1f;
		float healSt = newPlayer.GetComponent<Attributtes> ().getTotalStat (Utils.Stat.CURA);
		float dmg = newPlayer.GetComponent<Attributtes> ().getTotalDamage ();
		
		heal = (int)(healSt * _healCoef + dmg * _dmgCoef);
		
		Destroy (gameObject, 5f);
	}
}
