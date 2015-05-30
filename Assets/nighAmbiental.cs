using UnityEngine;
using System.Collections;

public class nighAmbiental : MonoBehaviour {

	public Light sun;
	public AudioSource fx_thunder;

	// Use this for initialization
	void Start () {
		StartCoroutine("thunder");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator thunder() {
		while (true) {
			float time = Random.Range(4, 20);
			yield return new WaitForSeconds(time);
			sun.intensity += 3f;
			fx_thunder.Play();
			yield return new WaitForSeconds(0.2f);
			sun.intensity -= 3f;
		}
	}
}
