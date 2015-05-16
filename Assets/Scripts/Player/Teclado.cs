using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Teclado : MonoBehaviour {

	//private GameObject Skills;
	public GameObject[] habilidades;
	public Habilidad[] skillScripts;
	private const int SkillNumber = 6;
	private SkillThrower skillThrower;


	void Awake()
	{


		if(GetComponent<NetworkView>().isMine)
		{
			habilidades = new GameObject[SkillNumber];
			this.skillScripts = new Habilidad[SkillNumber];

			
			this.skillThrower = GetComponent<SkillThrower>();
	//		elemento = Resources.Load ("Bullet", typeof(GameObject)) as GameObject;
	//		habilidad = elemento.GetComponent ("Disparo") as Habilidad;
	//		habilidad.setPlayer (GameObject.Find ("Player"));
	//		habilidad.resetAttackTime ();
	//		//elemento = Resources.Load ("Bullet", typeof(GameObject)) as GameObject;

		}
		else
		{
			enabled = false;
		}

	}

	void Start()
	{
		/*GameObject selectedSkills = GameObject.Find ("SelectedSkillsPanel") as GameObject;
		for (int i = 1; i < selectedSkills.transform.childCount; i++) {
			if (selectedSkills.transform.GetChild(i).GetComponent<SelectSkill>() != null &&
			    selectedSkills.transform.GetChild(i).GetComponent<SelectSkill>().name != "") {
				SetSkill(selectedSkills.transform.GetChild(i).GetComponent<SelectSkill>().name, i);
			}
		}*/
	}

	public void SetSkill(Habilidad skill, int pos) {
		//this.habilidades[pos] = Resources.Load (skill, typeof(GameObject)) as GameObject;
		this.skillScripts[pos] = skill;
		this.skillScripts [pos].Init (gameObject, skillThrower);

		GameObject skWindow = GameObject.Find ("SelectedSkillsPanel").transform.GetChild (pos).gameObject;
		skWindow.GetComponent<SelectSkill> ().icon = this.skillScripts [pos].icon;
		skWindow.transform.Find("skill-icon").GetComponent<Image> ().sprite = this.skillScripts [pos].icon;
		skWindow.transform.Find("skill-icon").GetComponent<Image> ().color = Color.white;
		StartCoroutine (skWindow.transform.GetChild (1).GetComponent<ShowCooldownTime> ().CoolDownUpdate ());
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Fire1")) {
			this.skillScripts[0].useWithCooldown();
		}
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			if (this.skillScripts[1] != null)
				this.skillScripts[1].useWithCooldown();
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			if (this.skillScripts[2] != null)
				this.skillScripts[2].useWithCooldown();
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			if (this.skillScripts[3] != null)
				this.skillScripts[3].useWithCooldown();
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			if (this.skillScripts[4] != null)
				this.skillScripts[4].useWithCooldown();
		}

		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			if (this.skillScripts[5] != null)
				this.skillScripts[5].useWithCooldown();
		}
	}
}
