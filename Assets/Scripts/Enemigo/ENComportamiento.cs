using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ENComportamiento : MonoBehaviour {
    public Amenaza sisAmenaza;
	public GameObject modelo;

	protected ENEstadisticas estadisticas;
	public Attributtes targetAttri;

	public enum EstadosEnemigo {
		none,
		inicio,
		patrulla,
		moverATarget,
		moverAInicio,
		ataque
	}
	
	public EstadosEnemigo estadoActual;
	
	public GameObject target;
	public GameObject grupo;
	
	// Use this for initialization
	void Start () {
		grupo = null;
		estadoActual = EstadosEnemigo.inicio;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (Network.isServer) {
			switch (estadoActual) {
			case EstadosEnemigo.inicio:
				Iniciar ();
				break;
			case EstadosEnemigo.patrulla:
				Patrullar ();
				break;
			case EstadosEnemigo.moverATarget:
				MoverATarget ();
				break;
			case EstadosEnemigo.moverAInicio:
				MoverAPuntoInicial ();
				break;
			case EstadosEnemigo.ataque:
				Atacar ();
				break;
			default:
				break;
			}
			ActualizarTarget ();
			DistanciaMaxima ();
		}
	}
	
	public virtual void Iniciar(){}
	public virtual void Patrullar(){}
	public virtual void MoverATarget(){}
	public virtual void MoverAPuntoInicial(){}
	public virtual void Atacar(){}
	public virtual void DistanciaMaxima(){}
	public virtual void AtaqueConjunto (){}
	public virtual void CambiarTarget() {}

	protected bool PuedoVerlo(Vector3 enemigo, Vector3 target){
		RaycastHit hit;
		Vector3 direccion = target - enemigo;
		float dist = Vector3.Distance (target,enemigo);
		Ray lookRay = new Ray (enemigo, direccion);
		
		if (Physics.Raycast (lookRay, out hit,dist)) {
			if (hit.collider.tag == "Destructible")
			{
				return false;
			}
		}
		return true;
	}

	public GameObject GetTarget() {
		return target;
	}

	public void SetGrupo(GameObject gr) {
		grupo = gr;
	}

	public GameObject GetGrupo() {
		return grupo;
	}

	private void ActualizarTarget(){
		if (sisAmenaza != null && sisAmenaza.HayTargets())
			CambiarTarget ();
		//target = sisAmenaza.GetTargetMasAmenaza();
	}

	public void ActualizarAmenaza(GameObject player, float dmg, float mod) {
		sisAmenaza.ActualizarAmenaza (player, dmg, mod);
	}

	public GameObject GetModelo() {
		return modelo;
	}

    public GameObject GetCanvasEnemigo()
    {
        return estadisticas.GetCanvasEnemigo();
    }
}