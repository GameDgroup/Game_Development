using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {

    public BoardManager boardScript; //Reference to the class that has the methods for
    //setting up our gameboard by instantiating the floor (background) and other units
    //(items such as food and walls) as well as enemies

    private int level = 3;

    void Awake ()
    {
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }
}
