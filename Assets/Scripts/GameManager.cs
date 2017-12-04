using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
	
	public static GameManager instance;

	private BoardManager boardManager;

	private GameObject camera;

	private CharacterController player;

	private int levelCurrent = 0;
	
	private void Awake(){
		instance = this;
		boardManager = BoardManager.instance;
		camera = GameObject.Find("Main Camera");
		player = GameObject.Find("Character").GetComponent<CharacterController>();
		InitGame();
	}

	public void updateCamera(Vector3 position){
		camera.transform.position = new Vector3(
			position.x,
			position.y,
			-10
		);
	}

	private void InitGame(){
		boardManager.nextLevel();
	}

	public void nextTurn(){
		EnnemyController[] scripts = boardManager.getEnemiesHolder().GetComponentsInChildren<EnnemyController>();
		for (int i = 0; i < scripts.Length; i++){
			scripts[i].nextMove();
		}
	}

	public void inscreaseScore(){
		player.
	}
	
}
