using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExodo : EnemyCore {

    public int action;
    // Use this for initialization

    // Update is called once per frame

    public void Awake()
    {
        enemyBehavior();
        timeBetweenShots = 1f;
    }


    //IA Enemy
    public override void moveTowardsPlayer()
    {
        if (player != null && !characterState.Equals(EnemyStatus.dying))
        {

            if (Vector2.Distance(transform.position, player.position) > stopDistance)
            {

                resetAnimations();
                action = Random.Range(0, 3);
                Debug.Log("action:" + action);
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                animator.SetBool("isRunning", true);
                if (animator.GetBool("isShooting"))
                {
                    animator.SetBool("isShooting", false);
                }
            }
            else
            {
                animator.SetBool("isRunning", false);

                switch (action)
                {
                    case 0:
                        animator.SetBool("isJumping", true);
                        action = Random.Range(0, 3);
                        break;
                    case 1:
                        animator.SetBool("isShootingDown", true);
                        timeBetweenShots = 0.1f;
                        Shoot();
                        break;
                    case 2:
                        animator.SetBool("isAlert", true);
                        timeBetweenShots = 1f;
                        Shoot(true);
                        break;

                }
                if (Time.time >= attackTime && characterState != EnemyStatus.dying && isArmedEnum.Equals(Armed.yes))
                {
                    
                    //  StartCoroutine(Attack());
                    //  attackTime = Time.time + timeBetweenAttack;
                }
            }
        }
    }

    public void resetAnimations()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isShootingDown", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isAlert", false);
    }

    public override void enemyBehavior()
    {
        iaBehavior = IABehavior.follow;
    }
}
