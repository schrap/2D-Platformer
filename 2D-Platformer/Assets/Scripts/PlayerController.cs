using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask platform;
    public Transform groundCheck;
    public Transform wallCheck;

    public float movementSpeed;
    public float wallSlidingSpeed;

    public int jumpAmount;
    public float jumpForce;
    public float wallJumpForce;
    public float jumpDuration;


    private Rigidbody2D m_Rb;
    private Animator m_Animator;
    private float m_HorizontalInput;

    private bool m_FacingRight;
    private bool m_IsGrounded;
    private bool m_IsJumping;
    private bool m_IsLongJumping;
    private bool m_isWallJumping;
    private bool m_IsTouchingWall;
    private bool m_IsWallSliding;

    private int m_JumpsAvailable;
    private float m_JumpTimer;

    private float m_CheckRadius = 0.001f;

    private float m_DeltaTimeScale = 10.0f;


    private void Start()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_FacingRight = true;
    }

    private void Update()
    {
        m_HorizontalInput = Input.GetAxisRaw("Horizontal");

        m_IsGrounded = Physics2D.OverlapCircle(groundCheck.position, m_CheckRadius, platform);
        m_IsTouchingWall = Physics2D.OverlapCircle(wallCheck.position, m_CheckRadius, platform);

        if (m_HorizontalInput != 0)
        {
            m_Animator.SetBool("isWalking", true);
        } else
        {
            m_Animator.SetBool("isWalking", false);
        }

        //flip the character to face correct direction
        if ((m_HorizontalInput > 0 && !m_FacingRight) || (m_HorizontalInput < 0 && m_FacingRight))
        {
            transform.Rotate(0, 180, 0);
            m_FacingRight = !m_FacingRight;
        }

        //reset jumps when player is on ground or touching wall
        if (m_IsGrounded || m_IsTouchingWall)
        {
            m_JumpsAvailable = jumpAmount;
        }

        //slide player down wall when moving into it
        if (m_IsTouchingWall && !m_IsGrounded && m_HorizontalInput != 0)
        {
            m_IsWallSliding = true;
        }
        else
        {
            m_IsWallSliding = false;
        }

        //player jumps on button press if available
        if (Input.GetKeyDown(KeyCode.W) && m_JumpsAvailable > 0)
        {
            m_IsJumping = true;
            m_JumpTimer = jumpDuration;

            if (m_IsWallSliding)
            {
                m_isWallJumping = true;
            } else
            {
                m_isWallJumping = false;
            }
        }


        //player jumps higher on first jump if button is pressed for longer
        if (Input.GetKey(KeyCode.W) && m_JumpsAvailable == jumpAmount - 1)
        {
            if (m_JumpTimer > 0)
            {
                m_JumpTimer -= Time.deltaTime;
                m_IsLongJumping = true;
            }
            else
            {
                m_IsLongJumping = false;
            }
        }
        else
        {
            m_IsLongJumping = false;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            m_IsLongJumping = false;
            m_isWallJumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (m_IsJumping)
        {
            m_Rb.velocity = Vector2.up * jumpForce * Time.deltaTime * m_DeltaTimeScale;
            m_JumpsAvailable--;
            m_IsJumping = false;
        }

        if (m_IsLongJumping)
        {
            m_Rb.velocity = Vector2.up * jumpForce * Time.deltaTime * m_DeltaTimeScale;
            m_JumpTimer -= Time.deltaTime * m_DeltaTimeScale;
        }

        if (m_IsWallSliding && !m_isWallJumping)
        {
            m_Rb.velocity = new Vector2(m_Rb.velocity.x, Mathf.Clamp(m_Rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        } else if (m_IsWallSliding && m_isWallJumping)
        {
            m_Rb.velocity = Vector2.up * wallJumpForce * Time.deltaTime * m_DeltaTimeScale;
            m_isWallJumping = false;
        }

        m_Rb.velocity = new Vector2(m_HorizontalInput * movementSpeed * Time.deltaTime * m_DeltaTimeScale, m_Rb.velocity.y);
    }
}
