using UnityEngine;
using System.Collections;

public class ENSpiderQueen : ENComportamiento
{
    #region Declaracion variables
    //Habilidades
    private const int skillNumber = 2;
    public GameObject[] habilidades;
    public Habilidad[] skillScripts;
    private float[] coolDown;
    private SkillThrower skillThrower;

    //Patrulla
    public float pausaPatrulla;
    public Vector3 posPatrulla;
    public bool moverPatrulla = false;

    //Rotacion
    public float smothRotation = 6.0f;
    public float smothRotationPatrol = 0.1f;
    public float angulo;
    private bool smooth = true;

    //Rango
    public float rangoAtaque;
    public float rangoAtaqueArea;
    #endregion
    #region Funciones Iniciales
    /***********************/
    /* FUNCIONES INICIALES */
    /***********************/
    void Awake()
    {
        if (GetComponent<NetworkView>().isMine)
        {
            habilidades = new GameObject[skillNumber];
            this.skillScripts = new Habilidad[skillNumber];
        }
        else
        {
            enabled = false;
        }
    }
	// Use this for initialization
	void Start () {
        target = null;
        
        this.coolDown = new float[skillNumber];
        this.skillThrower = GetComponent<SkillThrower>();
        sisAmenaza = GetComponent<Amenaza>();
        estadisticas = GetComponent<ENEstadisticas>();
        this.skillScripts[0] = new ENDBDisparo();
        this.skillScripts[0].Init(this.gameObject, skillThrower);
        this.skillScripts[1] = new ENSpiderArea();
        this.skillScripts[1].Init(this.gameObject, skillThrower);

        //EstablecerCanvas();
	}
    #endregion
    #region Funciones OVERRIDE
    /**********************/
    /* FUNCIONES OVERRIDE */
    /**********************/
    public override void Iniciar()
    {
        StopAllCoroutines();
        target = null;

        StartCoroutine("Patrulla");
        estadisticas.ResetVida();
        estadoActual = EstadosEnemigo.patrulla;
    }

    public override void Patrullar()
    {
        if (target != null)
        {
            StopCoroutine("Patrulla");
            estadoActual = EstadosEnemigo.ataque;
        }
        else
        {
            moverPatrulla = RotarPatrulla();
        }
    }

    public override void Atacar()
    {
        if (target == null)
        {
            estadoActual = EstadosEnemigo.inicio;
        }
        else
        {
            int habilidad = SeleccionarAtaque();
            switch (habilidad)
            {
                case 0:
                    this.skillScripts[0].useWithCooldown();
                    break;
                case 1:
                    this.skillScripts[1].useWithCooldown();
                    //this.skillScripts[1].useWithCooldown(); no implementado todavía
                    break;
                default:// No hago nada
                    break;
            }

            Rotar();
            if (TargetMuerto())
            {
                EliminaTarget();
            }
        }
    }

    public override void DistanciaMaxima()
    {
        if (estadoActual == EstadosEnemigo.ataque && target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > rangoAtaque)
            {
                EliminaTarget();
            }
        }
    }

    public override void AtaqueConjunto()
    {
        grupo.GetComponent<ENGrupo>().SetAtaqueGrupo(target);
    }

    public override void CambiarTarget()
    {
        GameObject target_aux;
        target_aux = sisAmenaza.GetTargetMasAmenaza();
        if (target_aux == null)
            estadoActual = EstadosEnemigo.inicio;
        else
        {
            if (target != target_aux)
            {
                target = target_aux;
                targetAttri = target.GetComponent<Attributtes>();
            }
        }
    }
    #endregion
    #region TRIGGERS
    /************/
    /* TRIGGERS */
    /************/
    void OnTriggerEnter(Collider other)
    {
        if (GetComponent<NetworkView>().isMine && other.tag == "Player")
        {
            StartCoroutine(ObjetivoVisible(other.gameObject));
        }
    }
    #endregion
    #region CORRUTINAS
    /**************/
    /* CORRUTINAS */
    /**************/
    IEnumerator Patrulla()
    {
        while (true)
        {
            yield return new WaitForSeconds(pausaPatrulla);
            Vector3 modeloPosition = modelo.transform.position;
            Vector3 nuevaPosicion = new Vector3(
                        Random.Range(modeloPosition.x - 10, modeloPosition.x + 10),
                        modelo.transform.position.y,
                        Random.Range(modeloPosition.z - 10, modeloPosition.z + 10));
            if (!moverPatrulla)
            {
                posPatrulla = nuevaPosicion;
                moverPatrulla = true;
            }
        }
    }
    IEnumerator ObjetivoVisible(GameObject pe_target)
    {
        while (true)
        {
            if (PuedoVerlo(transform.position, pe_target.transform.position))
            {
                //estadisticas.SetBarraVida(enemigoUI);
                estadisticas.ActualizarAmenaza(pe_target.name, 1.0f, 0.0f);

                CambiarEstadoAtacar(pe_target);
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    #endregion
    #region Funciones AUXILIARES
    /***********************/
    /* FUNCIONES AUXILIARES*/
    /***********************/
    public void CambiarEstadoAtacar(GameObject pe_target)
    {
        estadoActual = EstadosEnemigo.ataque;
        target = sisAmenaza.GetTargetMasAmenaza();
        targetAttri = target.GetComponent<Attributtes>();

        if (grupo != null)
        {
            AtaqueConjunto();
        }
    }

    // Elimina el target del sistema de amenaza y busca el siguiente
    void EliminaTarget()
    {
        sisAmenaza.EliminarJugador(target);
        CambiarTarget();
    }

    bool TargetMuerto()
    {
        if (targetAttri.health <= 0)
            return true;
        return false;
    }

    void Rotar()
    {
        if (smooth)
        {
            Quaternion rot = Quaternion.LookRotation(target.transform.position - modelo.transform.position, Vector3.up);
            modelo.transform.rotation = Quaternion.Slerp(modelo.transform.rotation, rot, Time.deltaTime * smothRotation);
        }
        else
        {
            modelo.transform.LookAt(new Vector3(target.transform.position.x,
                                        modelo.transform.position.y,
                                        target.transform.position.z));
        }
    }

    bool RotarPatrulla()
    {
        angulo = Vector3.Angle(posPatrulla - modelo.transform.position, modelo.transform.forward);
        if (angulo < 2.0f)
        {
            return false;
        }
        else
        {
            Quaternion rot = Quaternion.LookRotation(posPatrulla - modelo.transform.position, Vector3.up);
            modelo.transform.rotation = Quaternion.Slerp(modelo.transform.rotation, rot, Time.deltaTime * smothRotationPatrol);
        }
        return true;
    }

    int SeleccionarAtaque()
    {
        float distancia = Vector3.Distance(transform.position, target.transform.position);
        if (distancia > rangoAtaqueArea)
        {
            return 0;
        }
        return 1;
    }
#endregion
}
