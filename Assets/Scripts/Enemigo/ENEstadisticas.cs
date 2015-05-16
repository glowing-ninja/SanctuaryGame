using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ENEstadisticas : MonoBehaviour {

	//_____Variables start______
	public int Entereza;
	public int Fuerza;
	public int Destreza;
	public int DHechizos;
	public int Sanacion;
	public int Armadura;
	public int DañoBase;
	public int Experiencia;
	public float PuntosSalud;
	public float maxHealth;
	//public int VAtaque
	public int Nivel;
	
	public float AFuego;
	public float AAgua;
	public float ARayo;
	public float ALuz;
	public float AOscuridad;
	//private MultiplayerScript multiplayerScript;
	//_____Variables end________
	// Use this for initialization
	private Transform rotationTransform;

	public GameObject sct;
	public ENBarraVida barraVida;
    private Amenaza sisAmenaza;

    protected GameObject enemyCanvas;
    public GameObject prefabCanvasEnemigo;
    public GameObject canvasEnemigo;
    private Transform canvasAncla;
    public bool barraVida2D;

    public string nombreEnemigo;

	void Awake()
	{
		//canvas = this.GetComponent<Canvas> ();
		//multiplayerScript = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerScript>();
	}
	
	void Start () {
		this.PuntosSalud = Entereza * 10f;
		this.maxHealth = this.PuntosSalud;
		this.rotationTransform = gameObject.transform.FindChild("Enemigo") as Transform;
        sisAmenaza = GetComponent<Amenaza>();
        EstablecerCanvas();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetBarraVida(ENBarraVida pe_barraVida)
    {
        if (barraVida == null)
            barraVida = pe_barraVida;
    }
	public float CalculateDamage(float Dmg, Utils.Element Element, float EAffinity)
	{
		float WeakAffinity = 0f; //%Afinidad elemento debil al pasado como argumento
		float StrongAffinity = 0f; //%Afinidad elemento resistente
		float DmgRecibido = 0f;
		float MaxElementalDmg = 0f;
		float FinalElementalDmg = 0f;

		//Calculo daño elemental.
		switch (Element) 
		{
		case Utils.Element.AGUA:
			WeakAffinity = this.AFuego/100;
			StrongAffinity = this.ARayo/100;
			break;
		case Utils.Element.FUEGO:
			WeakAffinity = this.ARayo/100;
			StrongAffinity = this.AAgua/100;
			break;
		case Utils.Element.RAYO:
			WeakAffinity = this.AAgua/100;
			StrongAffinity = this.AFuego/100;
			break;
		case Utils.Element.LUZ:
			WeakAffinity = this.AOscuridad/100;
			break;
		case Utils.Element.OSCURIDAD:
			WeakAffinity = this.ALuz/100;
			break;
		}
		
		MaxElementalDmg = Dmg * EAffinity;
		FinalElementalDmg = (MaxElementalDmg * WeakAffinity) - (MaxElementalDmg * StrongAffinity);
		Dmg = Dmg + FinalElementalDmg;
		
		
		//Calculamos el daño que inflingen
		DmgRecibido = Dmg - (this.Armadura / (this.Nivel * 20f) / 100f);
		//Debug.Log (Dmg + "* (" + this.Armadura + "/  (" + this.Nivel + "* 20f) / 100f)");
		//Debug.Log ("Daño = " + DmgRecibido);
		
		return DmgRecibido;
	}

	public bool ApplyDamageGenerico(float DmgRecibido){

		
		this.PuntosSalud = this.PuntosSalud - (int)DmgRecibido;
		
		
		//GameObject canvas = transform.FindChild ("EnemyCanvas").gameObject;
		GameObject canvas = GameObject.Find("Canvas");
		//Vector3 pos = canvas.transform.FindChild("BarraVidaDefecto").transform.position;
		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
		GameObject combatText = Instantiate (sct) as GameObject;
		combatText.transform.SetParent (canvas.transform, false);
		combatText.transform.position = pos;
		combatText.transform.GetChild(0).GetComponent<Text> ().text = Mathf.Abs((int)DmgRecibido) + "";
		
		//combatText.transform.GetChild(0).GetComponent<Text> ().color = tmp;
		
		if(this.PuntosSalud <= 0f)
		{
			return true;
		}
		else
		{
			//networkView.RPC("ApplyDamageOnOthers", RPCMode.Others, (int)DmgRecibido);
		}
		return false;
	}

	public bool ApplyDamage(float Dmg, Utils.Element Element, float EAffinity)
	{
		float DmgRecibido = CalculateDamage(Dmg, Element, EAffinity);
		//barraVida.UpdateUIEnemigo ();
        barraVida.UpdateUIEnemigo(PuntosSalud / maxHealth);
		return ApplyDamageGenerico (DmgRecibido);
	}

	public bool ApplyDamage(GameObject lanzador, float Dmg, Utils.Element Element, float EAffinity)
	{
		return ApplyDamage (lanzador, Dmg, Element, EAffinity, 1);
	}

	public bool ApplyDamage(GameObject lanzador, float Dmg, Utils.Element Element, float EAffinity, float mod)
	{
		float DmgRecibido = CalculateDamage(Dmg, Element, EAffinity);
		bool applyDam = ApplyDamageGenerico (DmgRecibido);
        ActualizarAmenaza(lanzador.name, DmgRecibido, mod);
		return applyDam;
	}

	public void ResetVida() {
		PuntosSalud = maxHealth;
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		if(this.PuntosSalud < this.maxHealth)
		{
			GetComponent<NetworkView>().RPC("UpdateHealth", player, this.PuntosSalud);
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// Sending
		if (stream.isWriting) 
		{
			if(Network.isServer)
			{
				float healthC = this.PuntosSalud;
				Vector3 position = new Vector3(0,0,0);
				Quaternion rotation = Quaternion.identity;

				position = this.gameObject.transform.position;
				if(this.rotationTransform != null)
					rotation = this.rotationTransform.rotation;
				
				stream.Serialize(ref healthC);
				stream.Serialize(ref position);
				stream.Serialize(ref rotation);
				//barraVida.UpdateUIEnemigo ();
                barraVida.UpdateUIEnemigo(PuntosSalud / maxHealth);
			}
		}
		else 
		{
			// Receiving
			float healthZ = 0;
			Vector3 position = new Vector3(0,0,0);
			Quaternion rotation = Quaternion.identity;
			
			stream.Serialize(ref healthZ);
			stream.Serialize(ref position);
			stream.Serialize(ref rotation);
			
			this.PuntosSalud = healthZ;
            if (barraVida != null)
                barraVida.UpdateUIEnemigo(PuntosSalud / maxHealth);
			    //barraVida.UpdateUIEnemigo ();
			this.gameObject.transform.position = position;
			if(this.rotationTransform != null)
				this.rotationTransform.rotation = rotation;
		}
	}

	[RPC]
	void UpdateHealth(float health)
	{
		this.PuntosSalud = health;
	}

	[RPC]
	void ApplyBuff (int amount) {
		//gameObject.AddComponent<BuffDamageRetardado>();
		//gameObject.GetComponent<BuffDamageRetardado>().Init(amount, 0f);
	}

    public void ActualizarAmenaza(string player, float dmg, float mod)
    {
        GetComponent<NetworkView>().RPC("UpdateAmenaza", RPCMode.All, player, dmg, mod);
    }

    [RPC]
    void UpdateAmenaza(string player, float dmg, float mod)
    {
        if (sisAmenaza != null)
            sisAmenaza.ActualizarAmenaza(player,dmg,mod);
        //barraVida.UpdateUIEnemigo();
        barraVida.UpdateUIEnemigo(PuntosSalud / maxHealth);
    }

    public GameObject GetCanvasEnemigo()
    {
        return canvasEnemigo;
    }

    public void EstablecerCanvas()
    {
        if (barraVida2D)
        {
            enemyCanvas = GameObject.FindGameObjectWithTag("EnemyCanvas");
            canvasAncla = transform.FindChild("AnclaCanvas").transform;

            Vector3 pos = Camera.main.WorldToScreenPoint(canvasAncla.position);
            GameObject w_canvas2D = Instantiate(prefabCanvasEnemigo) as GameObject;

            w_canvas2D.name = "EnemyCanvas_" + this.name;
            w_canvas2D.transform.SetParent(enemyCanvas.transform, false);
            w_canvas2D.transform.position = pos;

            Text w_text2D = w_canvas2D.transform.GetComponentInChildren<Text>();
            w_text2D.text = nombreEnemigo + " Lvl " + Nivel;
            canvasEnemigo = w_canvas2D;

            barraVida = canvasEnemigo.GetComponent<ENBarraVida>();
            barraVida.SetAnclaCanvas(canvasAncla);
            barraVida.SetAmenaza(sisAmenaza);
        }
        else
        {
            barraVida = transform.GetComponentInChildren<ENBarraVida>();
        }
    }
}