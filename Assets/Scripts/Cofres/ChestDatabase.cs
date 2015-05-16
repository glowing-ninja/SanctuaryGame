using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class ChestDatabase 
{

	private float[] positionX;
	private float[] positionY;
	private float[] positionZ;
	private int[] rarity;
	public int Size;

	public ChestDatabase(int size)
	{
		this.positionX = new float[size];
		this.positionY = new float[size];
		this.positionZ = new float[size];
		this.rarity = new int[size];
		this.Size = size;
	}

	public void AddChest(int index, int newRarity, Vector3 newPosition)
	{
		if(index < this.Size)
		{
			this.rarity[index] = newRarity;
			this.positionX[index] = newPosition.x;
			this.positionY[index] = newPosition.y;
			this.positionZ[index] = newPosition.z;
		}
	}

	public int[] getRarity()
	{
		return this.rarity;
	}

	public Vector3[] getPosition()
	{
		Vector3[] positions = new Vector3[this.Size];

		for(int i = 0; i < this.Size; i++)
		{
			positions[i] = new Vector3(this.positionX[i], this.positionY[i],
			                           this.positionZ[i]);
		}

		return positions;
	}

	public Vector3 getPositionAt(int index)
	{
		return new Vector3(this.positionX[index], this.positionY[index],
		                       this.positionZ[index]);
	}
}
