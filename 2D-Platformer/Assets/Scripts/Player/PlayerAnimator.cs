using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public Color damageColor = new Color(0.78f, 0.23f, 0.23f);
    public float damageAnimationTime = 0.3f;

    private Animator m_Animator;
    private SpriteRenderer m_PlayerSpriteRenderer;

    private bool m_FacingRight;
    public bool facingRight
    {
        get { return m_FacingRight; }
    }

    private float m_DamageTimer;

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
