using UnityEngine;
using System.Collections;

public class ShowBlackPanel : MonoBehaviour {

	public static void ShowBlackScreen() {
		ShowBlackPanel sbp = GameObject.Find("BlackPanel").GetComponent<ShowBlackPanel> ();
		sbp.StartCoroutine ("BlackScreen");
	}

	public IEnumerator BlackScreen () {
		CanvasGroup cg = GameObject.Find("BlackPanel").GetComponent<CanvasGroup> ();
		while (cg.alpha < 1) {
			cg.alpha += 0.25f;
			yield return new WaitForSeconds(0.1f);
		}
		if (cg.alpha > 1)
			cg.alpha = 1;
		yield return new WaitForSeconds (1f);
		while (cg.alpha > 0) {
			cg.alpha -= 0.25f;
			yield return new WaitForSeconds(0.1f);
		}
		if (cg.alpha < 0)
			cg.alpha = 0;
	}
}
