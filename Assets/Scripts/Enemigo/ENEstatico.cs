using UnityEngine;
using System.Collections;

public class ENEstatico : ENComportamiento {

	public float rangoAtaque;
	
	public GameObject[] habilidades;
	public Habilidad[] skillScripts;
	public float pausaPatrulla;
	public Vector3 posPatrulla;
	public bool moverPatrulla = false;
	private const int SkillNumber = 1;
	private float[] cooldown;
	private SkillThrower skillThrower;
	//public Attributtes targetAttri;
	private bool smooth = true;
	public float smothRotation = 6.0f;
	public float smothRotationPatrol = 0.1f;
	public float angulo;
	
	void Awake(){
		if(GetComponent<NetworkView>().isMine)
		{
			habilidades = new GameObject[SkillNumber];
			this.skillScripts = new Habilidad[SkillNumber];
		}
		else
		{
			enabled = false;
		}
	}

	void Start () {
		target = null;
		
		this.cooldown = new float[SkillNumber];
		this.skillThrower = GetComponent<SkillThrower>();
		sisAmenaza = GetComponent<Amenaza> ();
		estadisticas = GetComponent<ENEstadisticas> ();
		this.skillScripts [0] = new ENDBDisparo ();
		this.skillScripts [0].Init (this.gameObject, skillThrower);
	}
	
	public override void Iniciar(){
		StopAllCoroutines ();
		target = null;
		StartCoroutine ("Patrulla");
		estadisticas.ResetVida ();
		estadoActual = EstadosEnemigo.patrulla;
	}
	public override void Patrullar(){
		if (target != null) {
			StopCoroutine ("Patrulla");
			estadoActual = EstadosEnemigo.ataque;
		} else {
			moverPatrulla =RotarPatrulla();
		}
	}
	
	public override void Atacar(){
		//Lanzar habilidad
		if (target == null) {
			estadoActual = EstadosEnemigo.patrulla;
		}
		else {
			//this.skillScripts [0].Init (this.gameObject, skillThrower);
			this.skillScripts[0].useWithCooldown();

			if (smooth) {
				Quaternion rot = Quaternion.LookRotation(target.transform.position-modelo.transform.position,Vector3.up);
				modelo.transform.rotation = Quaternion.Slerp(modelo.transform.rotation,rot,Time.deltaTime*smothRotation);
			} else {
				modelo.transform.LookAt(new Vector3(target.transform.position.x,
		                                   modelo.transform.position.y,
				                           target.transform.position.z));
			}
			if (TargetMuerto())
				EliminaTarget();
		}
	}
	public override void DistanciaMaxima ()
	{
		if (estadoActual == EstadosEnemigo.ataque && target != null) {
			if (Vector3.Distance (transform.position, target.transform.position) > rangoAtaque) {
				EliminaTarget();
			}
		}
	}

	public override void AtaqueConjunto() {
		grupo.GetComponent<ENGrupo> ().SetAtaqueGrupo (target);
	}

	public override void CambiarTarget(){
		GameObject target_aux;
		target_aux = sisAmenaza.GetTargetMasAmenaza ();
		if (target_aux == null)
			estadoActual = EstadosEnemigo.inicio;
		else {
			if (target != target_aux) {
				target = target_aux;
				targetAttri = target.GetComponent<Attributtes> ();
			}
		}
	}

	//Corrutina
	IEnumerator ObjetivoVisible(GameObject tar) {
		while (true) {
			
			if (PuedoVerlo(transform.position,tar.transform.position)) {
                estadisticas.ActualizarAmenaza(tar.name, 1.0f, 0.0f);
				//sisAmenaza.ActualizarAmenaza (tar, 1.0f, 0.0f);
				CambiarEstadoAtacar(tar);
				break;
			}
			yield return new WaitForSeconds (1.0f);
		}
	}

	IEnumerator Patrulla(){
		//Debug.Log ("Inicio corruinta Patrulla");
		while (true) {
			yield return new WaitForSeconds (pausaPatrulla);
			Debug.Log("Nueva poscion para matrulla");
			Vector3 nuevaPosicion = new Vector3 (Random.Range (modelo.transform.position.x - 10, modelo.transform.position.x + 10),
			                                     modelo.transform.position.y,
			                                     Random.Range (modelo.transform.position.z - 10, modelo.transform.position.z + 10));

			if (!moverPatrulla) {
				posPatrulla = nuevaPosicion;
				moverPatrulla = true;
			}
		}
	}

	//Triggers 
	void OnTriggerEnter(Collider other) {
		if (GetComponent<NetworkView>().isMine) {
			if (other.tag == "Player") {
				StartCoroutine (ObjetivoVisible (other.gameObject));
			}
		}
	}

	//Funciones auxiliares
	public void CambiarEstadoAtacar(GameObject tar){
		estadoActual = EstadosEnemigo.ataque;
		target = sisAmenaza.GetTargetMasAmenaza ();
		targetAttri = target.GetComponent<Attributtes> ();
		//if (enemigoUI != null)
		//	enemigoUI.UpdateImgAmenaza ();
		if (grupo != null)
			AtaqueConjunto ();
	}
	// Elimina el target del sistema de amenaza y busca el siguiente
	void EliminaTarget() {
		sisAmenaza.EliminarJugador(target);
		CambiarTarget ();
	}

	bool TargetMuerto() {
		if (targetAttri.health <= 0)
			return true;
		return false;
	}

	bool RotarPatrulla() {
		angulo = Vector3.Angle (posPatrulla - modelo.transform.position, modelo.transform.forward);
		if (angulo < 2.0f) {
			return false;
		} else {
			Quaternion rot = Quaternion.LookRotation(posPatrulla-modelo.transform.position,Vector3.up);
			modelo.transform.rotation = Quaternion.Slerp(modelo.transform.rotation,rot,Time.deltaTime*smothRotationPatrol);
		}
		return true;
	}
}