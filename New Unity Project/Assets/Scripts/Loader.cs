using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameManager; 

	void Awake () {
        //Checks whether the static member of the GameManager class has been instantiated
        //A static member variable is accessible by all instances of the pertaining class
        //but there is a single instance of that variable. No single instance of the class 
        //has an instance of that variable  
        if (GameManager.instance == null)
            Instantiate(gameManager);
	}
	
}
