using System.Collections;
using UnityEngine;

public class Equip {

	public Chest chest = null;
	public Legs legs = null;
	public Boots boots = null;
	
	public Weapon weapon = null;
	
	public Equip() {
		Equipar (new Weapon(2));
		Equipar (new Chest ());
		Equipar (new Legs ());
		Equipar (new Boots ());

	}

	public bool Equipar (Item i) {
		
		if (i.GetType() == typeof(Weapon)) {
			Teclado skill = Utils.player.GetComponent<Teclado>();
			weapon = i as Weapon;
			skill.SetSkill(weapon.skill, 0);
		}
		
		if (i.GetType() == typeof(Chest)) {
			chest = i as Chest;
		}
		
		if (i.GetType() == typeof(Legs)) {
			legs = i as Legs;
		}
		
		if (i.GetType() == typeof(Boots)) {
			boots = i as Boots;
		}
		return true;
	}

	
	public int getTotalDamage() {
		if (weapon != null)
			return weapon.getDamage();
		return 0;
	}

	public int getTotalArmor() {
		int armadura = 0;
		if (chest != null)
			armadura += chest.getArmor();
		if (legs != null)
			armadura += legs.getArmor();
		if (boots != null)
			armadura += boots.getArmor();
		return armadura;
	}

	public int getWeapon() {
		return weapon.getWeapon ();
	}

	/*public bool Equipar (int pos) {
		bool equipado = true;
		Item ant = null;
		generateSlots gs =  GameObject.Find("InventoryPanel").GetComponent<generateSlots>();
		Item i = gs.inventory [pos];

		if (i.GetType() == typeof(Weapon)) {
			Teclado skill = GameObject.Find(PlayerPrefs.GetString("playerName")).GetComponent<Teclado>();
			if (weapon == null || skill.skillScripts[0].cooldownTimeRemaining() <= 0f) {
				ant = weapon;
				weapon = i as Weapon;
				skill.SetSkill(weapon.skillName, 0);
			}
			equipado = false;
			// MOSTRAR MENSAJE SI NO SE PUEDE EQUIPAR 
		}

		if (i.GetType() == typeof(Chest)) {
			ant = chest;
			chest = i as Chest;
		}

		if (i.GetType() == typeof(Legs)) {
			ant = legs;
			legs = i as Legs;
		}

		if (i.GetType() == typeof(Boots)) {
			ant = boots;
			boots = i as Boots;
		}
		if (equipado)
			gs.inventory[pos] = null;
		if (ant != null)
			gs.Add (ant);
		
		return equipado;
	}

	public bool Equipar (Item i) {
		
		if (i.GetType() == typeof(Weapon)) {
			Teclado skill = GameObject.Find(PlayerPrefs.GetString("playerName")).GetComponent<Teclado>();
			weapon = i as Weapon;
			skill.SetSkill(weapon.skillName, 0);
		}
		
		if (i.GetType() == typeof(Chest)) {
			chest = i as Chest;
		}
		
		if (i.GetType() == typeof(Legs)) {
			legs = i as Legs;
		}
		
		if (i.GetType() == typeof(Boots)) {
			boots = i as Boots;
		}
		return true;
	}*/

	public int getTotalStat(Utils.Stat st) {
		int total = 0;
		if (chest != null)
			total += chest.getStat(st);
		if (legs != null)
			total += legs.getStat(st);
		if (boots != null)
			total += boots.getStat(st);
		if (weapon != null)
			total += weapon.getStat(st);

		return total;
	}

}
