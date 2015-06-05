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
		//bt1.SetActive (false);
		//bt2.SetActive (false);
		wait = true;
		while (wait) {
			yield return new WaitForSeconds(0.5f);
		}

		if (doTutorial) {
			transform.position = new Vector3(-1000f, -1000f);
			ShowError.Show ("! Bienvenidos al tutorial de este fantastico juego !");
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

			ShowError.Show ("En la parte de abajo de las estadisticas veras las maestrias con arma. Cada habilidad esta asociada a un tipo de arma. Cuantas mas habilidades uses mejores estadisticas obtendras");
			
			//wait = true;
			//while (wait) {
			//}
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			yield return new WaitForSeconds(5f);

			ShowError.Show ("Cierra las estadísticas haciendo click de nuevo sobre el icono y pasa a la segunda opción. Ahora es el turno del equipamiento");
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
				yield return new WaitForSeconds(2f);
			}
			
			ShowError.Show ("Si ya has hecho click, el círculo estará de color verde, eso significa que está listo para que elijas habilidad. Elije Divinidad o Torbellino. Si pasas el ratón por encima de la habilidad verás una descripción.");
			//wait = true;
			//while (wait) {
			//}
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			yield return new WaitForSeconds(4f);
			flecha5.SetActive (false);
			
			ShowError.Show ("Es hora de usar tu nueva habilidad. Encima del icono de tu habilidad podrás ver con que tecla usarla. Hay varios tipos de habilidades: sobre un objetivo, automáticas sobre el mapa.");
			wait = true;
			player.GetComponent<Teclado>().enabled = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}

			ShowError.Show ("Puedes probar las habilidades con el bloque de hielo de aqui arriba.");
			wait = true;
			player.GetComponent<Teclado>().enabled = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}
			
			ShowError.Show ("En el boton escape tienes el menu de jugador. Cuando salgas del juego (con el boton salir) se guardara automaticamente, pero nunca esta de mas guardar cuando consigas un objeto importante");
			wait = true;
			//flecha6.SetActive (true);
			//bt1.SetActive(true);
			//bt2.SetActive (true);
			while (wait) {
				yield return new WaitForSeconds(3f);
			}

			ShowError.Show ("Esta zona ya esta dominada por tus demonios. Alguno de ellos puede venderte objetos utiles.");
			wait = true;
			flecha6.SetActive (false);
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}

			ShowError.Show ("En el menu de compra deberas hacer click sobre el objeto que te interese para aumenta el nivel de rareza. ¡ Cuanto mas raro lo quieras, mas tendras que pagar !.");
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}

			ShowError.Show ("Por ultimo, si te sobra dinero puedes gastarlo en nuestras tragaperras del infierno. Los premios son realmente impresionantes.");
			wait = true;
			while (wait) {
				yield return new WaitForSeconds(0.5f);
			}

			ShowError.Show ("Es hora de que empieces a pelear. Busca el portal de fuego al noroeste, eso debería bastar para bajar a la mazmorra. ¡ Suerte !");
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
			//bt1.SetActive (true);
			//bt2.SetActive (true);
			Destroy(gameObject);
		}
	}
}
