using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
	public enum EnemyType{
		Bat,
		Skeleton,
		BlackMage
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

	public EnemyType type;
	
	// Pattern de mouvement
	private int[,] pattern = {
		{4, 3, 4, 3},
		{4, 4, 0, 0},
		{4, 4, 0, 0}
	};

	private Direction[,] directionPattern = {
		{Direction.Up, Direction.Left, Direction.Up, Direction.Right},
		{Direction.Up, Direction.Down, Direction.None, Direction.None},
		{Direction.Left, Direction.Right, Direction.None, Direction.None},
	};
	
	
	// Use this for initialization
	void Start ()
	{
		ennemy_body = GetComponent<Rigidbody2D>();
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		deplacement();
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player") 
		{	
			life -= 1;
			if (life <= 0) 
			{
				destroy();
			}
		}
	}

	void deplacement()
	{
		
	}

	void destroy()
	{
		Destroy(gameObject);
	}
}
