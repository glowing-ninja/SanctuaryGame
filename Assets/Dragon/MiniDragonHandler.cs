using UnityEngine;
using System.Collections;

public class MiniDragonHandler : ENComportamiento {

	public bool onCd = false;
	public GameObject attack;
	public Transform thrower;
	bool fight = false;

	void Start () {
		estadoActual = EstadosEnemigo.ataque;
		StartCoroutine ("BasicAttack");
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
			sisAmenaza.ActualizarAmenaza (go, 1f, 0f);
		}
		CambiarTarget ();
	}
	
	public override void Atacar(){
		//Lanzar habilidad
		if (target == null) {
			//	estadoActual = EstadosEnemigo.inicio;
		}
		else {
			// ataque
			modelo.transform.LookAt(new Vector3(target.transform.position.x,
			                                    modelo.transform.position.y,
			                                    target.transform.position.z));
		}
	}

	// Update is called once per frame
	void Update () {
		if (Network.isServer) {
			switch (estadoActual) {
			case EstadosEnemigo.none:
				break;
			case EstadosEnemigo.ataque:
				Atacar ();
				break;
			default:
				break;
			}
			if (target != null && TargetMuerto())
				EliminaTarget();
			ActualizarTarget ();
			DistanciaMaxima ();
		}
	}

	
	public override void CambiarTarget(){
		GameObject target_aux;
		target_aux = sisAmenaza.GetTargetMasAmenaza ();
		if (target_aux == null) {
			Destroy(this.gameObject);
		} else {
			if (target != target_aux) {
				target = target_aux;
				targetAttri = target.GetComponent<Attributtes> ();
			}
		}
	}

	private void ActualizarTarget(){
		if (sisAmenaza != null && sisAmenaza.HayTargets())
			CambiarTarget ();
		//target = sisAmenaza.GetTargetMasAmenaza();
	}
	
	void EliminaTarget() {
		sisAmenaza.EliminarJugador(target);
		CambiarTarget ();
	}
	
	bool TargetMuerto() {
		if (targetAttri.health <= 0)
			return true;
		return false;
	}

	/*void OnTriggerEnter(Collider other) {
		if (!fight && other.tag == "Player") {
			fight = true;
			estadoActual = EstadosEnemigo.ataque;
			StartCoroutine ("BasicAttack");
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
				sisAmenaza.ActualizarAmenaza (go, 1f, 0f);
			}
			CambiarTarget ();
		}
	}*/

	public IEnumerator BasicAttack() {
		while (true) {
			if (!onCd) {
				onCd = true;
				yield return new WaitForSeconds(3f);
				GameObject instantiated = GameObject.Instantiate(attack,
				                                                 thrower.position,
				                                                 transform.rotation) as GameObject;
				instantiated.GetComponent<MRU>().setTarget(target);
				onCd = false;
			}
		}
	}
}
