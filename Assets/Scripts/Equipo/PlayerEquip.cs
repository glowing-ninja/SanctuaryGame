using UnityEngine;
using System.Collections;

public class PlayerEquip : MonoBehaviour {
	
	public Transform equipWindow;
	
	public Equip equipment;

	public bool Equipar (int pos) {
		bool equipado = true;
		Item ant = null;
		generateSlots gs =  GameObject.Find("InventoryPanel").GetComponent<generateSlots>();
		Item i = gs.inventory [pos];
		
		if (i.GetType() == typeof(Weapon)) {
			Teclado skill = Utils.player.GetComponent<Teclado>();
			if (equipment.weapon == null || skill.skillScripts[0].cooldownTimeRemaining() <= 0f) {
				ant = equipment.weapon;
				equipment.weapon = i as Weapon;
				skill.SetSkill(equipment.weapon.skill, 0);
			}
			else
				equipado = false;
		}
		
		if (i.GetType() == typeof(Chest)) {
			ant = equipment.chest;
			equipment.chest = i as Chest;
		}
		
		if (i.GetType() == typeof(Legs)) {
			ant = equipment.legs;
			equipment.legs = i as Legs;
		}
		
		if (i.GetType() == typeof(Boots)) {
			ant = equipment.boots;
			equipment.boots = i as Boots;
		}
		if (equipado) {
			gs.inventory [pos] = null;
		}
		if (ant != null)
			gs.Add (ant);
		
		return equipado;
	}
	
	public bool Equipar (Item i) {
		
		if (i.GetType() == typeof(Weapon)) {
			Teclado skill = Utils.player.GetComponent<Teclado>();
			equipment.weapon = i as Weapon;
			skill.SetSkill(equipment.weapon.skill, 0);
		}
		
		if (i.GetType() == typeof(Chest)) {
			equipment.chest = i as Chest;
		}
		
		if (i.GetType() == typeof(Legs)) {
			equipment.legs = i as Legs;
		}
		
		if (i.GetType() == typeof(Boots)) {
			equipment.boots = i as Boots;
		}
		return true;
	}
}
