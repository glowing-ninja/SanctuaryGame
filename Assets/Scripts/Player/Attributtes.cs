﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attributtes : MonoBehaviour {

	private GameObject spawnPoint;
	public const int VIDA_POR_STA = 5;

	public GameObject sct;
	public GameObject scGold;
	public GameObject scExp;
	public Color healColor;

	public int level;
	public int expTotal;
	public int expActual;

		
	public int damage;
	public int bonusDamage;
	public int armor;
	public int bonusArmor;
	public int[] stats;
	public int[] bonusStats;
	public int[] masterys;
	public int[] masteryExp;
	public int[] masteryExpTotal;
	private int[,] masteryMultiTable;

	public Transform inventory;
	public Transform equipamiento;
	public Transform canvas;

	public int health;
	public int MaxHealth;

	private Color32 piel;
	private int equipedWeapon;

	public Transform handWeapon;
	public Transform offHandWeapon;

	void Awake()
	{
		if(!GetComponent<NetworkView>().isMine)
		{
			//enabled = false;
		}
	}

	[RPC]
	void ShareColor(int red, int green, int blue)
	{
		Color32 c = new Color32();
		c.r = (byte)red;
		c.g = (byte)green;
		c.b = (byte)blue;
		
		piel = c;
		gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].color = c;
	}

	// Use this for initialization
	void Start () {

		this.spawnPoint = GameObject.Find("FirstSpawnPoint");

		bool cargarPj = false;
		string playerName = Utils.playerName;//PlayerPrefs.GetString ("playerName");

		stats = new int[5]; 
		bonusStats = new int[5];
		masterys = new int[5]; 
		masteryExp = new int[5]; 
		masteryExpTotal = new int[5];

		SQLite db = GameObject.Find ("BD").GetComponent<SQLite> ();
		if (db.QueryString ("name", playerName) != "") {
			cargarPj = true;
		}

		if (cargarPj) {

			level = db.QueryInt ("level", 	playerName);
			expActual = db.QueryInt ("xp", 	playerName);
			
			armor = 30 + 5 * (level - 1);
			damage = 30 + 5 * (level - 1);

			stats[0] = 15 + 4 * (level - 1);
			stats[1] = 15 + 4 * (level - 1);
			stats[2] =  0 + 3 * (level - 1);
			stats[3] = 10 + 4 * (level - 1);
			stats[4] = 25 + 6 * (level - 1);

			masterys[0] = db.QueryInt ("mstr", 	playerName);
			masterys[1] = db.QueryInt ("msp",	playerName);
			masterys[2] = db.QueryInt ("mdex", 	playerName);
			masterys[3] = db.QueryInt ("mheal", playerName);
			masterys[4] = db.QueryInt ("msta", 	playerName);

			masteryExp[0] = db.QueryInt("mStrExp", playerName);
			masteryExp[1] = db.QueryInt("mSpExp", playerName);
			masteryExp[2] = db.QueryInt("mDexExp", playerName);
			masteryExp[3] = db.QueryInt("mHealExp", playerName);
			masteryExp[4] = db.QueryInt("mStaExp", playerName);

			Color32 c = new Color32();
			c.r = (byte)db.QueryInt("redColor", playerName);
			c.g = (byte)db.QueryInt("greenColor", playerName);
			c.b = (byte)db.QueryInt("blueColor", playerName);

			piel = c;
			Utils.player.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].color = c;

			if(GetComponent<NetworkView>().isMine)
				gameObject.GetComponent<NetworkView>().RPC("ShareColor", RPCMode.OthersBuffered, db.QueryInt("redColor", playerName), db.QueryInt("greenColor", playerName),
			                                           db.QueryInt("blueColor", playerName));

		} else {
			level = 1;
			expActual = 0;
			
			armor = 30;
			damage = 30;

			stats [(int)Utils.Stat.FUERZA] = 15;
			stats [(int)Utils.Stat.MAGIA] = 15;
			stats [(int)Utils.Stat.DESTREZA] = 0;
			stats [(int)Utils.Stat.CURA] = 10;
			stats [(int)Utils.Stat.AGUANTE] = 25;
			
			masterys [(int)Utils.WeaponType.ESPADA] = 0;
			masterys [(int)Utils.WeaponType.LIBRO]   = 0;
			masterys [(int)Utils.WeaponType.ARCO]    = 0;
			masterys [(int)Utils.WeaponType.BASTON]  = 0;
			masterys [(int)Utils.WeaponType.ESCUDO] = 0;
		}
		
		bonusArmor = 0;
		bonusDamage = 0;
		
		masteryExpTotal[0] = masterys[0] * 20 + 20; // - exp almacenada ya
		masteryExpTotal[1] = masterys[1] * 20 + 20; // - exp almacenada ya
		masteryExpTotal[2] = masterys[2] * 20 + 20; // - exp almacenada ya
		masteryExpTotal[3] = masterys[3] * 20 + 20; // - exp almacenada ya
		masteryExpTotal[4] = masterys[4] * 20 + 20; // - exp almacenada ya

		GameObject.Find ("lb_Lv").GetComponent<Text> ().text = "Lv: " + level;
		expTotal = getNextExp ();

		masteryMultiTable = new int[5,5];
		for (int i = 0; i < 5; i++)
			for (int j = 0; j < 5; j++)
				masteryMultiTable[i, j] = 0;

		masteryMultiTable [(int)Utils.WeaponType.ESPADA, (int)Utils.Stat.FUERZA] = 1;
		masteryMultiTable [(int)Utils.WeaponType.ESPADA, (int)Utils.Stat.AGUANTE] = 1;
		
		masteryMultiTable [(int)Utils.WeaponType.LIBRO, (int)Utils.Stat.MAGIA] = 1;
		masteryMultiTable [(int)Utils.WeaponType.LIBRO, (int)Utils.Stat.DESTREZA] = 1;

		masteryMultiTable [(int)Utils.WeaponType.ARCO, (int)Utils.Stat.DESTREZA] = 2;

		masteryMultiTable [(int)Utils.WeaponType.BASTON, (int)Utils.Stat.CURA] = 1;
		masteryMultiTable [(int)Utils.WeaponType.BASTON, (int)Utils.Stat.AGUANTE] = 1;

		masteryMultiTable [(int)Utils.WeaponType.ESCUDO, (int)Utils.Stat.DESTREZA] = 1;
		masteryMultiTable [(int)Utils.WeaponType.ESCUDO, (int)Utils.Stat.AGUANTE] = 1;


		inventory = GameObject.Find("InventoryPanel").transform;
		if (cargarPj) {
			inventory.GetComponent<generateSlots>().inventory = db.AddAllItems(playerName, "Inventory", 18);
			inventory.GetComponent<generateSlots>().Gold = db.QueryInt("gold", playerName);
		}

		equipamiento = GameObject.Find ("EquipPanel").transform;
		equipamiento.GetComponent<PlayerEquip> ().equipment = new Equip ();
		if (cargarPj) {
			Item[] equipoTEMP = db.AddAllItems(playerName, "Equip", 4);
			for (int i = 0; i < equipoTEMP.Length; i++) {
				if (equipoTEMP[i] != null) {
					PlayerEquip pe = equipamiento.GetComponent<PlayerEquip>();
					pe.Equipar(equipoTEMP[i]);
				}
			}
		}


		canvas = GameObject.Find("Canvas").transform;

		gameObject.GetComponent<HealthTransform>().enabled = true;
		if (level < 10)
			gameObject.GetComponent<ExperienceTransform>().enabled = true;
		StartCoroutine (HealPorSegundo ());
		InstantiateWeapon ();

		//if (Utils.modo == Utils.GameMode.PVE) {
		//	Utils.aliados.Add("Player");
		//	Utils.enemigos.Add("Enemy");
		//}
	}

	public void MasteryUp(Utils.Stat st) {
		masteryExp [(int)st] += 10;
		if (masteryExp[(int)st] == masteryExpTotal[(int)st]) {
			masterys[(int)st]++;
			masteryExpTotal[(int)st] = masterys[(int)st] * 20 + 20;
			masteryExp [(int)st] = 0;
		}
	}

	public bool Equipar(int i) {
		Item item = inventory.GetComponent<generateSlots>().inventory[i];
		if (item.level <= level) {
			bool equipado = equipamiento.GetComponent<PlayerEquip>().Equipar(i);

			if (equipamiento != null)
				equipamiento.GetComponent<PlayerEquip>().equipWindow.GetComponent<UpdateEquip>().GetItems();
			gameObject.GetComponent<HealthTransform> ().HandleHealth (false);
			InstantiateWeapon ();
			return  equipado;
		}
		return false;
	}

	public void InstantiateWeapon() {
		GameObject weapon;
		GameObject instantiated;
		Transform hand = this.handWeapon;//GameObject.Find ("HandWeapon").transform;
		if (hand.childCount > 0)
			Destroy (hand.GetChild (0).gameObject);
		Transform offHand = this.handWeapon;//GameObject.Find ("OffHandWeapon").transform;
		if (offHand.childCount > 0)
			Destroy (offHand.GetChild (0).gameObject);

		PlayerEquip pe = equipamiento.GetComponent<PlayerEquip>();
		int st = pe.equipment.weapon.subType;
		this.equipedWeapon = st;


		switch (st) {
		case (int)Utils.WeaponType.ESPADA:
			weapon = Resources.Load<GameObject>("Prefab/Armas/newEspada_2_mano");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			break;
		case (int)Utils.WeaponType.LIBRO:
			weapon = Resources.Load<GameObject>("Prefab/Weapons/Book");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			break;
		case (int)Utils.WeaponType.ARCO:
			weapon = Resources.Load<GameObject>("Prefab/Armas/newArco");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			break;
		case (int)Utils.WeaponType.BASTON:
			weapon = Resources.Load<GameObject>("Prefab/Armas/newStaff");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			break;
		case (int)Utils.WeaponType.ESCUDO:
			weapon = Resources.Load<GameObject>("Prefab/Armas/newEspada_1_mano");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);

			GameObject weaponOff = Resources.Load<GameObject>("Prefab/Armas/newEscudo");
			GameObject instantiatedOff = GameObject.Instantiate(weaponOff) as GameObject;
			instantiatedOff.transform.SetParent(offHand, false);
			break;
		}
		if(gameObject.GetComponent<NetworkView>().isMine)
			gameObject.GetComponent<NetworkView>().RPC("InstantiateWeaponOnOthers", RPCMode.Others, st);
	}

	[RPC]
	public void InstantiateWeaponOnOthers(int st)
	{GameObject weapon;
		GameObject instantiated;
		Transform hand = this.handWeapon;
		if (hand.childCount > 0)
			Destroy (hand.GetChild (0).gameObject);
		Transform offHand = this.offHandWeapon;
		if (offHand.childCount > 0)
			Destroy (offHand.GetChild (0).gameObject);
		
		this.equipedWeapon = st;

		switch (st) {
		case (int)Utils.WeaponType.ESPADA:
			weapon = Resources.Load<GameObject>("Prefab/Armas/newEspada_2_mano");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			break;
		case (int)Utils.WeaponType.LIBRO:
			weapon = Resources.Load<GameObject>("Prefab/Weapons/Book");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			break;
		case (int)Utils.WeaponType.ARCO:
			weapon = Resources.Load<GameObject>("Prefab/Armas/newArco");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			break;
		case (int)Utils.WeaponType.BASTON:
			weapon = Resources.Load<GameObject>("Prefab/Armas/newStaff");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			break;
		case (int)Utils.WeaponType.ESCUDO:
			weapon = Resources.Load<GameObject>("Prefab/Armas/newEspada_1_mano");
			instantiated = GameObject.Instantiate(weapon) as GameObject;
			instantiated.transform.SetParent(hand, false);
			
			GameObject weaponOff = Resources.Load<GameObject>("Prefab/Armas/newEscudo");
			GameObject instantiatedOff = GameObject.Instantiate(weaponOff) as GameObject;
			instantiatedOff.transform.SetParent(offHand, false);
			break;
		}
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		gameObject.GetComponent<NetworkView>().RPC("InstantiateWeaponOnOthers", player, this.equipedWeapon);
	}


	public int getNextExp() {
		int exp = 0;
		switch(level) {
		case 1:
			exp = 500;
			break;
		case 2:
			exp = 1000;
			break;
		case 3:
			exp = 2000;
			break;
		case 4:
			exp = 8000;
			break;
		case 5:
			exp = 16000;
			break;
		case 6:
			exp = 32000;
			break;
		case 7:
			exp = 128000;
			break;
		case 8:
			exp = 256000;
			break;
		case 9:
			exp = 512000;
			break;
		}
		return exp;
	}

	public void addExp(int exp) {
		int auxExp = exp;
		if (level < 10/*LEVEL_MAXIMO*/) {
			expActual += exp;
			if (expActual >= expTotal) {
				expActual = expActual - expTotal;
				levelUp();
			}

			while(expActual >= expTotal && level < 10)
			{
				expActual = expActual - expTotal;
				levelUp();
			}
			GameObject combatText = Instantiate (scExp) as GameObject;
			
			combatText.transform.SetParent(canvas.transform);
			combatText.transform.position = canvas.transform.position;
			combatText.GetComponent<Text> ().text = "Exp: " + exp;
			gameObject.GetComponent<ExperienceTransform> ().HandleExp ();
			Destroy(combatText, 1f);
		}
	}

	public void levelUp() {
		level++;
		GameObject.Find ("lb_Lv").GetComponent<Text> ().text = "Lv: " + level;
		if (level == 10) {
			gameObject.GetComponent<ExperienceTransform>().enabled = false;
			expActual = 0;
		}
		
		expTotal = getNextExp ();

		armor += 5;
		damage += 5;
		stats[(int)Utils.Stat.FUERZA]  +=  4;
		stats[(int)Utils.Stat.MAGIA]   +=  4;
		stats[(int)Utils.Stat.DESTREZA]  +=  3;
		stats[(int)Utils.Stat.CURA] +=  4;
		stats[(int)Utils.Stat.AGUANTE]  += 6;
		
		gameObject.GetComponent<HealthTransform> ().HandleHealth (true);
	}

	public int getTotalMastery(Utils.Stat st) {
		return masterys [(int)st] * masteryMultiTable [(int)equipamiento.GetComponent<PlayerEquip> ().equipment.getWeapon (), (int)st];
	}

	public int getTotalEquip(Utils.Stat st) {
		return equipamiento.GetComponent<PlayerEquip> ().equipment.getTotalStat (st);
	}

	public int getTotalStat(Utils.Stat st) {
		int stat  = stats [(int)st] +
			bonusStats[(int)st] +
			getTotalEquip(st)+
			getTotalMastery(st);
		return  stat;
	}

	public int getTotalDamage() {
		return damage + bonusDamage + equipamiento.GetComponent<PlayerEquip>().equipment.getTotalDamage();
	}

	
	public int getTotalArmor() {
		return armor + bonusArmor + equipamiento.GetComponent<PlayerEquip>().equipment.getTotalArmor();
	}

	public int getGold () {
		return inventory.GetComponent<generateSlots>().Gold;
	}

	public void addGold(int g) {
		inventory.GetComponent<generateSlots>().AddGold(g);

		GameObject combatText = Instantiate (scGold) as GameObject;
		
		combatText.transform.SetParent(canvas.transform);
		combatText.transform.position = canvas.transform.position;
		if (g > 0)
			combatText.transform.GetChild(0).GetComponent<Text> ().text = "+" + g;
		else if (g < 0)
			combatText.transform.GetChild(0).GetComponent<Text> ().text = "" + g;

		
		Destroy(combatText, 1f);
	}

	public void doDamage(int dmg)
	{
		Color tmp = Color.red;
		if (dmg >= 0) {

			health -= dmg;
			if (health < 0)
			{
				if(GetComponent<NetworkView>().isMine)
					StartCoroutine("Death");
			}
		}
		else if (dmg < 0) {
			tmp = healColor;
			health -= dmg;
			if (health > MaxHealth)
				health = MaxHealth;
		}
		GameObject combatText = Instantiate (sct) as GameObject;

		combatText.transform.SetParent(canvas.transform);
		combatText.transform.position = canvas.transform.position;
		combatText.transform.GetChild(0).GetComponent<Text> ().text = Mathf.Abs(dmg) + "";
		combatText.transform.GetChild(0).GetComponent<Text> ().color = tmp;

		gameObject.GetComponent<HealthTransform> ().HandleHealth (false);
	}

	public IEnumerator HealPorSegundo() {
		while (true) {
			yield return new WaitForSeconds (10f);
			int cantidad = (int)(MaxHealth * 0.02);
			doDamage (-cantidad);
		}
	}

	[RPC]
	void ApplyBuff(string nameBuff, int amount, float duration, int id) {
		//Buff b = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (gameObject, "Assets/Scripts/Player/Attributtes.cs (336,12)", nameBuff) as Buff;
		//b.Init (amount, duration, id);
		//Types.get
		Buff b = this.gameObject.AddComponent(System.Type.GetType (nameBuff)) as Buff;
		b.Init(amount, duration, id, this.gameObject);
	}

	[RPC]
	void RevertBuff(string nameBuff, int id) {
		Buff b = (Buff)gameObject.GetComponent (nameBuff);
		Buff[] bs = (Buff[])gameObject.GetComponents<Buff>();
		for (int i = 0; i < bs.Length; i++) {
			if (bs[i].id == id) {
				bs[i].Revert ();
				i = bs.Length + 1;
			}
		}
	}

	[RPC]
	public void Heal(int amount, int id) {
		ApplyBuff ("BuffHealthModifier", amount, 0, id);
		//Buff b = gameObject.AddComponent < BuffHealthModifier>();
		//b.Init (amount, 0,id);
	}

	[RPC]
	public void SendDoDamage(int dmg)
	{
		this.doDamage(dmg);
	}

	//Incremetamos el buff de armadura
	public void IncreaseArmorBuff(int vBuff)
	{
		this.bonusArmor += vBuff;
	}
	
	//Disminuye el buff de armadura
	public void DecreaseArmorbuff(int vBuff)
	{
		this.bonusArmor -= vBuff;
	}
	
	//Aumenta la destreza
	public void IncreaseDexterity(int dex)
	{
		this.bonusStats[(int)Utils.Stat.DESTREZA] += dex;
	}
	
	//Diminuimos la destreza
	public void  DecreaseDexterity(int dex)
	{
		this.bonusStats[(int)Utils.Stat.DESTREZA] -= dex;
	}

	IEnumerator Death()
	{
		Debug.Log("Experiencia antes: " + expActual);


		LevelManager levelManager = GameObject.Find ("GameManager").GetComponent<LevelManager> ();
		
		if(levelManager)
			levelManager.resetActual();

		Attributtes att = Utils.player.GetComponent<Attributtes> ();

		att.health = 0;
		att.expActual = att.expActual - (int)Mathf.Max(att.expActual * 0.1f, 1f);

		if(att.expActual < 0)
			att.expActual = 0;

		gameObject.GetComponent<ExperienceTransform> ().HandleExp ();
		this.gameObject.transform.position = this.spawnPoint.transform.position;//new Vector3(130f,11f,160f);
		TPController pController = Utils.player.GetComponent<TPController> ();
		Teclado pTeclado = Utils.player.GetComponent<Teclado> ();
		followMouse fMouse = Utils.player.GetComponent<followMouse> ();
	
		pController.enabled = false;
		fMouse.enabled = false;
		pTeclado.enabled = false;

		HandleUI handle = GameObject.Find ("Canvas").GetComponent<HandleUI>();
		GameObject respawnPanel = handle.getRespawnPanel ();
		Text rText = respawnPanel.transform.Find ("RespawnTime").GetComponent<Text>();
		respawnPanel.SetActive (true);

		for(int i = 0; i < 10; i++)
		{
			rText.text = (10 - i).ToString();
			yield return new WaitForSeconds(1f);
		}
		att.health = this.MaxHealth;
		gameObject.GetComponent<HealthTransform> ().HandleHealth (true);


		respawnPanel.SetActive (false);
		pController.enabled = true;
		fMouse.enabled = true;
		pTeclado.enabled = true;

		Debug.Log("Experiencia despuees: " + expActual);


	}
}
