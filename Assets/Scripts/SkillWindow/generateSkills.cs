using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class generateSkills : MonoBehaviour {

	public string[,] skillIcon;
	public Habilidad[,] skillPrefab;
	public GameObject skillSlot;

	void OnEnable () {
		Utils.skillWindowClick = Utils.ClickSystem.SELECTSKILL;
	}

	void OnDisable () {
		Utils.skillWindowClick = Utils.ClickSystem.USESKILL;
	}

	// Use this for initialization
	void Awake () {
		skillIcon = new string[5, 4];
		skillIcon [0, 1] = "Sprites/SKILL_ICON/STR_ICON_TORBELLINO";

		skillIcon [2, 0] = "Sprites/SKILL_ICON/DEX_ICON_ATAQUERAPIDO";

		skillIcon [3, 0] = "Sprites/SKILL_ICON/HEAL_ICON_CASTIGO";
		skillIcon [3, 1] = "Sprites/SKILL_ICON/HEAL_ICON_AREASANADORA";
		skillIcon [3, 2] = "Sprites/SKILL_ICON/HEAL_ICON_AREAPOTENCIADORA";
		skillIcon [3, 3] = "Sprites/SKILL_ICON/HEAL_ICON_DIVINIDAD";

		skillIcon [4, 0] = "Sprites/SKILL_ICON/STA_ICON_DEFENSAABSOLUTA";

		skillPrefab = new Habilidad[5, 4];
		skillPrefab [0, 1] = new F2Torbellino();
		
		skillPrefab [2, 0] = new D1AtaqueRapido ();
		
		skillPrefab [3, 0] = new H1Castigo();
		skillPrefab [3, 1] = new H2AreaSanadora();
		skillPrefab [3, 2] = new H3AreaPotenciadora ();
		skillPrefab [3, 3] = new HODivinidad();
		
		skillPrefab [4, 0] = new ST1DefensaAbsoluta();

		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 4; j++) {
				if (skillIcon[i,j] != null) {
					Vector3 pos = new Vector3 (j * 50, i * -50);
					GameObject tmp = Instantiate (skillSlot, pos, Quaternion.identity) as GameObject;
					tmp.GetComponent<SelectSkill>().skill = skillPrefab[i,j];
					Sprite stmp = Resources.Load<Sprite>(skillIcon[i,j]);
					tmp.GetComponent<SelectSkill>().icon = stmp;
					tmp.transform.SetParent(transform, false);
				}
				else {
					Vector3 pos = new Vector3 (j * 50, i * -50);
					GameObject tmp = Instantiate (skillSlot, pos, Quaternion.identity) as GameObject;
					//tmp.GetComponent<SelectSkill>().skill = skillPrefab[i,j];
					Sprite stmp = Resources.Load<Sprite>("Sprites/no-item");
					tmp.GetComponent<SelectSkill>().icon = stmp;
					tmp.transform.SetParent(transform, false);
				}
			}
		}
	}
}
