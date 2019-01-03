using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterCore : MonoBehaviour {

    [Header("Character Informations")]
    public float life;
    [SerializeField]
    private float speed;
    public float jumpForce;
    public enum characterStatus { idle, running, dying, jumping, shooting };
    public characterStatus characterState;
    [SerializeField]
    public bool faceRight;
    private bool isGrunded;
    private bool jump;
    [SerializeField]
    private int numberJumps;
    [SerializeField]
    private int maxJumps;

    [Header("Gun Properties")]
 
    public GunType gunType;
    public GameObject projectile;
    public Transform shotPoint;
    public float timeBetweenShots;
    private float shotTime;
    public Transform gunPositionStart;
    public Transform gunPositionEnd;
    public enum GunType { pistol, automatic };

    [Header("Animation Properties")]
    private Animator animator;

    private Rigidbody2D characterRigidBody;

    public int eggCoin;
    public int egg;
    public Text lifeText;
    public Text eggText;
    public Text eggCoinText;
    bool isMobile = false;

    // Use this for initialization
    void Start () {

        //Creating rigidBody2D Component
        gameObject.AddComponent<Rigidbody2D>();

        //Attributing
        characterRigidBody = gameObject.GetComponent<Rigidbody2D>();
        characterRigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        animator = GetComponent<Animator>();
        faceRight = false;
        isGrunded = true;
        jump = false;
        characterRigidBody.freezeRotation = true;
        timeBetweenShots = 0.1f;
        gunType = GunType.pistol;
      //  lifeText.text = life.ToString();
        egg = 0;
        eggCoin = 0;
      //  eggText.text = egg.ToString();
       // eggCoinText.text = eggCoin.ToString();
    }

    // Update is called once per frame
    void Update () {
        //shoot line
		  Debug.DrawLine(gunPositionStart.position, gunPositionEnd.position, Color.green);
	}

    private void FixedUpdate()
    {
     //   MoveCharacter();
        Jump();
        invokeShoot();
        MoveCharacterWithVelocity();
        Flip();
    }


    //MOVIMENT FUNCTIONS
    public void MoveCharacter()
    {
        characterState = characterStatus.running;
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        Vector2 moveAmount = moveInput.normalized * speed;
        characterRigidBody.MovePosition(characterRigidBody.position + moveAmount * Time.deltaTime);
    }

    public void MoveCharacterWithVelocity()
    {
       
        if(isMobile)
        {   
            if (CrossPlatformInputManager.GetAxis("Horizontal") > 0 || CrossPlatformInputManager.GetAxis("Horizontal") < 0)
            {
                characterRigidBody.velocity = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal") * speed, characterRigidBody.velocity.y);
                //stateMachine(characterStatus.running);
                animator.SetBool("isRunning", true);
            }
            else
            {
                // stateMachine(characterStatus.idle);
                animator.SetBool("isRunning", false);
            }
        }
        else
        {

            if(Input.GetAxis("Horizontal") > 0  || Input.GetAxis("Horizontal") < 0)
            {
                characterRigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * speed,characterRigidBody.velocity.y);
                //stateMachine(characterStatus.running);
                animator.SetBool("isRunning", true);
            }
            else
            {
                // stateMachine(characterStatus.idle);
                animator.SetBool("isRunning", false);
            }
        }
    }

    public void stateMachine(characterStatus status)
    {
        characterState = status;
        switch (characterState)
        {
            case characterStatus.running:Shoot();
                Shoot();
                break;
            case characterStatus.shooting:
                Shoot();
                break;
            case characterStatus.idle:
                animator.SetBool("isRunning", false);
                animator.SetBool("isShooting", false);
                break;
 
        }
    }

    //FUNCTIONS

    public void Flip()
    {
        if(isMobile) 
        {
            if ((CrossPlatformInputManager.GetAxis("Horizontal") > 0 && !faceRight || CrossPlatformInputManager.GetAxis("Horizontal") < 0 && faceRight) && characterState != characterStatus.dying)
            {
                Debug.Log("Virou");
                faceRight = !faceRight;
                Vector2 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
        else
        {
            if((Input.GetAxis("Horizontal") > 0 && !faceRight || Input.GetAxis("Horizontal") < 0 && faceRight) && characterState != characterStatus.dying)
            {
                Debug.Log("Virou");
                faceRight = !faceRight;
                Vector2 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }
        
    public void Jump()
    {
        if (isGrunded)
        {
            animator.SetBool("isJumping", false);
            numberJumps = 0;
        }
        if (isMobile)
        {
            bool jumpPressed = CrossPlatformInputManager.GetButton("jumpButton");

            Debug.Log("Jump Pressed:"+ jumpPressed);
  
 
            if (jumpPressed && numberJumps < maxJumps)
            {
                Debug.Log("Jump");
                // animator.SetFloat("velocidadeY", characterRigidBody.velocity.y);
                animator.SetBool("isJumping", true);
                characterRigidBody.AddForce(new Vector2(0f, jumpForce));
                numberJumps++;
                isGrunded = false;
            }
        }
        else
        {
   

            if (Input.GetKeyDown(KeyCode.Space) && numberJumps < maxJumps)
            {
                Debug.Log("Jump");
                // animator.SetFloat("velocidadeY", characterRigidBody.velocity.y);
                animator.SetBool("isJumping", true);
                characterRigidBody.AddForce(new Vector2(0f, jumpForce));
                numberJumps++;
                isGrunded = false;
            }
        }
       
        jump = false;
       
    }


    public void invokeShoot()
    {
        Invoke("Shoot", 1f);
    }
    
    public void Shoot()
    {
        if(isMobile)
        {
            bool shootingPressed = CrossPlatformInputManager.GetButton("shootButton");
            if (shootingPressed)
            {
                if(projectile == null)
                {
                    throw new System.ArgumentException("Projectile is null", "projectile");
                }
                else if (gunPositionStart == null || gunPositionEnd == null)
                {
                    throw new System.ArgumentException("GameObject ShotPoint is null", "ShotPoint");
                }
                Debug.Log("Shoot");
                Vector2 target = new Vector2(transform.position.x * 2, transform.position.y);
          

            
                if (Time.time >= shotTime && !animator.GetBool("isRunning"))
                {
                    if(gunType.Equals(GunType.pistol))
                    {
                        animator.SetBool("isShooting", true);
                    }
                    else
                    {
                        animator.SetBool("getAutomaticGun", true);
                    }
                    Projectile bullet = projectile.GetComponent<Projectile>();
                    if (bullet == null)
                    {
                        throw new System.ArgumentException("Projectile script is missing on bullet object", "projectile");
                    }
                    //stateMachine(characterStatus.shooting);
                    bullet.instanceDirection = faceRight;
                    Instantiate(projectile, shotPoint.position, transform.rotation);
                    shotTime = Time.time + timeBetweenShots;
                }
            
            }
            else
            {
                if (gunType.Equals(GunType.pistol))
                {
                    animator.SetBool("isShooting", false);
                }
                else if(gunType.Equals(GunType.automatic))
                {
                    animator.SetBool("getAutomaticGun", false);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
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



                if (Time.time >= shotTime && !animator.GetBool("isRunning"))
                {
                    if (gunType.Equals(GunType.pistol))
                    {
                        animator.SetBool("isShooting", true);
                    }
                    else
                    {
                        animator.SetBool("getAutomaticGun", true);
                    }
                    Projectile bullet = projectile.GetComponent<Projectile>();
                    if (bullet == null)
                    {
                        throw new System.ArgumentException("Projectile script is missing on bullet object", "projectile");
                    }
                    //stateMachine(characterStatus.shooting);
                    bullet.instanceDirection = faceRight;
                    Instantiate(projectile, shotPoint.position, transform.rotation);
                    shotTime = Time.time + timeBetweenShots;
                }

            }
            else
            {
                if (gunType.Equals(GunType.pistol))
                {
                    animator.SetBool("isShooting", false);
                }
                else if (gunType.Equals(GunType.automatic))
                {
                    animator.SetBool("getAutomaticGun", false);
                }
            }
        }
    }

    IEnumerator CallStateMachine(characterStatus status)
    {
        yield return new WaitForSeconds(5f);

        stateMachine(characterStatus.idle); 
    }

    public void Die()
    {
        Debug.Log("Die");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Ground"))
        {
            Debug.Log("is Grounded");
            isGrunded = true;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log("dano");
        life -= damageAmount;
        lifeText.text = life.ToString();
        if (life <= 0)
        {
            characterState = characterStatus.dying;
            Destroy(gameObject);
        }
    }


    ///Get and Setters
    ///
    public GunType getGunTypeAutomatic()
    {
        return GunType.automatic;
    }

    public GunType getGunTypePistol()
    {
        return GunType.pistol;
    }
}
