using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class Board_Manager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8; //Columns
    public int rows = 8; //Dimensions of game board

    public Count wallCount = new Count(5, 9); //Lower and upper limit for the number of walls in each level

    public Count foodCount = new Count(1, 5); //Lower and upper limit for the number of food items per level

    public GameObject exit; //Prefab to spawn for exit (game objects are created to occupy the world created)
    public GameObject[] floorTiles; //Arrays for the prefabs (all of each kind?)
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;  //Reference to the transform (position component) of Board object
    private List<Vector3> gridPositions = new List<Vector3>(); //List of possible positions to place tiles

    //Clears the gridPositions list to prepare it for the next randomly generated board
    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {   //Adds a vector that represents the position of every location in the board 
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Sets up the outerwall and floor (background)
    void BoardSetup()
    {   //Instantiate game board with a reference to its transform (position component)
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {   //For every column and row of our board, select a random floortile prefab to instantiate
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                //If at the edges, use outerWall prefabs instead
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                //Instantiate the gameobject specified above and place it at (x,y,0)    
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Sets parent of the newly instantiated object to boardHolder
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    //Returns, in the form of a Vector3 object, a random position from list gridPositions
    Vector3 RandomPosition()
    {   //Get a random number between 0 and the number of elements in gridPosition (list)
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Get as a Vector3 object, the position at randomIndex from gridPosition list
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry selected at random so that we prevent trying to use that position again
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position
        return randomPosition;
    }

    //Accepts an array of GameObjects to choose from to instantiate and place in our board
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {   //Random number of objects of the type passed to this function to instantiate
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate the objects (place on board)
        for (int i = 0; i < objectCount; i++)
        {
            //Call RandomPosition to get a random position from our gridPosition list
            Vector3 randomPosition = RandomPosition();

            //Get a reference to a GameObject of the current type (among the possible prefabs)
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate the object of type tileChoice to place in randomPosition
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    //Initializes the level by calling this class's functions   
    public void SetupScene(int level)
    {
        //Creates the outerwalls and floors
        BoardSetup();

        //Clears and prepares list (gridPositions) for positions of tiles (prefabs for foreground units and items)
        InitializeList();

        //Instantiate a number of GameObjects of the wall types
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        //Instantiate a number of GameObjects of the food types
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        //Get a number for the amount of enemies to instantiate in this level
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate enemy tiles based on the count calculated above
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //Instantiate the exit GameObject using a prefab
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0F), Quaternion.identity);
    }
}
