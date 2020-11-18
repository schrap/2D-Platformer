using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public Color damageColor = new Color(0.78f, 0.23f, 0.23f);
    public float damageAnimationTime = 0.3f;

    public Transform attackTrail;
    public float attackAnimationSpeed;

    private Animator m_Animator;
    private SpriteRenderer m_PlayerSpriteRenderer;

    private bool m_FacingRight;
    public bool facingRight
    {
        get { return m_FacingRight; }
    }

    private float m_DamageTimer;
    private bool m_Attacking;

    private float m_Angle;
    private Vector2 currentAttackPoint;
    private bool attackDirectionRight;


    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_PlayerSpriteRenderer = GetComponent<SpriteRenderer>();
        m_FacingRight = true;
    }


    private void Update()
    {
        m_DamageTimer -= Time.deltaTime;
        if (m_DamageTimer < 0)
        {
            animateDamage(false);
        }

        if (m_Attacking)
        {
            if (attackDirectionRight)
            {
                attackTrail.position = new Vector2(currentAttackPoint.x + 0.5f * Mathf.Cos(m_Angle), currentAttackPoint.y + 0.5f * Mathf.Sin(m_Angle));
                m_Angle -= Mathf.PI / 4 * Time.deltaTime * attackAnimationSpeed;

                if (m_Angle < -Mathf.PI / 2)
                {
                    m_Attacking = false;
                    attackTrail.gameObject.SetActive(false);
                }
            }
            else
            {
                attackTrail.position = new Vector2(currentAttackPoint.x + 0.5f * Mathf.Cos(m_Angle), currentAttackPoint.y + 0.5f * Mathf.Sin(m_Angle));
                m_Angle += Mathf.PI / 4 * Time.deltaTime * attackAnimationSpeed;

                if (m_Angle > 3 * Mathf.PI / 2)
                {
                    m_Attacking = false;
                    attackTrail.gameObject.SetActive(false);
                }
            }
        }
    }


    //flip the character to face correct direction based on horizontal input
    public void flipPlayerY(float horizontalInput)
    {
        if ((horizontalInput > 0 && !m_FacingRight) || (horizontalInput < 0 && m_FacingRight))
        {
            transform.Rotate(0, 180, 0);
            m_FacingRight = !m_FacingRight;
        }
    }

    public void animatePlayerAttack()
    {
        m_Animator.SetTrigger("isAttacking");
        m_Angle = Mathf.PI / 2;
        currentAttackPoint = GetComponent<PlayerController>().attackPoint.position;
        attackDirectionRight = m_FacingRight;
        attackTrail.position = new Vector2(currentAttackPoint.x + 0.5f * Mathf.Cos(m_Angle), currentAttackPoint.y + 0.5f * Mathf.Sin(m_Angle));
        attackTrail.gameObject.SetActive(true);
        m_Attacking = true;
    }

    public void animateDamage(bool takesDamage)
    {
        if (takesDamage)
        {
            m_PlayerSpriteRenderer.color = damageColor;
            m_DamageTimer = damageAnimationTime;
        }
        else
        {
            m_PlayerSpriteRenderer.color = Color.white;
        }
    }

    public void animatePlayerDash()
    {
        m_Animator.SetTrigger("isDashing");
    }

    public void animatePlayerIdleOrWalking(float horizontalInput)
    {
        m_Animator.SetBool("isWalking", horizontalInput != 0);
        m_Animator.SetBool("isJumping", false);
        m_Animator.SetBool("isFalling", false);
    }

    public void animateJumpingOrFalling(bool isJumping)
    {
        m_Animator.SetBool("isWalking", false);
        m_Animator.SetBool("isJumping", isJumping);
        m_Animator.SetBool("isFalling", !isJumping);
    }
}
