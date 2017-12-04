using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
	
	public static GameManager instance;

	private BoardManager boardManager;

	private GameObject camera;

	public GameObject player;
	
	private CharacterController playerScript;

	private int levelCurrent = 0;
	
	private void Awake(){
		instance = this;
		boardManager = BoardManager.instance;
		camera = GameObject.Find("Main Camera");
		playerScript = player.GetComponent<CharacterController>();
		loadLevel();
	}

	public void updateCamera(Vector3 position){
		camera.transform.position = new Vector3(
			position.x,
			position.y,
			-10
		);
	}

	public void loadLevel(){
		boardManager.nextLevel();
	}

	public void nextTurn(){
		EnnemyController[] scripts = boardManager.getEnemiesHolder().GetComponentsInChildren<EnnemyController>();
		for (int i = 0; i < scripts.Length; i++){
			scripts[i].nextMove();
		}
	}

	public Vector3 getPositionPlayer(){
		return player.transform.position;
	}
}
