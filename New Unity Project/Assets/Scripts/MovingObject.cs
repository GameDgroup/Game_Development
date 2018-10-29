using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//As an abstract class, this class cannot be instantiated but it can be
//used as a template for other classes who use the functionality and
//attributes declared here
public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;
    //Amount of time, in seconds, allotted for the movement of an instance
    //of a class that inherits from this class.

    public LayerMask blockingLayer; //Layer on which to check for collisions of MovingObjects

    private BoxCollider2D boxCollider; //Reference to MovingObject's BoxCollider component
    private Rigidbody2D rb2D; //Reference to MovingObject's RigidBody2D component
    
    private float inverseMoveTime; //

	// Use this for initialization
    //Protected to allow the inheriting object to modify the implementation for itself
	protected virtual void Start ()
    {
        boxCollider = GetComponent<BoxCollider2D>(); //Get MovingObject's BoxCollider2D component
        //rb2D = GetComponent<Rigidbody2D>(); //Get MovingObject's RigidBody2D component
        //inverseMoveTime = 1f / moveTime;
	}

    //out is used to pass arguments by reference as we need to return more
    //than a single value
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //Get MovingObject's current position cast as a 2D vector
        //z-component is discarded in the cast process
        Vector2 start = transform.position;
        //Position to move MovingObject to using vector addition
        Vector2 end = start + new Vector2(xDir, yDir);

        //Prevent MovingObject from colliding with its own BoxCollider2D
        //when casting a line to the target position
        boxCollider.enabled = false;
        //Cast a line from start to end and check whether there was a collision
        hit = Physics2D.Linecast(start, end, blockingLayer);
        //After the line is cast, re-enable MovingObject's BoxCollider2D
        boxCollider.enabled = true; 

        //Conditional construct to determine whether the object collided
        //with anything on its trajectory
        if(hit.transform == null) //Did not register a position it hit
        {
            //Given collision did not occur, begin movement of MovingObject
            StartCoroutine(SmoothMovement(end));
            //Successfully moved object to target position, end
            return true;
        }

        //If target position would result in a collision preventing occupying
        //it, then return false and SmoothMovement is not called, thus failing
        //to initiate or actuate any movement of MovingObject
        return false;
    }
    protected IEnumerator SmoothMovement (Vector3 end)
    {   //Calculating Squared distances is relatively cheaper than square roots
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {   //Calculate a new position based on the MovingObject's current position
            // and its target position. The new position is the third argument away
            //from the current position unless it is larger than the end position
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            //Set MovingObject's position to the position calculated above
            rb2D.MovePosition(newPosition);
            //Recalculate the square distance between the MovingObject's current
            //position and its target position:transform is the MovingObject's
            //component containing information about its position
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    //The virtual keyword means AttemptMove can be overridden by inheriting classes using the override keyword.
    //AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        //Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D hit;

        //Set canMove to true if Move was successful, false if failed.
        bool canMove = Move(xDir, yDir, out hit);

        //Check if nothing was hit by linecast
        if (hit.transform == null)
            //If nothing was hit, return and don't execute further code.
            return;

        //Get a component reference to the component of type T attached to the object that was hit
        T hitComponent = hit.transform.GetComponent<T>();

        //If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
        if (!canMove && hitComponent != null)

            //Call the OnCantMove function and pass it hitComponent as a parameter.
            OnCantMove(hitComponent);
    }

    //Must be overriden by the inheriting class
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
