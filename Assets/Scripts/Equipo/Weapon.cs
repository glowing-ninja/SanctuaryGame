using System.Collections;
using UnityEngine;

public class Weapon : Item {

	public int type;
	public Habilidad skill;

	public Weapon () {
		name = "Libro del iniciado";
		skill = new DBBolaMagica ();
		damage = 5;
		this.type = 0;
		this.subType = 1;
		setWeaponIcon ();
	}

	public Weapon (int st) {
		name = "Libro del iniciado";
		skill = new DBBolaMagica ();
		damage = 5;	
		this.type = 0;
		this.subType = st;
		setWeaponIcon ();
	}

	public Weapon(int st, string name, int rarity, int level, int damage, int armor, int str, int sp, int dex, int heal, int sta, int st1, int st2, int element, int affinity) {
		this.type = 0;
		this.subType = st;
		setWeaponIcon ();

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
		gold = this.level * 10;
	}

	public void setWeaponIcon() {
		switch (this.subType) {
		case (int)Utils.WeaponType.ESPADA:
			icon = Resources.Load<Sprite>("Sprites/sword-icon");
			skill = new FBMachaque ();
			break;
		case (int)Utils.WeaponType.LIBRO:
			icon = Resources.Load<Sprite>("Sprites/book-icon");	
			skill = new DBBolaMagica ();
			break;
		case (int)Utils.WeaponType.ARCO:
			icon = Resources.Load<Sprite>("Sprites/bow-icon");
			skill = new DBDisparo ();
			break;
		case (int)Utils.WeaponType.BASTON:
			icon = Resources.Load<Sprite>("Sprites/staff-icon");
			skill = new HBPotencialDivino ();
			break;
		case (int)Utils.WeaponType.ESCUDO:
			icon = Resources.Load<Sprite>("Sprites/shield-icon");
			skill = new STBGolpeConEscudo ();
			break;
		}
	}
	
	public int getWeapon() {
		return subType;
	}

	public int getDamage() {
		return damage;
	}
	
	public override string toString() {
		return  "Nombre: " + name + 
				"\n Nivel: " + level +
				"\n Rareza: " + rarity +
				"\n Daño: " + damage +
				"\n Fuerza: " + stats[0] +
				"\n Hechizos: " + stats[1] + 
				"\n Destreza: " + stats[2] + 
				"\n Sanacion: " + stats[3] + 
				"\n Entereza: " + stats[4] + 
				"\n Afinidad: " + isAffinity + 
				"\n Afinidad: " + affinity;
	}
}
