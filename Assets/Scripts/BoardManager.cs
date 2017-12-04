using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour{
    public enum TileType{
        Floor,
        Stair,
        None
    }

    public static BoardManager instance;
    
    private const int STAIR_HEIGHT = 3;
    private const int STAIR_WIDTH = 2;
    private const int SPAWN_RATE_TILE_SPE = 10;
    public int spawnRatePowerUp = 2;


    public int columns = 100; // The number of columns on the board (how wide it will be).
    public int rows = 100; // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20); // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(3, 10); // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(3, 10); // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(6, 10); // The range of lengths corridors between rooms can have.
    public IntRange numberEnemyByRoom = new IntRange(10, 40); // The range of enemies number.
    
    public GameObject[] floorTiles; // An array of wall tile prefabs.
    public GameObject[] outerWallTilesTop; // An array of outer wall tile prefabs.
    public GameObject[] outerWallTilesBottom; // An array of outer wall tile prefabs.
    public GameObject[] outerWallTilesSpe; // An array of outer wall special tile prefabs.
    public GameObject[] floorUnderWallTiles;
    public GameObject[] powerUpTilesTop;
    public GameObject[] powerUpTilesBottom;
    public GameObject wallSideTile;
    public GameObject stairTile;

    public GameObject[] enemies; //An array who contains different enemies
    private GameObject player;

    private TileType[][] tiles; // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms; // All the rooms that are created for this board.
    private Corridor[] corridors; // All the corridors that connect the rooms.
    private GameObject boardHolder; // GameObject that acts as a container for all other tiles.
    private GameObject enemiesHolder; // GameObject that acts as a container for all other enemies.

    private float sizeTile = 0.1600f;

    private void Awake(){
        instance = this;
    }

    public void nextLevel(){
        player = GameObject.Find("Character");
        
        Destroy(boardHolder);
        Destroy(enemiesHolder);
        
        boardHolder = new GameObject("BoardHolder");
        enemiesHolder = new GameObject("EnemisHolder");
        
        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        InstantiateEnemy();

        GameObject.Find("Main Camera").transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            -10
        );
    }


    private void SetupTilesArray(){
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


    private void CreateRoomsAndCorridors(){
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows, numberEnemyByRoom);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);
        
        for (int i = 1; i < rooms.Length; i++){
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1], numberEnemyByRoom);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length){
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
        }
        
        bool available = false;
        int indexRoom = Random.Range(0, rooms.Length);
        Room staircaseRoom = rooms[indexRoom];

        int xStaircase = staircaseRoom.xPos + Random.Range(1, staircaseRoom.roomWidth);
        int yStaircase = staircaseRoom.yPos;
                
        while (!available && indexRoom != -1){
            available = true;

            for (int j = 0; j < corridors.Length; j++){
                if (corridors[j].startXPos == xStaircase || corridors[j].EndPositionX == xStaircase
                    || corridors[j].startYPos == yStaircase || corridors[j].EndPositionY == yStaircase){
                    available = false;
                }
            }

            if (!available){
                if (xStaircase + 1 < staircaseRoom.xPos + staircaseRoom.roomWidth){
                    xStaircase++;
                }
                else{
                    indexRoom = (indexRoom+1 < rooms.Length) ? indexRoom + 1: -1;
                    if (indexRoom != -1){
                        staircaseRoom = rooms[indexRoom];

                        xStaircase = staircaseRoom.xPos + Random.Range(0, staircaseRoom.roomWidth);
                        yStaircase = staircaseRoom.yPos;
                    }
                }
            }
        }

        if (indexRoom == -1){
            Debug.Log("Stair is not at a good place, restart generation");
            rooms = new Room[1];
            corridors = new Corridor[1];
            
            CreateRoomsAndCorridors();
        }else{
            for (int i = 0; i < STAIR_WIDTH; i++){
                if (xStaircase - i >= 0 && yStaircase - 1 >= 0){
                    tiles[xStaircase][yStaircase] = TileType.Stair;
                    tiles[xStaircase - i][yStaircase - 1] = TileType.Stair;   
                }
            }
            Debug.Log("Stair start at: " + xStaircase + ":" + yStaircase);
            GameObject stair = Instantiate(stairTile, new Vector3((xStaircase - (STAIR_HEIGHT / 2)) * sizeTile - 0.005f, (yStaircase) * sizeTile - 0.01f, 0f), Quaternion.identity);
            stair.transform.parent = boardHolder.transform;   
        }

        indexRoom = (int)Mathf.Round(rooms.Length * .5f);
        int xCoordPlayer = rooms[indexRoom].xPos;
        int yCoordPlayer = rooms[indexRoom].yPos;
        
        Debug.Log("Player: " + xCoordPlayer + ":" + yCoordPlayer);
        Vector3 playerPos = new Vector3(xCoordPlayer * sizeTile, yCoordPlayer * sizeTile, 0);
        player.transform.position = playerPos;
    }


    private void SetTilesValuesForRooms(){
        for (int i = 0; i < rooms.Length; i++){
            Room currentRoom = rooms[i];

            for (int j = 0; j < currentRoom.roomWidth; j++){
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++){
                    int yCoord = currentRoom.yPos + k;
    
                    if(tiles[xCoord][yCoord] == TileType.None)
                        tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    private void SetTilesValuesForCorridors(){
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


    private void InstantiateTiles(){
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

    private void InstantiateEnemy(){
        int widthCurrentRoom;
        int heightCurrentRoom;

        int xCoordRandom;
        int yCoordRandom;
        int randomIndex;

        GameObject enemy;
        Vector3 position;
        
        foreach (Room room in rooms){
            widthCurrentRoom = room.roomWidth;
            heightCurrentRoom = room.roomHeight;

            for (int i = 0; i < room.nbEnemy; i++){
                randomIndex = Random.Range(0, enemies.Length);
                xCoordRandom = Random.Range(0, widthCurrentRoom);
                yCoordRandom = Random.Range(0, heightCurrentRoom);
                
                position = new Vector3(
                    (room.xPos + xCoordRandom) * sizeTile,
                    (room.yPos + yCoordRandom) * sizeTile,
                    0f
                );

                enemy = Instantiate(enemies[randomIndex], position, Quaternion.identity);
                enemy.transform.parent = enemiesHolder.transform;
            }
        }
    }

    private void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord){
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord * sizeTile, yCoord * sizeTile, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity);

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

    private void instantiateTopTile(float xCoord, float yCoord){
        int tileSpe = Random.Range(0, 100);
        int randomIndex;
        Vector3 position;
        GameObject tileInstance;

        if (tileSpe < spawnRatePowerUp){
            position = new Vector3(xCoord * sizeTile, yCoord * sizeTile + 0.115f, 0f);
            randomIndex = Random.Range(0, powerUpTilesTop.Length);
            tileInstance = Instantiate(powerUpTilesTop[randomIndex], position, Quaternion.identity);
            tileInstance.transform.parent = boardHolder.transform;
            
            position = new Vector3(xCoord * sizeTile, (yCoord-1) * sizeTile, 0f);
            tileInstance = Instantiate(powerUpTilesBottom[randomIndex], position, Quaternion.identity);
        } else if (tileSpe < SPAWN_RATE_TILE_SPE){
            position = new Vector3(xCoord * sizeTile, yCoord * sizeTile, 0f);
            randomIndex = Random.Range(0, outerWallTilesSpe.Length);
            tileInstance = Instantiate(outerWallTilesSpe[randomIndex], position, Quaternion.identity);
            tileInstance.transform.parent = boardHolder.transform;
            
            randomIndex = Random.Range(0, floorUnderWallTiles.Length);
            position = new Vector3(xCoord * sizeTile, (yCoord-1) * sizeTile, 0f);
            tileInstance = Instantiate(floorUnderWallTiles[randomIndex], position, Quaternion.identity);
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

    public TileType[][] getTilesGrid(){
        return tiles;
    }

    public GameObject getEnemiesHolder(){
        return enemiesHolder;
    }
}