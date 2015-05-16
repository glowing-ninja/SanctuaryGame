using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tipo_Enemigo{
	public int cantidad;
	public GameObject tipo;
}

public class ENGrupo : MonoBehaviour {

	//Tipos de enmigos
	public Tipo_Enemigo[] tiposEnemigo;
	public float areaEfecto = 10;
	public Vector3 posicionInicial;
	
	public IList<GameObject> enemigos;
	
	// Use this for initialization
	void Start () {
		enemigos = new List<GameObject> ();
		if (posicionInicial == Vector3.zero) {
			posicionInicial = transform.position;
		}
		if (tiposEnemigo.Length > 0) CrearEnemigos ();
	}
	
	//Funciones auxiliares
	void CrearEnemigos (){
		int num_Enemigos;
		Vector3 nuevaPosicion;
		Quaternion rot;
		GameObject clone;
		foreach (Tipo_Enemigo t_enmigo in tiposEnemigo) {
			num_Enemigos = 0;
			while (num_Enemigos < t_enmigo.cantidad){
				nuevaPosicion = new Vector3(Random.Range(posicionInicial.x - areaEfecto, posicionInicial.x + areaEfecto),
				                            posicionInicial.y,
				                            Random.Range(posicionInicial.z - areaEfecto, posicionInicial.z + areaEfecto));
				rot = t_enmigo.tipo.transform.rotation;
				clone = Instantiate(t_enmigo.tipo,nuevaPosicion,rot) as GameObject;
				clone.transform.parent = this.transform;
				clone.GetComponent<ENComportamiento>().SetGrupo(this.gameObject);
				enemigos.Add(clone);
				num_Enemigos++;
			}
		}
	}
	
	public void SetAtaqueGrupo(GameObject tar){
		foreach (GameObject en in enemigos) {
			en.GetComponent<ENComportamiento>().target = tar;
		}
	}

}
