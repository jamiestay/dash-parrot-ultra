using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerAI : MonoBehaviour
{
    public float speed;
    public GameObject player;
    private Vector2 ee;
    public float followRangeX;
    public float followRangeY;
    public LayerMask whatIsPlayer;
    public float stopDistance;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private Vector2 direction;

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        animator.SetBool("isRun", false);

        Collider2D[] followingPlayer = Physics2D.OverlapBoxAll(transform.position, new Vector2(followRangeX,followRangeY), 0, whatIsPlayer);
        for (int i = 0; i < followingPlayer.Length; i++)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > stopDistance)
            {
                if(isGrounded == true)
                {
                    direction = (player.transform.position - transform.position).normalized;
                    if(direction.x >= 0.5)
                    {
                        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    }
                    ee = new Vector2(player.transform.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, ee, speed * Time.deltaTime);
                    animator.SetBool("isRun", true);
                    animator.SetBool("isAttack", false);
                }

            }
            else
            {
                animator.SetBool("isRun", false);
                animator.SetBool("isAttack", true);
            }

        }

        return;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(followRangeX, followRangeY, 1));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }


}
