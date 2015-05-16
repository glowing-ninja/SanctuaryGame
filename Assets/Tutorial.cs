using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public GameObject player;
	public bool wait = false;
	public bool doTutorial = false;

	public GameObject flecha1;
	public GameObject flecha2;
	public GameObject flecha3;
	public GameObject flecha4;
	
	public GameObject flecha5;
	public GameObject flecha6;

	public GameObject bt1;
	public GameObject bt2;


	public GameObject barraHabilidades;

	// Use this for initialization
	public void Start () {
		StartCoroutine (StartTutorial ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setWaitFalse() {
		wait = false;
	}

	public void setDoTutorial(bool toDo) {
		doTutorial = toDo;
	}

	public IEnumerator StartTutorial () {
		while (GameObject.FindGameObjectWithTag("Player") == null) {
			yield return new WaitForSeconds(0.5f);
		}
		player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<TPController>().enabled = false;
		player.GetComponent<followMouse>().enabled = false;
		player.GetComponent<Teclado>().enabled = false;
		barraHabilidades.SetActive(false);
		bt1.SetActive (false);
		bt2.SetActive (false);
		wait = true;
		while (wait) {
			yield return new WaitForSeconds(0.5f);
		}

		if (doTutorial) {
			transform.position = new Vector3(-1000f, -1000f);
			ShowError.Show ("! Bienvenidos al tutorial de este prototipo !");
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("Parece que tienes ganas de moverte por aquí. Cierra esta ventana y utiliza las teclas ASDW para moverte por el mapa.");
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			player.GetComponent<TPController> ().enabled = true;
			
			yield return new WaitForSeconds(5f);
			
			ShowError.Show ("Vaya, esos giros no son nada naturales, voy a habilitarte la rotación. Mueve el ratón por la pantalla.");
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			player.GetComponent<followMouse> ().enabled = true;
			
			yield return new WaitForSeconds(5f);
			
			ShowError.Show ("Bien. Ahora te enseñaré lo básico. Acerca el ratón al lado derecho para mostrar el menú. Es mejor que cierres estos mensajes después de realizar la acción.");
			flecha1.SetActive (true);
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("Con el menú abierto haz click sobre la primera opción para ver tus estadísticas.");
			
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			flecha1.SetActive (false);
			
			ShowError.Show ("Actualmente no es recomendable abrir más de un menú. Cierra las estadísticas haciendo click de nuevo sobre el icono y pasa a la segunda opción.");
			flecha2.SetActive (true);
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("Si pasas el ratón por encima de los objetos verás sus estadísticas. ¡ Pruébalo !");
			
			flecha2.SetActive (false);
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("La siguiente opción del menú es el inventario. Ahora está vacío, pero pronto podrás empezar a llenarlo.");
			
			flecha3.SetActive (true);
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("Por último están las habilidades. Aún no te he enseñado a usarlas.");
			flecha3.SetActive (false);
			flecha4.SetActive (true);
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			ShowError.Show ("Aquí tienes tu barra de habilidades. Cada arma te otorga una habilidad básica con un tiempo de reutilización pequeño para que siempre tengas algo que pulsar.");
			wait = true;
			barraHabilidades.SetActive(true);
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("Para asignar más habilidades abre la última opción del menú: Las habilidades disponibles.");
			flecha4.SetActive (true);
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			flecha4.SetActive (false);
			
			ShowError.Show ("Con el menú de habilidades abierto vamos a hacer click sobre uno de los huecos disponibles de tu barra de habilidades.");
			flecha5.SetActive (true);
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("Si ya has hecho click, el círculo estará de color verde, eso significa que está listo para que elijas habilidad. Elije Divinidad o Torbellino. Si pasas el ratón por encima de la habilidad verás una descripción.");
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			flecha5.SetActive (false);
			
			ShowError.Show ("Es hora de usar tu nueva habilidad. Encima del icono de tu habilidad podrás ver con que tecla usarla. Hay varios tipos de habilidades: sobre un objetivo, automáticas sobre el mapa. No te asustes si una habilidad no funciona a la primera.");
			wait = true;
			player.GetComponent<Teclado>().enabled = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("Para finalizar te enseñaré como vender objetos y guardar tu partida durante este prototipo. En la parte inferior derecha de la pantalla te voy a dar un par de opciones provisionales. El guardado no es automático, así que no te olvides de guardar antes de salir.");
			wait = true;
			flecha6.SetActive (true);
			bt1.SetActive(true);
			bt2.SetActive (true);
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			ShowError.Show ("Es hora de que empieces a pelear. Busca la estructura naranja y atraviesala, eso debería bastar para bajar a la mazmorra. ¡ Suerte !");
			wait = true;
			flecha6.SetActive (false);
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
		}
		else {
			player.GetComponent<TPController>().enabled = true;
			player.GetComponent<followMouse>().enabled = true;
			player.GetComponent<Teclado>().enabled = true;
			barraHabilidades.SetActive(true);
			bt1.SetActive (true);
			bt2.SetActive (true);
			Destroy(gameObject);
		}
	}
}
