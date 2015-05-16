using UnityEngine;
using System.Collections;

public class BossHandler : ENComportamiento {

	public bool fight = false;
	public float moveSpeed = 0.05f;

	public GameObject AlientoFuego;
	public GameObject BolaDeFuego;
	public GameObject IncubadoraHuevo;

	public GameObject BolaDeFuegoAnimacion;

	public Transform boca;

	public bool onCd = false;
	public bool enrage = false;

	public int numOleadasBolasDeFuego = 3;
	public int numsBolasDeFuego = 5;
	public float timeBetweenBalls = 4f;
	public float cdAlientoFuego = 15f;
	public float duracionAlientoFuego = 5f;

	public float maxPV;
	public Animator anim;

	public Transform bossArea;

	public GameObject[] playersInCombat;

	private ENEstadisticas stats;

	public GameObject puente;

	public GameObject addsParent;

	public void waitingCombat () {
		estadoActual = EstadosEnemigo.none;
	}

	public override void Iniciar(){
		//StopAllCoroutines ();
		//target = null;
		estadoActual = EstadosEnemigo.ataque;
		//sisAmenaza = new Amenaza();
		
		StartCoroutine("HandleAlientoFuego");
		StartCoroutine("HandleIncubadoraHuevo");
		StartCoroutine("HandleBolaDeFuego");
	}

	public override void Atacar(){
		//Lanzar habilidad
		if (target == null) {
		//	estadoActual = EstadosEnemigo.inicio;
		}
		else if (Vector3.Distance(transform.position, target.transform.position) >= 6) {
			anim.SetBool("Walk", true);
			estadoActual = EstadosEnemigo.moverATarget;
		}
		else {
			// ataque
			modelo.transform.LookAt(new Vector3(target.transform.position.x,
			                                    modelo.transform.position.y,
			                                    target.transform.position.z));
			//if (TargetMuerto())
			//	EliminaTarget();
		}
	}

	private void ActualizarTarget(){
		if (sisAmenaza != null && sisAmenaza.HayTargets())
			CambiarTarget ();
		//target = sisAmenaza.GetTargetMasAmenaza();
	}
	
	public void ActualizarAmenaza(GameObject player, float dmg, float mod) {
		sisAmenaza.ActualizarAmenaza (player, dmg, mod);
	}

	void Start () {
		stats = gameObject.GetComponent<ENEstadisticas>();
		maxPV = stats.maxHealth;
		//playersInCombat = GameObject.FindGameObjectsWithTag ("Player") as GameObject[];
		//foreach ( GameObject go in pl
	}

	public override void CambiarTarget(){
		GameObject target_aux;
		target_aux = sisAmenaza.GetTargetMasAmenaza ();
		if (target_aux == null) {
			Debug.Log("Me reinicio");
			puente.transform.position = new Vector3 (puente.transform.position.x, -0.67f, puente.transform.position.z);
			GameObject.Find("BossArea").GetComponent<SpawnEnemy>().bossSpawn();
			Destroy(this.gameObject);
		} else {
			if (target != target_aux) {
				target = target_aux;
				targetAttri = target.GetComponent<Attributtes> ();
			}
		}
	}

