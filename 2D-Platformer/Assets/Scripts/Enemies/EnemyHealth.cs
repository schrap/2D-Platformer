using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int health = 5;

    private int m_Health;


    void Start()
    {
        m_Health = health;
    }

    public void receiveDamage(int damage)
    {
        m_Health -= damage;

        if (m_Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
