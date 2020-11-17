using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator m_Animator;
    private bool m_FacingRight;

    public bool facingRight
    {
        get { return m_FacingRight; }
    }

    
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_FacingRight = true;
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

    public void animateDamageTaken()
    {
        m_Animator.SetTrigger("isDamaged");
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
