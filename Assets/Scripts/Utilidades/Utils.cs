using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour {

	public enum Element {
		FUEGO,
		AGUA,
		RAYO,
		LUZ,
		OSCURIDAD,
		NONE
	};

	public enum Stat {
		FUERZA,
		MAGIA,
		DESTREZA,
		CURA,
		AGUANTE,
		NONE
	};

	public enum WeaponType {
		ESPADA,
		LIBRO,
		ARCO,
		BASTON,
		ESCUDO
	};

	public enum ItemType {
		CHEST,
		LEGS,
		BOOTS,
		WEAPON
	};

	public enum DungeonType
	{
		NONE,
		DUNGEON1,
		DUNGEON2,
		DUNGEON3
	};

	public enum ClickSystem {
		EQUIP,
		SELL,
		TRADE,
		USESKILL,
		SELECTSKILL
	}

	public enum GameMode {
		PVE,
		PVP
	}

	public static ArrayList aliados;
	public static ArrayList enemigos;

	public static string objectPlayerName;
	public static string playerName;

	public static GameObject player;

	public const int mapOffset = 250;

	public static ClickSystem inventoryClick = ClickSystem.EQUIP;
	public static ClickSystem skillWindowClick = ClickSystem.USESKILL;
	public static GameMode modo = GameMode.PVE;
	public static int pos2Skill = -1;

	public class PlayerData {
		public string name;
		public int level;

		public PlayerData(string n, int lv) {
			this.name = n;
			this.level = lv;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
