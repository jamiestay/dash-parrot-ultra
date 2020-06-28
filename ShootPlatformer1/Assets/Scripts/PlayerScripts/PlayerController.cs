using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]

    public float speed;
    public float jumpForce;
    private float moveInput;

    private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;
    private TrailRenderer tr;

    [Header("Ground Check")]

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    

    [Header("Player Jump")]
    public int extraJumps;
    public int extraJumpValue;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    private float jumpAllowTimer;
    public float jumpAllowTimerValue;
    private float groundedAllowTimer;
    public float groundedAllowTimerValue;

    [Header("Player Effects")]
    public GameObject jumpEffect;
    private bool spawnDust;
    public GameObject trailEffect;
    public float startTimeBtwTrail;
    private float timeBtwTrail;
    public CameraShake camShake;
    private AudioSource source;
    public AudioClip landingSound;
    public AudioClip upSound;
    public AudioClip dashSound;

    [Header("Wall Sliding")]
    public LayerMask whatIsWall;
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.05f;
    bool isWallSliding = false;
    RaycastHit2D wallCheckHit;
    public float jumpTimer;
    public Transform wallCheck;
    private bool isWalled = false;
    public float wallCheckRadius;
    public string wallStr;
    private bool wallCollisionCheck;
    public float wallBufferValue;
    private float wallBuffer;


    [Header("Attack and Health")]
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemy;
    public string enemyTag;
    public HealthBar hp;
    public float maxHealth = 25;
    private float currentHealth;
    





    void Start()
    {
        hp.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        dashTime = startDashTime;
        extraJumps = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;
        tr.Clear();
        wallCollisionCheck = false;
    }

    void Update()
    {

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }



        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpAllowTimer = jumpAllowTimerValue;
        }


            jumpAllowTimer -= Time.deltaTime;

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            //sr.flipX = false;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            //sr.flipX = true;
        }

        if (moveInput == 0)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            if (timeBtwTrail <= 0)
            {
                Instantiate(trailEffect, groundCheck.position, Quaternion.identity);
                timeBtwTrail = startTimeBtwTrail;
            }
            else
            {
                timeBtwTrail -= Time.deltaTime;
            }
            animator.SetBool("isRunning", true);
        }

        if (isGrounded == true)
        {
            
            groundedAllowTimer = groundedAllowTimerValue;
            if (spawnDust == true)
            {
                source.clip = landingSound;
                source.Play();
                Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
                spawnDust = false;
            }
            extraJumps = extraJumpValue;
            animator.SetBool("isJumping", false);
        }
        else
        {
            spawnDust = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.W) && extraJumps > 0 && groundedAllowTimer <= 0 || Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0 && groundedAllowTimer <= 0)
        {
            source.clip = upSound;
            source.Play();
            camShake.Shake();
            if (isGrounded == true)
            {
                Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            }
            animator.SetTrigger("takeOff");
            rb.velocity = Vector2.up * jumpForce;
            jumpTimeCounter = jumpTime;
            isJumping = true;
            extraJumps--;
        }

        else if ((jumpAllowTimer > 0) && groundedAllowTimer > 0)
        {
            source.clip = upSound;
            source.Play();
            camShake.Shake();
            if (isGrounded == true)
            {
                Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            }
            else
            {
                extraJumps--;
            }
            animator.SetTrigger("takeOff");
            rb.velocity = Vector2.up * jumpForce;
            jumpTimeCounter = jumpTime;
            isJumping = true;
            jumpAllowTimer = 0;
            groundedAllowTimer = 0;
            

        }

       
        else if (isWallSliding && Input.GetKeyDown(KeyCode.W) && wallCollisionCheck == true || isWallSliding && Input.GetKeyDown(KeyCode.UpArrow) && wallCollisionCheck == true)
        {
            source.clip = upSound;
            source.Play();
            camShake.Shake();
            Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            animator.SetTrigger("takeOff");
            rb.velocity = Vector2.up * jumpForce;
            jumpTimeCounter = jumpTime;
            isJumping = true;
            wallCollisionCheck = false;

        }


            if (Input.GetKey(KeyCode.W) && isJumping == true || Input.GetKey(KeyCode.UpArrow) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }

        if(isWalled == true)
        {
            animator.SetBool("isWalled", true);
        }
        else
        {
            animator.SetBool("isWalled", false);
        }


        if(direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && extraJumps > 0 && isWalled == false)
            {
                if(moveInput < 0)
                {
                    tr.enabled = true;
                    extraJumps--;
                    direction = 1;
                    source.clip = dashSound;
                    source.Play();
                    animator.SetTrigger("dash");

                }
                else if(moveInput > 0)
                {
                    tr.enabled = true;
                    extraJumps--;
                    direction = 2;
                    source.clip = dashSound;
                    source.Play();
                    animator.SetTrigger("dash");

                }
            }
        }
        else
        {
            if(dashTime <= 0)
            {
                tr.Clear();
                tr.enabled = false;
                
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                Time.timeScale = 1;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if(direction == 1)
                {
                    camShake.Shake();
                    Time.timeScale = 1f;
                    rb.velocity = Vector2.left * dashSpeed;
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(Random.Range(1, 5));
                    }
                }
                else if(direction == 2)
                {
                    camShake.Shake();
                    Time.timeScale = 1f;
                    rb.velocity = Vector2.right * dashSpeed;
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(1);
                    }
                }
            }
        }


    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isWalled = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
     

        

        moveInput = Input.GetAxisRaw("Horizontal");

        if (direction == 0)
        {


            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }


        if (transform.localScale.x == 1)
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, whatIsWall);
            Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.blue);
        }
        else
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, whatIsWall);
            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.blue);
        }

        if(wallCheckHit && isWalled && moveInput != 0 || wallBuffer > 0 && isWalled && moveInput != 0)
        {
            wallBuffer = wallBufferValue;
            isWalled = true;
            isWallSliding = true;
            jumpTimer = Time.time + wallJumpTime;
        }
        else if (jumpTimer < Time.time)
        {
            isWalled = false;
            isWallSliding = false;
            wallBuffer -= Time.deltaTime;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue));
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == wallStr)
        {
            wallCollisionCheck = true;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.tag == enemyTag)
        {
            if(direction == 0)
            {
                takeDamage(0.1f);
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    void takeDamage(float damage)
    {
        currentHealth -= damage;
        hp.SetHealth(currentHealth);
    }
}
