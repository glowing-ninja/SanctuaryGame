using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowCooldownTime : MonoBehaviour {

	public int i;
	public Image skillImage;
	// Use this for initialization
	void Start () {
		skillImage = transform.parent.Find("skill-icon").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	public IEnumerator CoolDownUpdate () {
		while (true) {
			gameObject.GetComponent<Text>().text = Utils.player.GetComponent<Teclado>().skillScripts[i].cooldownTimeRemaining().ToString("F1");
			skillImage.fillAmount = Utils.player.GetComponent<Teclado>().skillScripts[i].CooldownPercentage();
			yield return new WaitForSeconds(0.1f);
		}
	}
}
