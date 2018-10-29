/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public int wallDamage = 1; //Amount of damage character deals to walls
    public int pointsPerFood = 10; //Amount of food points obtained from consuming a food item
    public int pointsPerSoda = 20; //Amount of food points obtained from consuming a soda item
    public float restartLevelDelay = 1f; //Time in seconds to delay restart of a level

    private Animator animator; //Reference variable to store a reference to the character's animator component
    private int food; //Total food points

	// Use this for initialization
	protected override void Start ()
    {
        //Get this object's animator component
        animator = GetComponent<Animator>();

        //GameManager has a static variable instance that is public and can
        //be used to access the GameManager object's public members through
        //This singleton
        food = GameManager.instance.playerFoodPoints;

        //base refers to the base or superclass but MovingObject does not
        //specifically implement one. Presumably, the base class is MonoBehaviour
        //or GameObject or another in the inheritance hierarchy that implements
        //Start()
        base.Start();
	}

    //Use this method to store the player's food points in the GameManager singleton
    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    void Update()
    {
        //Not the player's turn
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        //Get input value for horizontal movement
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        //Get input value for vertical movement
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Move only along horizontal axis
        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
    }

    protected override void OnCantMove<T>(T component)
    {
        //Cast the component as a Wall object
        Wall hitWall = component as Wall;
        //Call the Wall object's DamageWall method
        hitWall.DamageWall(wallDamage);
        //Set the character's animator trigger to play the Player_Chop animation
        animator.SetTrigger("Player_Chop");
    }

    public void Restart()
    {
        //Loads the last scence loaded. Main because it is the only level
        //and each is procedurally generated
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int loss)
    {
        //Set the trigger for the character object's animator so that
        //the Player_Hit animation is initiated
        animator.SetTrigger("Player_Hit");

        food -= loss; //Amount of damage the character takes

        //Determine whether the character's food points have depleted completely
        CheckIfGameOver();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--; //Part of the game's mechanics to use a food point for every unit moved

        //Attempt to move using the base class's implementation of it
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit; //Used in the following call to move which takes a reference parameter

        //Move(xDir, yDir, out hit);
        
        CheckIfGameOver(); //Because food points deplete with every movement, must check if game is over

        //If the game is not over, the player's turn is over after a movement
        GameManager.instance.playersTurn = false;
    }

    //Implements game mechanics for character
    private void OnTriggerEnter2D(Collider2D otherObject)
    {
       if (otherObject.tag == "Exit")
        {
            //Call method that ends the level
            Invoke("Restart", restartLevelDelay);
            enabled = false; //Disable the character while the level restarts
        }
       else if (otherObject.tag == "Food")
        {
            food += pointsPerFood; //Increase character's food points
            //Deactivate the food item on the game board
            otherObject.gameObject.SetActive(false);
        }
       else if (otherObject.tag == "Soda")
        {
            food += pointsPerSoda; //Increase character's food points
            //Deactivate the soda item on the game board
            otherObject.gameObject.SetActive(false);
        }
    }
    private void CheckIfGameOver()
    {
        //If food points have been depleted, the game is over and the
        //GameManager singleton is used to invoke its GameOver function
        //to disable the game
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;      //Allows us to use SceneManager

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
    public float restartLevelDelay = 0.1f;        //Delay time in seconds to restart level.
    public int pointsPerFood = 10;              //Number of points to add to player food points when picking up a food object.
    public int pointsPerSoda = 20;              //Number of points to add to player food points when picking up a soda object.
    public int wallDamage = 1;                  //How much damage a player does to a wall when chopping it.
    public float speed;

    private Animator animator;                  //Used to store a reference to the Player's animator component.
    private int food;                           //Used to store player food points total during level.
    private Vector2 moveVelocity;
    private Rigidbody2D rb;

    //Start overrides the Start function of MovingObject
    protected override void Start()
    {
        //Get a component reference to the Player's animator component
        animator = GetComponent<Animator>();

        //Get the current food point total stored in GameManager.instance between levels.
        food = GameManager.instance.playerFoodPoints;

        rb = GetComponent<Rigidbody2D>();
        //Call the Start function of the MovingObject base class.
        base.Start();
    }


    //This function is called when the behaviour becomes disabled or inactive.
    private void OnDisable()
    {
        //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
        //Occurs between changes in levels to store the accumulated food points in the game manager
        GameManager.instance.playerFoodPoints = food;
    }


    private void Update()
    {
        /*//If it's not the player's turn, exit the function.
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.


        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        //Check if we have a non-zero value for horizontal or vertical
        if (horizontal != 0 || vertical != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<Wall>(horizontal, vertical);
        }*/

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
    }



    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        AttemptMove<Wall>((int)rb.position.x, (int)rb.position.y);
        //if (Input.GetKey("a")) animator.SetTrigger("playerChop");

    }



    //AttemptMove overrides the AttemptMove function in the base class MovingObject
    //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //Every time player moves, subtract from food points total.
        food--;

        //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove<T>(xDir, yDir);

        //Hit allows us to reference the result of the Linecast done in Move.
        RaycastHit2D hit;

        //If Move returns true, meaning Player was able to move into an empty space.
        if (Move(xDir, yDir, out hit))
        {
            //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
        }

        //Since the player has moved and lost food points, check if the game has ended.
        CheckIfGameOver();

        //Set the playersTurn boolean of GameManager to false now that players turn is over.
        //GameManager.instance.playersTurn = false;
    }


    //OnCantMove overrides the abstract function OnCantMove in MovingObject.
    //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
    protected override void OnCantMove<T>(T component)
    {
        //Set hitWall to equal the component passed in as a parameter.
        Wall hitWall = component as Wall;

        //Call the DamageWall function of the Wall we are hitting.
        hitWall.DamageWall(wallDamage);

        //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
        animator.SetTrigger("playerChop");
    }


    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit")
        {
            //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("Restart", restartLevelDelay);

            //Disable the player object since level is over.
            enabled = false;
        }

        //Check if the tag of the trigger collided with is Food.
        else if (other.tag == "Food")
        {
            //Add pointsPerFood to the players current food total.
            food += pointsPerFood;

            //Disable the food object the player collided with.
            other.gameObject.SetActive(false);
        }

        //Check if the tag of the trigger collided with is Soda.
        else if (other.tag == "Soda")
        {
            //Add pointsPerSoda to players food points total
            food += pointsPerSoda;


            //Disable the soda object the player collided with.
            other.gameObject.SetActive(false);
        }
    }


    //Restart reloads the scene when called.
    private void Restart()
    {
        //Load the last scene loaded, in this case Main, the only scene in the game.
        SceneManager.LoadScene(0);
    }


    //LoseFood is called when an enemy attacks the player.
    //It takes a parameter loss which specifies how many points to lose.
    public void LoseFood(int loss)
    {
        //Set the trigger for the player animator to transition to the playerHit animation.
        animator.SetTrigger("playerHit");

        //Subtract lost food points from the players total.
        food -= loss;

        //Check to see if game has ended.
        CheckIfGameOver();
    }


    //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
    private void CheckIfGameOver()
    {
        //Check if food point total is less than or equal to zero.
        if (food <= 0)
        {

            //Call the GameOver function of GameManager.
            GameManager.instance.GameOver();
        }
    }
}