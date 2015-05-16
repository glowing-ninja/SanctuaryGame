using UnityEngine;
using System.Collections;

public class IncubadoraHandler : MonoBehaviour {

	public int PV = 0;
	GameObject Dragon;
	public bool doHeal = false;
	// Use this for initialization
	void Start () {
		//Dragon = transform.parent.FindChild ("HuevoDragon").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (doHeal) {
			Curar(50);
			doHeal = false;
		}
	}

	public void Curar (int amount) {
		PV += amount;
		Quemar (amount);
	}

	public void Quemar(int amount) {
		//Dragon.GetComponent<MiniDragonHandler> ().doDamage(amount * 2);
	}
}
