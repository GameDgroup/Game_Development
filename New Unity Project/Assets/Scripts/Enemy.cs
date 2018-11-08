using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDamage;
    public int badGuyHealth;
    public float speed;
    public int playerFood;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    private Player foodCheck;

	// Use this for initialization
	protected override void Start ()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        foodCheck = new Player();
        base.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        playerFood = foodCheck.getFoodPoints();

        if (Vector2.Distance(transform.position, target.position) > 1 && Vector2.Distance(transform.position, target.position) < 4 &&
            playerFood < 120)
        {
            seek();
        }
        else if (Vector2.Distance(transform.position, target.position) < 1 && playerFood < 120)
        {
            attack();
        }
        else if (Vector2.Distance(transform.position, target.position) > 1 && Vector2.Distance(transform.position, target.position) < 4 &&
                 playerFood >= 120)
        {
            flee();
        }
        
	}

    private void seek()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position,
                                                     speed * Time.deltaTime);
    }

    private void attack()
    {
        Player hitPlayer = new Player();

        animator.SetTrigger("EnemyAttack");

        hitPlayer.LoseFood(playerDamage);
    }

    private void flee()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, -1 * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        badGuyHealth = badGuyHealth - damage;

        if (badGuyHealth <= 0)
            Destroy(gameObject);
    }

    /*protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }*/

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        //AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        animator.SetTrigger("EnemyAttack");

        hitPlayer.LoseFood(playerDamage);
    }
}
