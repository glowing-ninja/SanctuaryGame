using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowError : MonoBehaviour {

	static GameObject inst;

	public static void Show(string error) {
		GameObject errorWindow = Resources.Load ("Error") as GameObject;
		ShowError.inst = Instantiate (errorWindow) as GameObject;
		ShowError.inst.transform.SetParent (GameObject.Find ("Canvas").transform, false);
		inst.transform.FindChild("Text").GetComponent<Text> ().text = error;
	}

	public void Close () {
		GameObject.Find ("WantTutorial").GetComponent<Tutorial> ().setWaitFalse ();
		Destroy (inst);
	}
}
