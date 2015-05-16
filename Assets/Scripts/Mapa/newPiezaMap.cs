using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class newPiezaMap {
	public bool top;
	public bool down;
	public bool left;
	public bool right;

	public newPiezaMap (bool t = false, bool d = true, bool l = true, bool r = false) {
		this.top = t;
		this.down = d;
		this.left = l;
		this.right = r;
	}

	public string Show() {
		return "Top: " + top + " Down: " + down + " Left: " + left + " Right: " + right;
	}
}
