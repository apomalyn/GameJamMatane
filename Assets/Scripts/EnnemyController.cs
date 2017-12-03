using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.WSA;

public class EnnemyController : MonoBehaviour
{
	public enum EnemyType{
		Bat = 0,
		Skeleton = 1,
		BlackMage = 2
	}

	public enum Direction{
		Up,
		Down,
		Left,
		Right,
		None
	}
	
	public float tileSize = 0.16f;
	
	
	private int life;
	private Rigidbody2D ennemy_body;
	private int restMove = 0;
	private int indexCurrentMove = 0;

	public Transform target;
	public float posTargetx;
	public float posTargety;
	private BoardManager.TileType [][] tiles;
	private bool targetOk = false;
	private int[,] open;
	private int[,] close;
	
	public bool go = true;
	
	public EnemyType type;
	
	// Pattern de mouvement
	private int[,] pattern = {
		{4, 3, 4, 3},
		{4, 4, 0, 0},
		{4, 4, 0, 0}
	};

	private const int MAX_MOVE = 4;

	private Direction[,] directionPattern = {
		{Direction.Up, Direction.Left, Direction.Up, Direction.Right},
		{Direction.Up, Direction.Down, Direction.None, Direction.None},
		{Direction.Left, Direction.Right, Direction.None, Direction.None},
	};
	
	
	// Use this for initialization
	void Start ()
	{
		ennemy_body = GetComponent<Rigidbody2D>();
		tiles = BoardManager.instance.getTilesGrid();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (go)
		{
			deplacement();
			go = false;
		}
		else
		{
			System.Threading.Thread.Sleep(2000);
			go = true;
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			targetOk = true;
		}
	}

	void deplacement()
	{
		if (targetOk)
		{
			posTargetx = target.gameObject.transform.position.x;
			posTargety = target.gameObject.transform.position.y;
			posTargetx = Mathf.Round(posTargetx / .1600f);
			posTargety = Mathf.Round(posTargety / .1600f);
			float posx = Mathf.Round(gameObject.transform.position.x / 0.1600f);
			float posy = Mathf.Round(gameObject.transform.position.y / 0.1600f);

		}
		else{
			Vector3 position = new Vector3();

			if (restMove == 0)
			{
				indexCurrentMove = (indexCurrentMove + 1 < MAX_MOVE) ? indexCurrentMove + 1 : 0;
				while (pattern[(int) type, indexCurrentMove] == 0)
				{
					indexCurrentMove = (indexCurrentMove + 1 < MAX_MOVE) ? indexCurrentMove + 1 : 0;
				}
				restMove = pattern[(int) type, indexCurrentMove];
			}

			switch (directionPattern[(int) type, indexCurrentMove])
			{
				case Direction.Up:
					//Translate
					position = new Vector3(0, 0.1600f, 0);
					break;
				case Direction.Down:
					//Translate
					position = new Vector3(0, -0.1600f, 0);
					break;
				case Direction.Left:

					//Translate
					position = new Vector3(-0.1600f, 0, 0);
					break;
				case Direction.Right:
					position = new Vector3(0.1600f, 0, 0);
					break;
			}
			if (position != new Vector3())
			{
				ennemy_body.transform.Translate(position, Space.World);
				restMove--;
			}
		}
	}

	void destroy()
	{
		Destroy(gameObject);
	}
}
