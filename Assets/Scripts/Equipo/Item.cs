using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Item {

	public const int probArma = 20;
	public const float r1Multi = 1.0f;
	public const float r2Multi = 1.2f;
	public const float r3Multi = 1.5f;
	public const float r4Multi = 1.5f;
	public const float r5Multi = 1.8f;
	public const int probAfinidad = 30;

	public string name;

	public int rarity;
	public int probModifier;
	public int level;

	public int type;
	public int subType;

	public int damage;
	public int armor;

	public int[] stats;
	public int isStat1;
	public int isStat2;

	public Utils.Element isAffinity;
	public int affinity;

	public Sprite icon;

	public int gold;

	public Item() {
			
		rarity = 1;
		level = 1;
		damage = 0;
		armor = 0;

		stats = new int[5]; 
		
		stats [(int)Utils.Stat.FUERZA]  = 0;
		stats [(int)Utils.Stat.MAGIA]   = 0;
		stats [(int)Utils.Stat.DESTREZA]  = 0;
		stats [(int)Utils.Stat.CURA] = 0;
		stats [(int)Utils.Stat.AGUANTE]  = 0;

		isAffinity = Utils.Element.NONE;
		affinity = 0;
		icon = Resources.Load<Sprite>("Sprites/sword-icon");
		gold = 5;
	}

	public static Item ItemGenerator(int chest, int level) {
		Item i = null;
		int mod = 0;
		if (chest == 2) {
			mod = 10;
		} else if (chest == 3) {
			mod = 100;
		}

		int t = Random.Range(0, 100);
		string name = "";
		if (t < probArma) {			// Si es un arma calculamos el subtip
			int subType = Random.Range(0,5);
			switch(subType) {
				case 0: name = "Espada bastarda de"; break;
				case 1: name = "Tomo de poder de"; break;
				case 2: name = "Arco de"; break;
				case 3: name = "Baston de"; break;
				case 4: name = "Espada y escudo de"; break;
			}
			i = new Weapon(subType);		
			i.type = 0;
			i.subType = subType;
		} else {
			int subType = Random.Range(0,3);
			switch(subType) {
			case 0: name = "Peto de"; i = new Chest();break;
			case 1: name = "Perneras de"; i = new Legs(); break;
			case 2: name = "Botas de"; i = new Boots(); break;
			}
			i.type = 1;
			i.subType = subType;
		}
		i.level = level;
		
		// Calculamos la rareza
		int rarity = 1;
		float multiR = 1f;
		int r = Random.Range(0,100);
		/* Si el cofre es de oro, rarity 5 tiene un 100% de prob. Si es de plata, rarity5 tiene un 12%, rarity4 un 28% y rarity3 un 70%*/
		if (r < 2 + mod) {       rarity = 5; multiR = r5Multi;}	//  2%
		else if (r < 10 + mod * 3) { rarity = 4; multiR = r4Multi;}	//  8%
		else if (r < 25 + mod * 8) { rarity = 3; multiR = r3Multi;}	// 15%
		else /*if (r < 50 + probModifier)*/ { rarity = 2; multiR = r2Multi;}	// 75%
		//else 			 { rarity = 1; multiR = r1Multi;}// 50%				Ya no salen
		i.rarity = rarity;
		
		// Calcular arma y armadura
		int damage = 0, armor = 0;
		if (i.type == 0) {
			damage  = (int)(Random.Range(Mathf.Max(level, 5), level * 2) * multiR);
			i.damage = damage;
		}
		else if (i.type == 1) {
			armor = (int)(Random.Range(Mathf.Max(level, 5), level * 2) * multiR);
			i.armor = armor;
		}

		
		// Calcular las estadisticas
		int stat1, stat2;
		//if (rarity == 1)
		//	name += " mendigo";
		
		if (rarity >= 2) {
			i.isStat1 = Random.Range(0,5);
			stat1 = (int)(Random.Range(level, (int)(level * 1.5)) * multiR);
			switch(i.isStat1) {
			case 0: name += " guerrero"; break;
			case 1: name += " mago"; break;
			case 2: name += " ladron"; break;
			case 3: name += " monje"; break;
			case 4: name += " gigante"; break;
			}
			i.stats[i.isStat1] = stat1;
		}
		
		if (rarity >= 4) {
			do {
				i.isStat2 = Random.Range(0,5);
				stat2 = (int)(Random.Range(level, (int)(level * 1.5)) * multiR);
				i.stats[i.isStat2] = stat2;
			} while (i.isStat2 == i.isStat1);
			switch(i.isStat2) {
				case 0: name += " bruto"; break;
				case 1: name += " inteligente"; break;
				case 2: name += " veloz"; break;
				case 3: name += " sanador"; break;
				case 4: name += " protector"; break;
			}
		}

		i.name = name;

		// Calcular si tiene afinidad
		if (Random.Range(0, 100) < probAfinidad) {
			i.isAffinity = i.RandomElement();
			i.affinity = Random.Range(1, 16);
		}

		i.gold = i.level * 5;

		return i;
	}

	public int getStat(Utils.Stat st) {
		return stats[(int)st];
	}

	public virtual string toString() {
		return "";
	}

	public Utils.Element RandomElement() {
		int n = Random.Range(0, 100);
		if ( n < 20) return Utils.Element.FUEGO;
		if ( n < 40) return Utils.Element.AGUA;
		if ( n < 60) return Utils.Element.RAYO;
		if ( n < 80) return Utils.Element.LUZ;
		return Utils.Element.OSCURIDAD;
	}

}