	public override void MoverATarget () {
		if (Vector3.Distance(transform.position, target.transform.position) < 6) {
			estadoActual = EstadosEnemigo.ataque;
			anim.SetBool("Walk", false);
		}
		else {
			transform.Translate(modelo.transform.forward * moveSpeed);
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
			case EstadosEnemigo.inicio:
				Iniciar ();
				break;
			case EstadosEnemigo.moverATarget:
				MoverATarget ();
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

		if (stats.PuntosSalud <= maxPV * 0.1f && !enrage) {
			if (!onCd) {
				enrage = true;
				//gameObject.renderer.material.color = Color.red;
				StopCoroutine("HandleAlientoFuego");
				StopCoroutine("HandleIncubadoraHuevo");
				StopCoroutine("HandleBolaDeFuego");
				numOleadasBolasDeFuego = 1000;
				numsBolasDeFuego = 10;
				timeBetweenBalls = 2f;
				StartCoroutine("HandleBolaDeFuego");
			}
		}
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

	public IEnumerator HandleBolaDeFuego() {
		float percent = 0.7f;
		while (true) {
			yield return new WaitForSeconds(0.1f);
			if (stats.PuntosSalud <= maxPV * percent) {
				Debug.Log("BOLA PREPARADA");
				while(onCd) {
					yield return new WaitForSeconds(0.1f);
				}
				onCd = true;
				moveSpeed = 0.0f;
				//GameObject inst1 = Instantiate(BolaDeFuegoAnimacion, boca.position, Quaternion.identity) as GameObject;
				for (int i = 0; i < numOleadasBolasDeFuego; i++) {
					anim.SetBool("BolasDeFuego", true);
					yield return new WaitForSeconds(1f);
					anim.speed = 0f;
					GameObject inst1 = Instantiate(BolaDeFuegoAnimacion, boca.position, boca.rotation) as GameObject;
					Destroy(inst1, 2f);
					for (int j = 0; j < numsBolasDeFuego; j++) {
						float x = bossArea.position.x + Random.Range(-14,14);
						float y = 0;
						float z = bossArea.position.z + Random.Range(-14,14);
						Vector3 pos = new Vector3(x,y,z);
						GameObject inst2 = Instantiate(BolaDeFuego, pos, Quaternion.identity) as GameObject;
					}
					
					numsBolasDeFuego++;
					Debug.Log("LANZO BOLA");
					yield return new WaitForSeconds(1f);
					anim.speed = 1f;
					yield return new WaitForSeconds(1f);
					anim.SetBool("BolasDeFuego", false);
					yield return new WaitForSeconds(timeBetweenBalls - 2f);
					Debug.Log("TERMINO BOLA");
				}
				
				numsBolasDeFuego-=2;
				onCd = false;
				moveSpeed = 0.05f;
				percent -= 0.25f;
			}
		}
	}

	public IEnumerator HandleAlientoFuego() {
		while (true) {
			yield return new WaitForSeconds(cdAlientoFuego - duracionAlientoFuego);
			Debug.Log("Aliento preparado");
			while(onCd) {
				yield return new WaitForSeconds(0.1f);
			}
			onCd = true;
			moveSpeed = 0.0f;
			anim.SetBool("AlientoDeFuego", true);
			yield return new WaitForSeconds(1f);
			anim.speed = 0f;
			GameObject inst = Instantiate(AlientoFuego, boca.position, boca.rotation) as GameObject;
			inst.transform.SetParent(boca, true);
			//inst.transform.LookAt(Vector3.forward);
			Debug.Log("LANZO ALIENTO");
			yield return new WaitForSeconds(duracionAlientoFuego);
			anim.speed = 1f;
			Debug.Log("TERMINO ALIENTO");
			Destroy(inst,0f);
			onCd = false;
			moveSpeed = 0.05f;
			yield return new WaitForSeconds(1f);
			anim.SetBool("AlientoDeFuego", false);
		}
	}

	public IEnumerator HandleIncubadoraHuevo() {
		float percent = 0.75f;
		while (true) {
			yield return new WaitForSeconds(0.1f);
			if (stats.PuntosSalud <= maxPV * percent) {
				Debug.Log("INCUBADORA PREPARADA");
				while(onCd) {
					yield return new WaitForSeconds(0.1f);
				}
				ArrayList randomPos = new ArrayList();
				GameObject[] incubadoras = GameObject.FindGameObjectsWithTag("BossMechanic") as GameObject[];
				int numPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
				// HACER QUE APAREZCA UN HUEVO POR PLAYER

				Vector3 pos = incubadoras[Random.Range(0, incubadoras.Length)].transform.position;

				//Vector3 pos = GameObject.Find("IncubadoraPosition").transform.position;
				GameObject inst = Instantiate(IncubadoraHuevo, pos, IncubadoraHuevo.transform.rotation) as GameObject;

				if(this.addsParent != null)
					inst.transform.parent = this.addsParent.transform;

				onCd = true;
				Debug.Log("LANZO INCUBADORA");
				yield return new WaitForSeconds(4f);
				Debug.Log("TERMINO INCUBADORA");
				onCd = false;
				percent -= 0.25f;
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (!fight && other.tag == "Player") {
			fight = true;
			target = other.gameObject;
			targetAttri = target.GetComponent<Attributtes> ();
			estadoActual = EstadosEnemigo.inicio;
			puente.transform.position = new Vector3 (puente.transform.position.x, -2.6f, puente.transform.position.z);
		}
	}
}
