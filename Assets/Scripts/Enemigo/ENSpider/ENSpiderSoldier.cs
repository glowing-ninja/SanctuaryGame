using UnityEngine;
using System.Collections;

public class ENSpiderSoldier : Pathfinding
{
    #region Declaración variables
    //Habilidades
    public GameObject[] habilidades;
    public Habilidad[] skillScripts;
    private const int SkillNumber = 1;
    private float[] cooldown;
    private SkillThrower skillThrower;

    //Patrulla
    public float distanciaPatrulla;
    public float pausaPatrulla;
    //public float rangoAtaque;
    public float distanciaMaxima;

    //Movimiento
    public float velocidadMovPatrulla;
    public float velocidadMovAtaque;
    public float velocidadMovReset;

    //Rango
    public float rangoAtaque;

    // Estas variables son publicas para depurar
    public Vector3 posicionInicial;
    public Vector3 posPatrulla;
    public Attributtes tarAttri;

    public float distancia;
    public bool moverPatrulla = false;
    #endregion

    #region Funciones Iniciales
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
    void Start()
    {
        target = null;
        posPatrulla = Vector3.zero;
        
        estadisticas = GetComponent<ENEstadisticas>();
        sisAmenaza = GetComponent<Amenaza>();
        if (pathfinder == null)
            pathfinder = GetComponentInParent<Pathfinder>();
        
        //Habilidades
        this.cooldown = new float[SkillNumber];
        this.skillThrower = GetComponent<SkillThrower>();
        //this.skillScripts[0] = new ENDBGolpe();
        this.skillScripts[0] = new ENDBDisparo();
        this.skillScripts[0].Init(this.gameObject, skillThrower);
    }
    #endregion
    #region Funciones OVERRIDE
    public override void Iniciar()
    {
        posicionInicial = transform.position;
        StopAllCoroutines();
        target = null;
        estadisticas.ResetVida();
        StartCoroutine("Patrulla");
        estadoActual = EstadosEnemigo.patrulla;
    }
    public override void Patrullar()
    {
        if (target != null)
        {
            StopCoroutine("Patrulla");
            StartCoroutine("ActualizarMovimiento");
            estadoActual = EstadosEnemigo.moverATarget;
        }
        else
        {
            if (moverPatrulla)
            {
                MoverGenerico(0);
                if (HaLlegado(posPatrulla))
                    moverPatrulla = false;
            }
        }
    }
    public override void MoverATarget()
    {
        if (EnRango(target.transform))
        {
            estadoActual = EstadosEnemigo.ataque;
        }
        else
        {
            MoverGenerico(1);
            //RotarModelo(target.transform.position);
        }
    }
    public override void MoverAPuntoInicial()
    {
        if (HaLlegado(posicionInicial))
            estadoActual = EstadosEnemigo.inicio;
        else
            MoverGenerico(2);
        //RotarModelo(target.transform.position);
    }
    public override void Atacar()
    {
        if (EnRango(target.transform))
        {
            // Ataco
            //Debug.Log("Estoy atacando");
            skillScripts[0].useWithCooldown();
        }
        else
        {
            estadoActual = EstadosEnemigo.moverATarget;
            //FindPath (transform.position, target.transform.position);
        }
    }

    public override void DistanciaMaxima()
    {
        if (estadoActual == EstadosEnemigo.ataque && target != null)
        {
            if (Vector3.Distance(transform.position, posicionInicial) > distanciaMaxima)
            {
                EliminaTarget();
            }
        }
    }

    public override void CambiarTarget()
    {
        GameObject target_aux;
        target_aux = sisAmenaza.GetTargetMasAmenaza();
        if (target_aux == null)
        {
            targetAttri = null;
            estadoActual = EstadosEnemigo.moverAInicio;
            StopCoroutine("ActualizarMovimiento");
            FindPath(transform.position, posicionInicial);
        }
        else
        {
            if (target != target_aux)
            {
                //estadoActual = EstadosEnemigo.moverATarget;
                target = target_aux;
                targetAttri = target.GetComponent<Attributtes>();
                //FindPath (transform.position, target.transform.position);
            }
        }
    }
    #endregion
    #region CORRUTINAS
    // Coorutinas
    IEnumerator Patrulla()
    {
        //Debug.Log ("Inicio corruinta Patrulla");
        while (true)
        {
            yield return new WaitForSeconds(pausaPatrulla);
            Debug.Log("Nueva poscion para matrulla");
            Vector3 nuevaPosicion = new Vector3(Random.Range(posicionInicial.x - distanciaPatrulla, posicionInicial.x + distanciaPatrulla),
                                                 posicionInicial.y,
                                                 Random.Range(posicionInicial.z - distanciaPatrulla, posicionInicial.z + distanciaPatrulla));
            //Debug.Log("Me muevo a: " + nuevaPosicion);
            if (!Physics.Raycast(transform.position, nuevaPosicion - transform.position, distanciaPatrulla))
            {
                if (!moverPatrulla)
                {
                    posPatrulla = nuevaPosicion;
                    moverPatrulla = true;
                }
            }//else Debug.Log("No me he movido!!!!");
        }
    }

    IEnumerator ObjetivoVisible(GameObject tar)
    {
        while (true)
        {
            if (PuedoVerlo(transform.position, tar.transform.position))
            {
                estadisticas.ActualizarAmenaza(tar.name, 1.0f, 0.0f);
                //sisAmenaza.ActualizarAmenaza (tar, 1.0f, 0.0f);
                CambiarEstadoAtacar(tar);
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator ActualizarMovimiento()
    {
        while (true)
        {
            if (target != null && !EnRango(target.transform))
                FindPath(transform.position, target.transform.position);

            yield return new WaitForSeconds(1.0f);
        }
    }
    #endregion
    #region TRIGGERS
    //Triggers 
    void OnTriggerEnter(Collider other)
    {
        if (GetComponent<NetworkView>().isMine)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(ObjetivoVisible(other.gameObject));
            }
        }
    }
    #endregion
    #region Funciones AUXILIARES
    //Funciones auxiliares
    public void CambiarEstadoAtacar(GameObject tar)
    {
        //estadoActual = EstadosEnemigo.moverATarget;
        target = sisAmenaza.GetTargetMasAmenaza();
        //FindPath (transform.position, target.transform.position);
        tarAttri = target.GetComponent<Attributtes>();
        //if (enemigoUI != null)
        //	enemigoUI.UpdateImgAmenaza ();
        if (grupo != null)
            AtaqueConjunto();
    }

    bool EnRango(Transform target)
    {
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist > rangoAtaque) return false;

        return true;
    }

    void MoverGenerico(int tipo)
    {
        switch (tipo)
        {
            case 0: // patrulla
                if (posPatrulla != Vector3.zero)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        posPatrulla,
                        Time.deltaTime * velocidadMovPatrulla);
                    RotarModelo(posPatrulla);
                }
                break;
            case 1: // ataque
                Move(velocidadMovAtaque);
                break;
            case 2: // reset
                Move(velocidadMovReset);
                break;
            default:
                break;
        }
    }

    bool HaLlegado(Vector3 posicion)
    {
        distancia = Vector3.Distance(transform.position, posicion);
        if (Vector3.Distance(transform.position, posicion) <= 1.5f)
            return true;
        return false;
    }

    void EliminaTarget()
    {
        sisAmenaza.EliminarJugador(target);
        CambiarTarget();
    }
    #endregion
}
