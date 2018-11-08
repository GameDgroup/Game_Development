using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{

    public Player movement;

    private void OnCollisionEnter(Collider2D collision)
    {
        if (collision.tag == "Outer_Wall")
        {
            //Debug.Log("Hit " + collision.collider.name);

            movement.enabled = false;

            

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
