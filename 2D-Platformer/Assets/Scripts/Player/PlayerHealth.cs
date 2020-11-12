using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int health;
    public float timeInvincible;

    private int m_Health;
    private float m_InvincibiltyTimer;

    private void Start()
    {
        m_Health = health;
        m_InvincibiltyTimer = 0;
    }

    private void Update()
    {
        m_InvincibiltyTimer -= Time.deltaTime;
    }

    //decrease health of player by given damage if not invincible, call death method if health falls below zero
    public void receiveDamage(int damage)
    {
        if (m_InvincibiltyTimer < 0)
        {
            m_Health -= damage;
            m_InvincibiltyTimer = timeInvincible;
            if (m_Health <= 0)
            {
                death();
            }
        }
    }

    //reset health on death, no game over yet
    private void death()
    {
        m_Health = health;
    }
}
