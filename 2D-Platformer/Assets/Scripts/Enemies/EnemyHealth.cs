using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int health = 5;

    public Color damageColor = new Color(0.78f, 0.23f, 0.23f);
    public float damageAnimationTime = 0.3f;

    private SpriteRenderer m_EnemySpriteRenderer;
    private int m_Health;

    private float m_DamageTimer;


    void Start()
    {
        m_EnemySpriteRenderer = GetComponent<SpriteRenderer>();
        m_Health = health;
    }

    private void Update()
    {
        m_DamageTimer -= Time.deltaTime;
        if (m_DamageTimer < 0)
        {
            animateDamage(false);
        }
    }

    public void receiveDamage(int damage)
    {
        m_Health -= damage;

        if (m_Health <= 0)
        {
            gameObject.SetActive(false);
        }

        animateDamage(true);
    }


    public void animateDamage(bool takesDamage)
    {
        if (takesDamage)
        {
            m_EnemySpriteRenderer.color = damageColor;
            m_DamageTimer = damageAnimationTime;
        }
        else
        {
            m_EnemySpriteRenderer.color = Color.white;
        }
    }
}
