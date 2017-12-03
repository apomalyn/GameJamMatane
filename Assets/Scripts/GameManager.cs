using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

	private BoardManager boardManager;

	private GameObject camera;

	private int levelCurrent = 0;
	
	private void Awake(){
		boardManager = BoardManager.instance;
		InitGame();
	}

	private void InitGame(){
		boardManager.nextLevel();
	}
	
	
}
