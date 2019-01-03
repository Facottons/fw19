using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour {

    [Header("Enemy Informations")]
    public float life;
    [SerializeField]
    protected float speed;
    public float jumpForce;
    public enum EnemyStatus { idle, running, dying, jumping, shooting , patrol };
    public EnemyStatus characterState;
    [SerializeField]
    private bool faceRight;
    private bool isGrunded;
    private bool jump;
    [SerializeField]
    private int numberJumps;
    [SerializeField]
    private int maxJumps;
    public Transform player;

    [Header("IA Informations")]
    public float stopDistance;
    protected float attackTime;
    public float attackSpeed;
    public float timeBetweenAttack;
    public enum IABehavior {follow, followOnSight };
    public IABehavior iaBehavior;

    [Header("Gun Properties")]
    
    public Armed isArmedEnum;
    public GameObject projectile;
    public GameObject rocket;
    public Transform shotPoint;
    public float timeBetweenShots;
    private float shotTime;
    public Transform gunPositionStartUp;
    public Transform gunPositionStart;
    public Transform gunPositionEnd;
    public enum Armed { yes, no };


    [Header("Animation Properties")]
    protected Animator animator;

    private Rigidbody2D characterRigidBody;

    [Header("Pickups")]
    public int pickupChance;
    public GameObject [] pickups;


    // Use this for initialization
    public virtual void Start()
    {

        Debug.Log("find player");
        isArmedEnum = Armed.yes;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Creating rigidBody2D Component
        gameObject.AddComponent<Rigidbody2D>();
        //Attributing
        characterRigidBody = gameObject.GetComponent<Rigidbody2D>();
        characterRigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        animator = GetComponent<Animator>();
        faceRight = true;
        isGrunded = true;
        jump = false;
        characterRigidBody.freezeRotation = true;
        
        stopDistance = Random.Range(2, stopDistance);

    }
	
	// Update is called once per frame
	void Update () {
        
        // Debug.DrawLine(gunPositionStart.position, gunPositionEnd.position, Color.green);
        if(player != null)
        {
            if(iaBehavior.Equals(IABehavior.follow))
            {
                gunPositionEnd.transform.position = new  Vector2(player.transform.position.x,gunPositionEnd.transform.position.y);
            }
            Debug.DrawLine(gunPositionStart.position, gunPositionEnd.position, Color.red);
            Flip();
        }
        else
        {
            animator.SetBool("noFoundPlayer", true);

        }
        if(characterState.Equals( EnemyStatus.dying))
        {
         //   animator.SetBool("isDying", false);
        }
    }

    public virtual void FixedUpdate()
    {
        moveTowardsPlayer();
    }

    public virtual void enemyBehavior()
    {
        iaBehavior = IABehavior.follow;
    }

    public void stateMachine(EnemyStatus status)
    {
        characterState = status;
        switch (characterState)
        {
            case EnemyStatus.running:
                Shoot();
                Shoot();
                break;
            case EnemyStatus.shooting:
                Shoot();
                break;
            case EnemyStatus.idle:
                animator.SetBool("isRunning", false);
                animator.SetBool("isShooting", false);
                break;

        }
    }


    //FUNCTIONS

    public void Flip()
    {
        bool playerDirection = player.GetComponent<CharacterCore>().faceRight;
        if ((gunPositionEnd.transform.position.x+1)  > gunPositionStart.transform.position.x && characterState != EnemyStatus.dying)
        {
            faceRight = true;
            Vector2 scale = transform.localScale;
            if (scale.x > 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
        else if((gunPositionEnd.transform.position.x+1)  < gunPositionStart.transform.position.x && characterState != EnemyStatus.dying)
        {
            faceRight = false;
            Vector2 scale = transform.localScale;
            if(scale.x < 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

         
    }

    public void Jump()
    {

        if (isGrunded)
        {
            numberJumps = 0;
        }
        if(isGrunded)
        {
            Debug.Log("Jump");
            characterRigidBody.AddForce(new Vector2(0f, jumpForce));
            numberJumps++;
            isGrunded = false;
        }

        jump = false;
    }
    public void freezeJump()
    {
        animator.StopPlayback();
    }


    public void Shoot()
    {
            if (projectile == null )
            {
                throw new System.ArgumentException("Projectile is null", "projectile");
            }
            else if (gunPositionStart == null || gunPositionEnd == null)
            {
                throw new System.ArgumentException("GameObject ShotPoint is null", "ShotPoint");
            }
            Debug.Log("Shoot");
            Vector2 target = new Vector2(transform.position.x * 2, transform.position.y);

            
            if (Time.time >= shotTime )
            {
                Projectile bullet = projectile.GetComponent<Projectile>();
                if (bullet == null)
                {
                    throw new System.ArgumentException("Projectile script is missing on bullet object", "projectile");
                }
                //stateMachine(EnemyStatus.shooting);
                animator.SetBool("isShooting", true);
                bullet.instanceDirection = faceRight;
                Instantiate(projectile, gunPositionStart.position, transform.rotation);
                shotTime = Time.time + timeBetweenShots;
            }
 
    }

    public void Shoot(bool isShootingDown)
    {
        if (projectile == null)
        {
            throw new System.ArgumentException("Projectile is null", "projectile");
        }
        else if (gunPositionStart == null || gunPositionEnd == null)
        {
            throw new System.ArgumentException("GameObject ShotPoint is null", "ShotPoint");
        }
        Debug.Log("Shoot");
        Vector2 target = new Vector2(transform.position.x * 2, transform.position.y);


        if (Time.time >= shotTime)
        {
            Projectile bullet = projectile.GetComponent<Projectile>();
            if (bullet == null)
            {
                throw new System.ArgumentException("Projectile script is missing on bullet object", "projectile");
            }
            //stateMachine(EnemyStatus.shooting);
            animator.SetBool("isShooting", true);
            bullet.instanceDirection = faceRight;
            if(isShootingDown)
            {
                Projectile rocket1 = rocket.GetComponent<Projectile>();
                rocket1.instanceDirection = faceRight;
                Instantiate(rocket, gunPositionStartUp.position, transform.rotation);
            }
            else
            {
                Instantiate(projectile, gunPositionStart.position, transform.rotation);
            }
            shotTime = Time.time + timeBetweenShots;
        }

    }

    public void Die()
    {
        Debug.Log("Die");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            Debug.Log("is Grounded");
            isGrunded = true;
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        life -= damageAmount;
        Debug.Log("dano:"+life);
        if (life <= 0 && characterState != EnemyStatus.dying)
        {
            int randomNumber = Random.Range(1,101);
            if(randomNumber < pickupChance)
            {
                GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
                Instantiate(randomPickup,transform.position,transform.rotation);
                Debug.Log("Instantiate pickup");
            }
            characterState = EnemyStatus.dying;
            animator.SetBool("isDying", true);

            StartCoroutine("DestroyEnemyAfterSeconds",2f);
        }
        else
        {
            animator.SetBool("isDying", false);
        }
    }

    public virtual IEnumerator DestroyEnemyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);

    }

    //IA Enemy
    public virtual void moveTowardsPlayer()
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) > stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                animator.SetBool("isRunning", true);
                if(animator.GetBool("isShooting"))
                {
                    animator.SetBool("isShooting", false);
                }
            }
            else
            {
                animator.SetBool("isRunning", false);
                if (Time.time >= attackTime && characterState != EnemyStatus.dying && isArmedEnum.Equals(Armed.yes))
                {
                    Shoot();
                  //  StartCoroutine(Attack());
                  //  attackTime = Time.time + timeBetweenAttack;
                }
            }
        }
    }


}
