using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public RectTransform HealthBar;
    public Sprite healthFull;
    public Sprite healthDamaged;

    public int health;
    public float timeInvincible;

    private int m_Health;
    private float m_InvincibiltyTimer;

    private Image[] m_HealthBar;


    private void Start()
    {
        m_Health = health;
        m_InvincibiltyTimer = 0;

        m_HealthBar = HealthBar.GetComponentsInChildren<Image>();
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

            m_Health = m_Health < 0 ? 0 : m_Health;
            for (int i = m_Health; i < health; i++)
            {
                m_HealthBar[i].sprite = healthDamaged;
            }

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
        for (int i = 0; i < health; i++)
        {
            m_HealthBar[i].sprite = healthFull;
        }
    }
}
