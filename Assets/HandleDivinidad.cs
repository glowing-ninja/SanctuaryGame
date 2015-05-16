using UnityEngine;
using System.Collections;

public class HandleDivinidad : SkillHandler {

	public float maxTime = 3.5f;
	
	private float time4Tick = 0.5f;
	private float time2Next = 0.0f;

	private ArrayList afectados;
	private ArrayList deletedEnemies;

	void Awake () {
		if (!GetComponent<NetworkView>().isMine)
			enabled = false;
	}

	// Use this for initialization
	void Start () {
		afectados = new ArrayList ();
		deletedEnemies = new ArrayList ();
		Invoke ("DestroyObject", maxTime);
	}
	
	void DestroyObject () {
		GameObject part = transform.FindChild ("DivinidadParticula").gameObject;
		part.transform.parent = null;
		Destroy (gameObject);
		Destroy (part, 2f);
	}
	
	void Update () {
		time2Next -= Time.deltaTime;
		if (time2Next <= 0) {
			time2Next = time4Tick;
			foreach (GameObject go in afectados) {
				if (go != null) {
					if (go.tag == "Player") {
						if (!go.GetComponent<NetworkView>().isMine)
							go.GetComponent<NetworkView>().RPC("Heal", go.GetComponent<NetworkView>().owner, heal, skillID);
						if (go.GetComponent<NetworkView>().isMine) {
							go.AddComponent<BuffHealthModifier>();
							go.GetComponent<BuffHealthModifier>().doHealing(heal);
						}
						
					}
					if (go.tag == "Enemy") {
						go.AddComponent<BuffHealthModifier>();
                        if (go.GetComponent<BuffHealthModifier>().doDamage(damage, jugador))
							deletedEnemies.Add(go);
					}
				}
			}
			foreach (GameObject go in deletedEnemies)
				afectados.Remove(go);
			deletedEnemies = new ArrayList ();
			time2Next = time4Tick;
		}
	}
	
	public void OnTriggerEnter(Collider other) {
		if (Network.isServer && (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")) {
			if (!afectados.Contains (other.gameObject))
				afectados.Add(other.gameObject);
		}
	}
	
	
	public void OnTriggerExit(Collider other) {
		if (GetComponent<NetworkView>().isMine && afectados.Contains(other.gameObject)) {
			afectados.Remove(other.gameObject);
		}
	}

	override public void Init(GameObject newPlayer, int skillID)
	{
		_healCoef = 0.5f;
		_dmgCoef = 0.2f;
        jugador = newPlayer;
		float healSt = newPlayer.GetComponent<Attributtes> ().stats [(int)Utils.Stat.CURA];
		float dmg = newPlayer.GetComponent<Attributtes> ().getTotalDamage ();

		this.heal = (int)(healSt * _healCoef + dmg * _dmgCoef);
		this.damage = (int)(healSt * _healCoef / 2f + dmg * _dmgCoef);
		this.skillID = skillID;
		//Destroy (gameObject, maxTime);
	}
}
