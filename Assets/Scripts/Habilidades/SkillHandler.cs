using UnityEngine;
using System.Collections;

public class SkillHandler : MonoBehaviour {
	public float _strCoef = 0f;
	public float _spCoef = 0f;
	public float _dexCoef = 0f;
	public float _healCoef = 0f;
	public float _staCoef = 0f;

	public float _dmgCoef = 0f;

	public int damage = 0;
	public int heal = 0;
	public int skillID = 0;
	// quien ha lanzado la habilidad
	public GameObject jugador;
	
	// Update is called once per frame
	virtual protected void Update () {
	
	}

	virtual public void Init(GameObject newPlayer, int skillID) {
	}
}
