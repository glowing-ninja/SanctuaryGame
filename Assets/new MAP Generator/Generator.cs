using System.Collections;
using UnityEngine;

public class Rectangle {
	private static int MIN_SIZE = 7;
	private static int MIN_DUNGEON_SIZE = MIN_SIZE - 1;
	//private static Random rnd = new Random(); 

	public int top, left, width, height;
	public Rectangle leftChild;
	public Rectangle rightChild;
	public Rectangle dungeon;
	public Rectangle bridge;
	public bool horizontal;

	public Rectangle(int top, int left, int height, int width) {
		this.top = top;
		this.left = left;
		this.width = width;
		this.height = height;
		this.leftChild = null;
		this.rightChild = null;
		this.dungeon = null;

	}

	public static bool nextBoolean () {
		if (Random.Range (0, 100) < 50)
			return false;
		return true;
	}

	public static int nextInt (int max) {
		return Random.Range (0, max);
	}

	public void Show () {
		Debug.Log ("T"+top + ", L" + left + ", W" + width + ", H" + height);
	}
	/*
1: start with the entire dungeon area (root node of the BSP tree)
2: divide the area along a horizontal or vertical line
3: select one of the two new partition cells
4: if this cell is bigger than the minimal acceptable size:
5: go to step 2 (using this cell as the area to be divided)
6: select the other partition cell, and go to step 4
7: for every partition cell:
8: create a room within the cell by randomly
choosing two points (top left and bottom right)
within its boundaries
9: starting from the lowest layers, draw corridors to connect
rooms in the nodes of the BSP tree with children of the same
parent
10:repeat 9 until the children of the root node are connected
*/
	public bool split() {
		horizontal = nextBoolean(); //direction of split
		int max = (horizontal ? height : width ) - MIN_SIZE; //maximum height/width we can split off
		if( max <= MIN_SIZE ) // area too small to split, bail out
			return false;
		int split = nextInt( max );
		if (split < MIN_SIZE)
			split = MIN_SIZE;
		if( horizontal ) { //populate child areas
			leftChild = new Rectangle( top, left, split, width );
			rightChild = new Rectangle( top+split, left, height-split, width );
		} else {
			leftChild = new Rectangle( top, left, height, split );
			rightChild = new Rectangle( top, left+split, height, width-split );
		}
		leftChild.split ();
		rightChild.split ();
		return true;
	}

	public void generateDungeon() {
		if( leftChild != null ) { //if current are has child areas, propagate the call
			leftChild.generateDungeon();
			rightChild.generateDungeon();
			if (horizontal) {
				dungeon = new Rectangle(leftChild.dungeon.top,
				                        leftChild.dungeon.left,
				                        rightChild.dungeon.top + rightChild.dungeon.height - leftChild.dungeon.top,
				                        Mathf.Max(leftChild.dungeon.left + leftChild.dungeon.width, rightChild.dungeon.left + rightChild.dungeon.width) - 
				                        		Mathf.Min(leftChild.dungeon.left, rightChild.dungeon.left));
				bridge = new Rectangle (leftChild.dungeon.top + leftChild.dungeon.height,
				                        //Mathf.Max(leftChild.dungeon.left, rightChild.dungeon.left),
				                        Random.Range(dungeon.left, dungeon.left+dungeon.width),
				                        rightChild.dungeon.top - leftChild.dungeon.height - leftChild.dungeon.top,
				                        1);
			}
			else {
				dungeon = new Rectangle(leftChild.dungeon.top,
				                        leftChild.dungeon.left,
				                        Mathf.Max(leftChild.dungeon.top + leftChild.dungeon.height, rightChild.dungeon.top + rightChild.dungeon.height) - 
				                        	Mathf.Min(leftChild.dungeon.top, rightChild.dungeon.top),
										rightChild.dungeon.left + rightChild.dungeon.width - leftChild.dungeon.left);
				bridge = new Rectangle (Random.Range(dungeon.top, dungeon.top + dungeon.height),
				                        leftChild.dungeon.left + leftChild.dungeon.width,
			                        	1,
				                        rightChild.dungeon.left - leftChild.dungeon.width - leftChild.dungeon.left);
			}
		} else {
			int dungeonTop = Rectangle.nextInt(height  / 4) + 1;
			int dungeonLeft = Rectangle.nextInt(width  / 4) + 1;
			int dungeonHeight = Mathf.Max(Rectangle.nextInt( height - dungeonTop ), height / 2);
			int dungeonWidth = Mathf.Max(Rectangle.nextInt( width - dungeonLeft), width / 2 );
			dungeon = new Rectangle( top + dungeonTop, left+dungeonLeft, dungeonHeight, dungeonWidth);
			dungeon.Show();
		}
	}
	
	public void PrintNode (GameObject floor, float piso) {
		
		Color temp = new Color(Random.Range(0,100) / 100f,
		                       Random.Range(0,100) / 100f,
		                       Random.Range(0,100) / 100f);
		for( int i = 0; i < height; i++ ) {
			for( int j = 0; j < width; j++ ) {
				Vector3 pos = new Vector3(left * 10 + j * 10 + 5, piso, top * 10 + i * 10 + 5);
				GameObject suelo = GameObject.Instantiate (floor, pos, Quaternion.identity) as GameObject;
				suelo.GetComponent<Renderer>().material.color = temp;
			}
		}
		if (leftChild == null) {
			for( int i = 0; i < dungeon.height; i++ ) {
				for( int j = 0; j < dungeon.width; j++ ) {
					Vector3 pos = new Vector3(dungeon.left * 10 + j * 10 + 5, -1, dungeon.top * 10 + i * 10 + 5);
					GameObject.Instantiate (floor, pos, Quaternion.identity);
				}
			}

		}
		else {
			for( int i = 0; i < bridge.height; i++ ) {
				for( int j = 0; j < bridge.width; j++ ) {
					Vector3 pos = new Vector3(bridge.left * 10 + j * 10 + 5, -1, bridge.top * 10 + i * 10 + 5);
					
					GameObject puente = GameObject.Instantiate (floor, pos, Quaternion.identity) as GameObject;
					puente.GetComponent<Renderer>().material.color = Color.black;
					puente.name = "Puente";
				}
			}
			leftChild.PrintNode(floor, piso+1f);
			rightChild.PrintNode(floor, piso+1f);
		}
	}
}

public class Generator : MonoBehaviour{
	
	public GameObject floor;

	void Start() {

		/*int trues = 0;
		int falses = 0;
		for (int i = 0; i < 10000; i++) {
			if (Rectangle.nextBoolean())
				trues++;
			else
				falses++;
		}

		Debug.Log ("true: " + trues + ", falses: " + falses);
*/
		Rectangle root = new Rectangle( 0, 0, 50, 50); 
		root.split ();
		root.generateDungeon ();
		
		printDungeons(root); //this is just to test the output
		
	}
	
	private void printDungeons(Rectangle root) {
		root.PrintNode (floor, 0f);
	}

}