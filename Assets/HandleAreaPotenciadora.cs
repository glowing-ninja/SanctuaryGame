using UnityEngine;
using System.Collections;

public class HandleAreaPotenciadora : SkillHandler {

	public float maxTime = 4f;
	private float timeAlive = 0;
	
	private float time4Tick = 0.5f;
	private float time2Next = 0.0f;
	private BuffAreaPotenciadora buff;
	GameObject explosion;

	void Awake () {
		if (!GetComponent<NetworkView>().isMine)
			enabled = false;
	}
	
	// Use this for initialization
	void Start () {
		this.explosion = Resources.Load ("Habilidad/Heal/LightExplosion") as GameObject;
		this.skillID = Random.Range (0, 10000);
	}
	
	
	void Update() {
		timeAlive += Time.deltaTime;
	}
	
	public void OnTriggerEnter(Collider other) {
		if (GetComponent<NetworkView>().isMine && other.gameObject.tag == "Player") {
			
			other.gameObject.GetComponent<NetworkView>().RPC("ApplyBuff", RPCMode.All, "BuffAreaPotenciadora",
			                                 heal,
			                                 maxTime-timeAlive,
			                                 skillID);
		}
		if (GetComponent<NetworkView>().isMine && other.gameObject.tag == "Enemy") {
			BuffHealthModifier bhm = other.gameObject.AddComponent<BuffHealthModifier>();
            bhm.doDamage(damage, jugador);
			Instantiate (explosion, other.gameObject.transform.position, explosion.transform.rotation);
			Destroy(gameObject);
		}
	}
	
	public void OnTriggerExit(Collider other) {
		if (GetComponent<NetworkView>().isMine && other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<NetworkView>().RPC("RevertBuff", RPCMode.All, "BuffAreaPotenciadora", skillID );
		}
	}

	override public void Init(GameObject newPlayer, int skillID)
	{
		_healCoef = 1f;
		_dmgCoef = 1f;
        jugador = newPlayer;
		float healSt = newPlayer.GetComponent<Attributtes> ().stats [(int)Utils.Stat.CURA];
		float dmg = newPlayer.GetComponent<Attributtes> ().getTotalDamage ();
		
		damage = (int)(healSt * _healCoef / 2f + dmg * _dmgCoef);
		heal = (int)(healSt * _healCoef + dmg * _dmgCoef);
		
		Destroy (gameObject, maxTime);
	}
}
