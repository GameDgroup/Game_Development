using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite; //Sprite to display indicating player has successfully attacked wall
    public int hp = 4; //Wall's hp

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    //Public access specified allows classes other than this one to access
    //this method. Perhaps, the player GameObject will have a reference to
    //this class and will be able to call this method with a particular
    //argument for loss.
	public void DamageWall(int loss)
    {
        //Display the sprite designated for the damage effect
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;

        //Presumably, setting the gameObject's pertaining attribute to 
        //false, removes it from the scene
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
