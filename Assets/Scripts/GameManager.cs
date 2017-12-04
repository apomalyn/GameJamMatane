using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
	
	public static GameManager instance;

	private BoardManager boardManager;

	private GameObject camera;

	private int levelCurrent = 0;
	
	private void Awake(){
		boardManager = BoardManager.instance;
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
	
	
}
