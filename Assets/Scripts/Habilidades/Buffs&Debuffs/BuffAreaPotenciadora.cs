using UnityEngine;
using System.Collections;

public class BuffAreaPotenciadora : Buff {

	public int[] mejoraStats;
	public GameObject potenciador;
	public GameObject ps1;

	public override void Init(float amount, float secondsToWait, int id, GameObject owner) {
		this.id = id;
		this.amount = amount;
		this.secondsToWait = secondsToWait;
		this.owner = owner;
		MejorarStats ();
	}

	void Update() {
		this.secondsToWait -= Time.deltaTime;
		if (this.secondsToWait <= 0)
			DevolverStats ();
	}

	public void MejorarStats() {
		mejoraStats = new int[5];
		int[] stats = gameObject.GetComponent<Attributtes> ().stats;
		for (int i = 0; i < 5; i++) {
			mejoraStats [i] = (int)(stats [i] * amount);
			gameObject.GetComponent<Attributtes> ().bonusStats[i] += mejoraStats[i];
		}
		potenciador = Resources.Load ("Habilidad/Heal/Potenciador") as GameObject;
		Vector3 pos = new Vector3 (gameObject.transform.position.x + 0.5f, gameObject.transform.position.y + 0.01f, gameObject.transform.position.z);
		ps1 = Instantiate(potenciador, pos, potenciador.transform.rotation) as GameObject;
		ps1.transform.SetParent (gameObject.transform);

		
		gameObject.GetComponent<HealthTransform> ().HandleHealth (false);
	}

	public void DevolverStats() {
		int[] stats = gameObject.GetComponent<Attributtes> ().stats;
		for (int i = 0; i < 5; i++) {
			gameObject.GetComponent<Attributtes> ().bonusStats[i] -= mejoraStats[i];
		}
		ps1.GetComponent<ParticleSystem>().Stop();
		
		gameObject.GetComponent<HealthTransform> ().HandleHealth (false);
		Destroy (ps1, 1f);
		Destroy (this);
	}

	override public void Revert() {
		DevolverStats ();
	}

	override public System.Type Type() {
		return typeof(BuffAreaPotenciadora);
	}
}
