using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

	public BoardManager boardManager;

	private int level = 3;

	private void Awake(){
		boardManager = GetComponent<BoardManager>();
		InitGame();
	}

	private void InitGame(){
	}
}
