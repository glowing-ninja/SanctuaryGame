using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class EnemyDatabase {

	public int Size;
	public EnemyDataClass[] EnemyList;

	public EnemyDatabase(int size)
	{
		this.EnemyList = new EnemyDataClass[size];
		this.Size = size;
	}

	public void AddEnemy(int index, Vector3 newPosition, NetworkViewID nViewID)
	{
		if(index < this.Size)
		{
			this.EnemyList[index] = new EnemyDataClass(newPosition, nViewID);
		}
	}

	public Vector3[] getPosition()
	{
		Vector3[] positions = new Vector3[this.Size];
		
		for(int i = 0; i < this.Size; i++)
		{
			positions[i] = EnemyList[i].getPosition();
		}
		
		return positions;
	}

	public Vector3 getPositionAt(int index)
	{
		if(index < this.Size)
			return EnemyList[index].getPosition();
		else
			return new Vector3(0,0,0);
	}

}
