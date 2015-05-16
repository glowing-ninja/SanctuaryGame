using System.Collections;
using UnityEngine;

public class Chest : Item {

	public Chest () {
		name = "Pechera del iniciado";
		armor = 5;
		this.type = 1;
		this.subType = 0;
		icon = Resources.Load<Sprite>("Sprites/chest-icon");
	}

	public Chest(string name, int rarity, int level, int damage, int armor, int str, int sp, int dex, int heal, int sta, int st1, int st2, int element, int affinity) {
		this.type = 1;
		this.subType = 0;

		this.name = name;
		this.rarity = rarity;
		this.level = level;
		
		this.damage = damage;
		this.armor = armor;

		this.isStat1 = st1;
		this.isStat2 = st2;

		stats [0] = str;
		stats [1] = sp;
		stats [2] = dex;
		stats [3] = heal;
		stats [4] = sta;

		this.isAffinity = (Utils.Element)element;
		this.affinity = affinity;
		
		icon = Resources.Load<Sprite>("Sprites/chest-icon");
		gold = this.level * 5;
	}

	
	public int getArmor() {
		return armor;
	}

	public override string toString() {
		return  "Nombre: " + name + 
			"\n Nivel: " + level +
				"\n Rareza: " + rarity +
				"\n Armadura: " + armor +
				"\n Fuerza: " + stats[0] +
				"\n Hechizos: " + stats[1] + 
				"\n Destreza: " + stats[2] + 
				"\n Sanacion: " + stats[3] + 
				"\n Entereza: " + stats[4] + 
				"\n Afinidad: " + isAffinity + 
				"\n Afinidad: " + affinity;
	}
}
