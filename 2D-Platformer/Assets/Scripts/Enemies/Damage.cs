using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public Transform player;
    public int damageDealt;

    private PlayerHealth m_PlayerHealth;

    private void Start()
    {
        m_PlayerHealth = player.GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.GetComponent<Collider2D>().Equals(collision))
        {
            m_PlayerHealth.receiveDamage(damageDealt);
        }
    }

}

