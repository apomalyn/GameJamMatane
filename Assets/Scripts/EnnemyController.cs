using System;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : Entity {
	public enum EnemyType{
		Bat = 0,
		Skeleton = 1,
		BlackMage = 2
	}

	private enum State{
		Pacific,
		Aggressive
	}

	private const int MAX_TURN_AGGRESSIVE = 5;
	
	public float tileSize = 0.16f;

	private LayerMask playerLayer;

	private State currentState = State.Pacific;
	private List<Direction> pathInAggressive;

	private int life;
	private int restMove = 0;
	private int indexCurrentMove = 0;

	public Tile target;
	private BoardManager.TileType [][] tiles;
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
	
	void Start ()
	{
		tiles = BoardManager.instance.getTilesGrid();
		restMove = pattern[(int) type, indexCurrentMove];
		playerLayer = LayerMask.NameToLayer("player");
		pathInAggressive = new List<Direction>();
	}

	public void nextMove(){

		search();

		Direction next = Direction.Up;
		
		if (currentState == State.Pacific){
			if (pathInAggressive.Count > 0){
				next = pathInAggressive[pathInAggressive.Count - 1];
				pathInAggressive.RemoveAt(pathInAggressive.Count - 1);
			}else{
				if (restMove == 0){
					indexCurrentMove =
						(indexCurrentMove + 1 < MAX_MOVE && directionPattern[(int) type, indexCurrentMove + 1] != Direction.None)
							? indexCurrentMove + 1
							: 0;
					restMove = pattern[(int) type, indexCurrentMove];
				}

				restMove--;
				next = directionPattern[(int) type, indexCurrentMove];
			}
		}else{
			target = GameManager.instance.getPositionPlayer();
			next = searchNextTile();
		}
		
		tryMove(next);
	}

	private void search(){
		Direction direction = Direction.None;
		if (Physics2D.Raycast(transform.position, Vector2.up, TILE_SIZE * 2, 1 << playerLayer.value)){
			direction = Direction.Up;
		}else if (Physics2D.Raycast(transform.position, Vector2.down, TILE_SIZE * 3, 1 << playerLayer.value)){
			direction = Direction.Down;
		}else if (Physics2D.Raycast(transform.position, Vector2.left, TILE_SIZE * 3, 1 << playerLayer.value)){
			direction = Direction.Left;
		}else if (Physics2D.Raycast(transform.position, Vector2.right, TILE_SIZE * 3, 1 << playerLayer.value)){
			direction = Direction.Right;
		}

		if (direction != Direction.None){
			if (currentState == State.Pacific){
				currentState = State.Aggressive;
				pathInAggressive.Reverse();
				pathInAggressive.Add(direction);
			}else if(pathInAggressive.Count > MAX_TURN_AGGRESSIVE){
				pathInAggressive.Reverse();
				currentState = State.Pacific;
			}
		}
	}

	private Direction searchNextTile(){
		Tile next = pathfinding();

		Direction direction = Direction.None;
		
		if (position.x + 1 == next.x && position.y == next.y){
			direction = Direction.Right;
		}else if (position.x == next.x && position.y + 1 == next.y){
			direction = Direction.Up;
		}else if (position.x == next.x && position.y - 1 == next.y){
			direction = Direction.Down;
		}else if (position.x - 1 == next.x && position.y == next.y){
			direction = Direction.Left;
		}
		
		pathInAggressive.Add(direction);

		return direction;
	}

	private Tile pathfinding(){
		
		List<Tile> toVisit = new List<Tile>();
		Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
		Dictionary<Tile, int> cost = new Dictionary<Tile, int>();

		int xCurrent = position.x;
		int yCurrent = position.y;

		switch (pathInAggressive[pathInAggressive.Count - 1]){
			case Direction.Up:
				yCurrent++;
				break;
			case Direction.Down:
				yCurrent--;
				break;
			case Direction.Right:
				xCurrent++;
				break;
			case Direction.Left:
				xCurrent--;
				break;
		}

		Tile current = new Tile(xCurrent, yCurrent);

		cameFrom.Add(current, current);
		cost.Add(current, 1);
		toVisit.Add(current);

		for (int i = 0; i < 4; i++){
			for (int j = 0; j < 4; j++){
				current = toVisit[0];
				toVisit.Remove(current);

				if (current.x == target.x && current.y == target.y){
					return nextForFind(target, cameFrom, cost);
				}

				int newCost = cost[current] + 1;

				foreach (Tile neighbour in getNeighbours(current)){
					if (neighbour.x == position.x && neighbour.y == position.y){
						
					}else if (!cameFrom.ContainsKey(neighbour)){
						cost.Add(neighbour, newCost);
						cameFrom.Add(neighbour, current);
						toVisit.Add(neighbour);
					}
					else if (newCost < cost[neighbour]){
						cost[neighbour] = newCost;
						cameFrom.Remove(neighbour);
						cameFrom.Add(neighbour, current);
					}
				}
			}
		}

		return nextForFind(current, cameFrom, cost);
	}

	private Tile nextForFind(Tile targetTile, Dictionary<Tile, Tile> cameFrom, Dictionary<Tile, int> cost){
		foreach (var node in cost){
			if (targetTile.x == node.Key.x && targetTile.y == node.Key.y && node.Value == 1){
				return targetTile;
			}
		}
		
		foreach (var node in cameFrom){
			if (node.Key.x == targetTile.x && node.Key.y == targetTile.y){
				return nextForFind(node.Value, cameFrom, cost);
			} 
		}

		return targetTile;
	}

	private List<Tile> getNeighbours(Tile node){
		List<Tile> neighbours = new List<Tile>();

		if (node.y + 1 < tiles[0].Length &&
		    tiles[node.x][node.y + 1] == BoardManager.TileType.Floor){	// 0:+1
			neighbours.Add(new Tile(node.x, node.y + 1));
		}
		
		if (node.y - 1 < tiles[0].Length &&
		    tiles[node.x][node.y - 1] == BoardManager.TileType.Floor){	// 0:-1
			neighbours.Add(new Tile(node.x, node.y - 1));
		}
		
		if (node.x + 1 < tiles.Length &&
		    tiles[node.x + 1][node.y] == BoardManager.TileType.Floor){	// +1:0
			neighbours.Add(new Tile(node.x + 1, node.y));
		}
		
		if (node.x - 1 < tiles.Length &&
		    tiles[node.x - 1][node.y] == BoardManager.TileType.Floor){	// -1:0
			neighbours.Add(new Tile(node.x - 1, node.y));
		}

		return neighbours;
	}
	
	public override bool hit(){
		life--;
		
		if (life < 1){
			Destroy(gameObject);
			return true;
		}
		return false;
	}
	
	public Tile getPosition(){
		return base.getPosition();
	}

	public void setPosition(Tile tile){
		base.setPosition(tile);
	}
}
