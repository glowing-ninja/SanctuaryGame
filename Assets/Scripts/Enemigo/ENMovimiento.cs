using UnityEngine;
using System.Collections;

public class ENMovimiento : Pathfinding
{
    #region Declaración variables
    //Habilidades
    public GameObject[] habilidades;
    public Habilidad[] skillScripts;
    public const int SkillNumber = 1;
    protected float[] cooldown;
    public SkillThrower skillThrower;

    //Patrulla
    public float distanciaPatrulla;
    public float pausaPatrulla;
    public float distanciaMaxima;

    //Movimiento
    public float velocidadMovPatrulla;
    public float velocidadMovAtaque;
    public float velocidadMovReset;

    //Rango
    public float rangoAtaque;

    public bool destoryWhenSikill = false;

    // Estas variables son publicas para depurar
    public Vector3 posicionInicial;
    public Vector3 posPatrulla;
    public Attributtes tarAttri;

    //public float distancia;
    protected bool moverPatrulla = false;
    
    private bool yaMuerto = false;
    #endregion

    #region Funciones Iniciales
    void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        
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
            if (!yaMuerto)
                skillScripts[0].useWithCooldown();
            if (destoryWhenSikill)
            {
                estadisticas.PuntosSalud = -10;
                yaMuerto = true;
            }
            //Destroy(this.gameObject);
        }
        else
        {
            estadoActual = EstadosEnemigo.moverATarget;
            //FindPath (transform.position, target.transform.position);
        }
    }

    public override void DistanciaMaxima()
    {
        if ((estadoActual == EstadosEnemigo.ataque || estadoActual == EstadosEnemigo.moverATarget) && target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > distanciaMaxima)
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

    public override bool DeadthInExplosion()
    {
        return yaMuerto;
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
                Move(velocidadMovAtaque,false);
                break;
            case 2: // reset
                Move(velocidadMovReset,true);
                break;
            default:
                break;
        }
    }

    bool HaLlegado(Vector3 posicion)
    {
        //distancia = Vector3.Distance(transform.position, posicion);
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
