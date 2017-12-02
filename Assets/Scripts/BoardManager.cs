using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour{
    [Serializable]
    public class Count{
        public int mini;
        public int max;

        public Count(int min, int max){
            this.max = max;
            this.mini = min;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count specialCount = new Count(3, 7);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] specialTiles;
    public GameObject[] ennemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void initialiseList(){
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++){
            for (int y = 1; y < rows - 1; y++){
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void boardSetup(){
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns; x++){
            for (int y = -1; y < rows; y++){
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows){
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 randomPosition(){
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    void layoutObjectRandom(GameObject[] tileArray, int mini, int max){
        int objectCount = Random.Range(mini, max + 1);

        for (int i = 0; i < objectCount; i++){
            Vector3 randomPosition = this.randomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void setupScene(int level){
        boardSetup();
        initialiseList();
        layoutObjectRandom(wallTiles, wallCount.mini, wallCount.max);
        layoutObjectRandom(specialTiles, specialCount.mini, specialCount.max);

        int enemyCount = (int) Mathf.Log(level, 2f);
        layoutObjectRandom(ennemyTiles, enemyCount, enemyCount);

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

    }
}