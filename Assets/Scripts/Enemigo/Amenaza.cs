using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Objetivo {
	public GameObject jugador;
	public float amenaza;

	
	public Objetivo(GameObject j, float a) {
		jugador = j;
		amenaza = a;
	}
}

public class Amenaza : MonoBehaviour {

	private IList<Objetivo> objetivos;
    // Debug
    public bool mostrarAmenaza = false;
	public string[] amenazas;

	public Amenaza() {
		objetivos = new List<Objetivo> ();
	}

	// Use this for initialization
	void Start () {
		objetivos = new List<Objetivo> ();
		amenazas = new string[4];
		//amenazas = new float[4];
		amenazas[0] = "(vacio)";
		amenazas[1] = "(vacio)";
		amenazas[2] = "(vacio)";
		amenazas[3] = "(vacio)";
		//objetivos = new Dictionary<GameObject, float> ();
	}

	void Update(){
        if (mostrarAmenaza)
        {
            for (int i = 0; i < amenazas.Count(); i++)
            {
                if (objetivos.Count > 0 && i < objetivos.Count())
                {
                    amenazas[i] = objetivos[i].jugador.name + " --> " + objetivos[i].amenaza;
                }
            }
        }
	}

    private void ActualizarAmenazaGenerico(Objetivo obj, GameObject jug, string nom_jug, float dam, float mod)
    {
        int jugador;

        if (obj == null)
        {
            GameObject w_jugador;
            if (jug == null)
            {
                w_jugador = GameObject.Find(nom_jug);
            }
            else
            {
                w_jugador = jug;
            }
            Objetivo nuevo_objetivo = new Objetivo(w_jugador, 0);
            objetivos.Add(nuevo_objetivo);
            jugador = objetivos.IndexOf(nuevo_objetivo);
        }
        else
        {
            jugador = objetivos.IndexOf(obj);
        }
        Decimal w_aux = (Decimal)dam * (Decimal)mod;
        objetivos[jugador].amenaza += (float)Math.Round(w_aux, 2);
        objetivos = objetivos.OrderByDescending(ob => ob.amenaza).ToList();
    }

	public void ActualizarAmenaza(GameObject jug, float dam, float mod) {
		Objetivo w_obj = objetivos.FirstOrDefault (ob => ob.jugador == jug);
        ActualizarAmenazaGenerico(w_obj, jug, null, dam, mod);
	}


    public void ActualizarAmenaza(string jug, float dam, float mod)
    {
        Objetivo w_obj = objetivos.FirstOrDefault(ob => ob.jugador.name == jug);
        ActualizarAmenazaGenerico(w_obj, null, jug, dam, mod);
    }
	
	public GameObject GetTargetMasAmenaza(){
		if (objetivos.Count () <= 0)
			return null;
		return objetivos.First ().jugador;
	}

	public bool ExisteJugador(GameObject jug) {
		Objetivo w_obj = objetivos.FirstOrDefault (ob => ob.jugador == jug);

		if (w_obj == null)
			return false;

		return true;
	}

	public float GetAmenazaJugador(GameObject jug) {
		Objetivo w_obj = objetivos.FirstOrDefault (ob => ob.jugador == jug);
		
		if (w_obj == null)
			return -1;
		
		return w_obj.amenaza;
	}

	public float GetAmenazaPorJugador(GameObject jug) {
		float w_ame_jug = GetAmenazaJugador (jug);
		float w_ame_first;
		if (w_ame_jug == -1)
			return w_ame_jug;
		w_ame_first = objetivos.First ().amenaza;
		if (w_ame_first == 0)
			return 1;
		return w_ame_jug / w_ame_first;
	}

	public void EliminarJugador(GameObject jug){
        Objetivo w_obj = objetivos.FirstOrDefault(ob => ob.jugador == jug);
        int w_indes = objetivos.IndexOf(w_obj);
		objetivos.RemoveAt (objetivos.IndexOf(objetivos.FirstOrDefault (ob => ob.jugador == jug)));
	}

	public bool HayTargets() {
		if (objetivos.Count > 0)
			return true;
		return false;
	}
}