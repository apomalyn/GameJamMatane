using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

	private BoardManager boardManager;

	private int levelCurrent = 0;

	private void Awake(){
		boardManager = GetComponent<BoardManager>();
		InitGame();
	}

	private void InitGame(){
		boardManager.nextLevel();
	}
}
