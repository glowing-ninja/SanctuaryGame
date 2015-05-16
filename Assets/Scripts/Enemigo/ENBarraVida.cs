using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ENBarraVida : MonoBehaviour {
	//rivate ENEstadisticas stats;
	private Amenaza amenaza;
	private Image barraVida;
	private Image imgAmenaza;
	private GameObject jugador;
    private GameObject OBAmenaza;
	private float vidaOriginal;

    public RectTransform rectVidaCanvas;
    public RectTransform recAmenazaCanvas;
    public Transform anclaCanvas;
    public Vector2 camera;
    public Vector3 posAnclaCanvas;
	
	void Start () {
        // Barra de vida
		//stats = GetComponentInParent<ENEstadisticas> ();
		barraVida = transform.FindChild ("BarraVidaDefecto").transform.FindChild("BarraVida").GetComponent<Image> ();
        // Imagen de amenaza
        //amenaza = GetComponentInParent<Amenaza>();
        OBAmenaza = transform.Find("ImgAmenazaDefecto").gameObject;
        OBAmenaza.SetActive(false);
        imgAmenaza = OBAmenaza.GetComponent<Image>();
		jugador = GameObject.Find (Utils.objectPlayerName);
        GetComponent<Canvas>().overrideSorting = true;
	}

    void Update()
    {
        if (anclaCanvas != null)
        {
            Vector3 posCamera = Camera.main.WorldToScreenPoint(anclaCanvas.position);
            transform.position = posCamera;
        }
    }

    public void SetAnclaCanvas(Transform pe_anclaCanvas)
    {
        anclaCanvas = pe_anclaCanvas;
    }

    public void SetAmenaza(Amenaza pe_amenaza)
    {
        amenaza = pe_amenaza;
    }

    void ActualizarPosicionBarra()
    {

    }
	public void UpdateUIEnemigo() {
		UpdateBarraVida ();
        UpdateImgAmenaza();
	}
    public void UpdateUIEnemigo(float pe_fillAmount)
    {
        UpdateBarraVida(pe_fillAmount);
        UpdateImgAmenaza();
    }

	private void UpdateBarraVida() {
		//barraVida.fillAmount = stats.PuntosSalud/stats.maxHealth;
	}

    private void UpdateBarraVida(float pe_fillAmount)
    {
        barraVida.fillAmount = pe_fillAmount;
    }

	public void UpdateImgAmenaza(){
		float w_amenaza;
		if (jugador == null)
			jugador = GameObject.Find (Utils.objectPlayerName);
		
		if (jugador != null) {
			w_amenaza = amenaza.GetAmenazaPorJugador(jugador);
			if (w_amenaza != -1){
                if (!OBAmenaza.activeSelf)
                    OBAmenaza.SetActive(true);
                imgAmenaza.fillAmount = w_amenaza;
                if (w_amenaza >= 0f && w_amenaza <= 0.5f)
					imgAmenaza.color = new Color32(61, 62, 184, 255);
				else if (w_amenaza <= 0.99f)
                    imgAmenaza.color = new Color32(184, 173, 32, 255);
				else
                    imgAmenaza.color = new Color32(178, 39, 39, 255);
			}
		}
	}
}