using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour{
    public enum TileType{
        Floor,
        Side,
        TopOuter,
        BottomOuter,
        None
    }


    public int columns = 100; // The number of columns on the board (how wide it will be).
    public int rows = 100; // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20); // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(3, 10); // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(3, 10); // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(6, 10); // The range of lengths corridors between rooms can have.
    public GameObject[] floorTiles; // An array of wall tile prefabs.
    public GameObject[] outerWallTilesTop; // An array of outer wall tile prefabs.
    public GameObject[] outerWallTilesBottom; // An array of outer wall tile prefabs.
    public GameObject[] floorUnderWallTiles;
    public GameObject[] powerUpTilesTop;
    public GameObject[] powerUpTilesBottom;
    public GameObject wallSideTile;
    public GameObject player;

    private TileType[][] tiles; // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms; // All the rooms that are created for this board.
    private Corridor[] corridors; // All the corridors that connect the rooms.
    private GameObject boardHolder; // GameObject that acts as a container for all other tiles.

    private float sizeTile = 0.1600f;

    private void Start(){
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        //InstantiateOuterWalls();
    }


    void SetupTilesArray(){
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns+1][];

        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++){
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[rows+1];
            for (int j = 0; j < tiles[i].Length; j++){
                tiles[i][j] = TileType.None;
            }
        }
    }


    void CreateRoomsAndCorridors(){
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++){
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length){
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }

            if (i == rooms.Length * .5f){
                Vector3 playerPos = new Vector3(rooms[i].xPos * sizeTile, rooms[i].yPos * sizeTile, 0);
                //Instantiate(player, playerPos, Quaternion.identity);
            }
        }
    }


    void SetTilesValuesForRooms(){
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++){
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++){
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++){
                    int yCoord = currentRoom.yPos + k;

                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors(){
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++){
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++){
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction){
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }
                
                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles(){
        bool draw;
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++){
            for (int j = 0; j < tiles[i].Length; j++){
                draw = true;
                // ... and instantiate a floor tile for it.
                if (tiles[i][j] == TileType.Floor){
                    if ((i + 1) <= columns && tiles[i + 1][j] == TileType.None){
                        GameObject tileInstance = Instantiate(wallSideTile, new Vector3((i + 1) * sizeTile, j * sizeTile, 0f), Quaternion.identity);
                        tileInstance.transform.parent = boardHolder.transform;
                    }

                    if ((i - 1) >= 0 && tiles[i - 1][j] == TileType.None){
                        GameObject tileInstance = Instantiate(wallSideTile, new Vector3((i - 1) * sizeTile, j * sizeTile, 0f), Quaternion.identity);
                        tileInstance.transform.parent = boardHolder.transform;
                    }

                    if ((j + 1) <= rows && tiles[i][j + 1] == TileType.None){
                        instantiateTopTile(i, j + 1);
                        draw = false;
                    }
                    
                    if ((j - 1) >= 0 && tiles[i][j - 1] == TileType.None){
                        InstantiateFromArray(outerWallTilesBottom, i, j - 1);
                    }

                    //Update if spe tile is set on wall
                    if (tiles[i][j] == TileType.Floor && draw){
                        InstantiateFromArray(floorTiles, i, j);
                    }
                }
            }
        }
    }

    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord){
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord * sizeTile, yCoord * sizeTile, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity);

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

    void instantiateTopTile(float xCoord, float yCoord){
        int tileSpe = Random.Range(0, 100);
        int randomIndex;
        Vector3 position;
        GameObject tileInstance;

        if (tileSpe < 2){
            position = new Vector3(xCoord * sizeTile, yCoord * sizeTile + 0.115f, 0f);
            randomIndex = Random.Range(0, powerUpTilesTop.Length);
            tileInstance = Instantiate(powerUpTilesTop[randomIndex], position, Quaternion.identity);
            tileInstance.transform.parent = boardHolder.transform;
            
            position = new Vector3(xCoord * sizeTile, (yCoord-1) * sizeTile, 0f);
            tileInstance = Instantiate(powerUpTilesBottom[randomIndex], position, Quaternion.identity);
        } else{
            
            position = new Vector3(xCoord * sizeTile, yCoord * sizeTile, 0f);
            randomIndex = Random.Range(0, outerWallTilesTop.Length);
            tileInstance = Instantiate(outerWallTilesTop[randomIndex], position, Quaternion.identity);
            tileInstance.transform.parent = boardHolder.transform;
            
            randomIndex = Random.Range(0, floorUnderWallTiles.Length);
            position = new Vector3(xCoord * sizeTile, (yCoord-1) * sizeTile, 0f);
            tileInstance = Instantiate(floorUnderWallTiles[randomIndex], position, Quaternion.identity);
        }

        tileInstance.transform.parent = boardHolder.transform;
    }
}