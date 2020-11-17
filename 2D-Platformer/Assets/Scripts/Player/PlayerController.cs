using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask platform;
    public LayerMask enemies;
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform attackPoint;

    public float movementSpeed = 20.0f;
    public float wallSlidingSpeed = 1.0f;

    public int jumpAmount = 2;
    public float jumpForce = 40.0f;
    public float wallJumpForce = 50.0f;
    public float longJumpDuration = 1.2f;

    public float dashPower = 16.5f;
    public float dashDuration = 4.0f;

    public int attackDamage = 1;
    public float attackRange = 0.5f;

    public float m_CheckRadius = 0.05f;


    private Rigidbody2D m_Rb;
    private PlayerAnimator m_PlayerAnimator;

    private float m_DeltaTimeScale = 10.0f;

    private float m_HorizontalInput;

    private bool m_IsGrounded;
    private bool m_IsTouchingWall;
    private bool m_IsWallSliding;

    private bool m_IsJumping;
    private bool m_IsLongJumping;
    private bool m_isWallJumping;
    private bool m_IsDashing;

    private int m_JumpsAvailable;
    private float m_LongJumpTimer;

    private bool m_DashAvailable;
    private float m_dashTimer;


    private void Start()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_PlayerAnimator = GetComponent<PlayerAnimator>();
    }

    /** get input for player and set bools to determine wether player is idle, walking, jumping, wallsliding, dashing or attacking,
     *  so that FixedUpdate() can calculate and set correct movement of player
     */
    private void Update()
    {
        m_HorizontalInput = Input.GetAxisRaw("Horizontal");
        m_IsGrounded = Physics2D.OverlapCircle(groundCheck.position, m_CheckRadius, platform);
        m_IsTouchingWall = Physics2D.OverlapCircle(wallCheck.position, m_CheckRadius, platform);
        m_IsWallSliding = m_IsTouchingWall && !m_IsGrounded && m_HorizontalInput != 0 ? true : false;

        m_PlayerAnimator.flipPlayerY(m_HorizontalInput);

        if (m_IsGrounded)
        {
            m_PlayerAnimator.animatePlayerIdleOrWalking(m_HorizontalInput);
        }
        else
        {
            m_PlayerAnimator.animateJumpingOrFalling(m_Rb.velocity.y > 0);
        }

        //reset jumps and dash when player is on ground or touching wall
        if (m_IsGrounded || m_IsTouchingWall)
        {
            m_JumpsAvailable = jumpAmount;
            m_DashAvailable = true;
        }
       

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W) || Input.GetKeyUp(KeyCode.W))
        {
            playerJump();
        }
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerDash();
            m_PlayerAnimator.animatePlayerDash();
        }
        */
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAttack();
            m_PlayerAnimator.animatePlayerAttack();
        }
    }


    //calculates actual movement of player based on set bools in update function
    private void FixedUpdate()
    {
        float velocityY = m_Rb.velocity.y;

        //calculate y velocity when player is (long) jumping
        if (m_IsJumping || m_IsLongJumping)
        {
            velocityY = jumpForce * Time.deltaTime * m_DeltaTimeScale;
            if (m_IsJumping)
            {
                m_JumpsAvailable--;
                m_IsJumping = false;
            }
            if (m_IsLongJumping)
            {
                m_LongJumpTimer -= Time.deltaTime * m_DeltaTimeScale;
            }
        }

        //calculate y velocity when player is wall sliding or wall jumping
        if (m_IsWallSliding)
        {
            if (m_isWallJumping)
            {
                velocityY = wallJumpForce * Time.deltaTime * m_DeltaTimeScale;
                m_isWallJumping = false;
            }
            else
            {
                velocityY = Mathf.Clamp(m_Rb.velocity.y, -wallSlidingSpeed, float.MaxValue);
            }
        }

        /*
        //add dash force to horizontal input for increased x velocity when player is dashing
        if (m_IsDashing)
        {
            if (m_PlayerAnimator.facingRight)
            {
                m_HorizontalInput += dashPower;
            }
            else
            {
                m_HorizontalInput -= dashPower;
            }
            m_IsDashing = false;
        }
        m_dashTimer -= Time.deltaTime * m_DeltaTimeScale;   //used for dash cooldown
        */

        m_Rb.velocity = new Vector2(m_HorizontalInput * movementSpeed * Time.deltaTime * m_DeltaTimeScale, velocityY);
    }

    //player (long) jumps if available
    private void playerJump()
    {
        //player jumps on button press if available
        if (Input.GetKeyDown(KeyCode.W) && m_JumpsAvailable > 0)
        {
            m_IsJumping = true;
            m_LongJumpTimer = longJumpDuration;
            m_isWallJumping = m_IsWallSliding;
        }

        //player jumps higher on first jump if button is pressed for longer
        if (Input.GetKey(KeyCode.W) && m_JumpsAvailable == jumpAmount - 1 && m_LongJumpTimer > 0)
        {
            m_IsLongJumping = true;
            m_LongJumpTimer -= Time.deltaTime;
        }
        else
        {
            m_IsLongJumping = false;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            m_IsLongJumping = false;
        }
    }


    //player dashes if available and cooldown time expired
    private void playerDash()
    {
        if (m_DashAvailable && m_dashTimer < 0)
        {
            m_IsDashing = true;
            m_DashAvailable = false;
            m_dashTimer = dashDuration;
        }
    }


    //all enemies inside attack range receive damage based on set player attack damage
    private void playerAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemies);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().receiveDamage(attackDamage);
        }
    }


    //visualize groundcheck, wallcheck and attack point in unity editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, m_CheckRadius);
        Gizmos.DrawWireSphere(wallCheck.position, m_CheckRadius);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
