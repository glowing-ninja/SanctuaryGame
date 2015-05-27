using UnityEngine;
using System.Collections;

public class ENRatExplosive : ENMovimiento
{
    void Awake()
    {
        posicionInicial = transform.position;
        if (GetComponent<NetworkView>().isMine)
        {
            habilidades = new GameObject[SkillNumber];
            this.skillScripts = new Habilidad[SkillNumber];
        }
        else
        {
            enabled = false;
        }
    }

	// Use this for initialization
	void Start () {
        target = null;
        posPatrulla = Vector3.zero;

        estadisticas = GetComponent<ENEstadisticas>();
        sisAmenaza = GetComponent<Amenaza>();
        if (pathfinder == null)
            pathfinder = GetComponentInParent<Pathfinder>();

        //Habilidades
        this.cooldown = new float[SkillNumber];
        this.skillThrower = GetComponent<SkillThrower>();
        this.skillScripts[0] = new ENRatExplosion();
        this.skillScripts[0].Init(this.gameObject, skillThrower);
	}
}
