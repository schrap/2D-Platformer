using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float movementSpeed;

    [Tooltip("make enemy go from last waypoint straight to first doing a loop, instead of going along waypoints in reverse")]
    public bool loop;

    private Transform m_Enemy;
    private int m_NextWaypoint;
    private int direction;

    private int lowerBound;


    void Start()
    {
        m_Enemy = GetComponent<Transform>();
        if (waypoints.Length > 1)
        {
            m_NextWaypoint = 1;
            direction = 1;
        }
    }


    private void Update()
    {
        lowerBound = loop ? 0 : 1;

        if (m_Enemy.position != waypoints[m_NextWaypoint].position)
        {
            m_Enemy.position = Vector2.MoveTowards(m_Enemy.position, waypoints[m_NextWaypoint].position, movementSpeed * Time.deltaTime);
        }
        else if (m_NextWaypoint >= lowerBound && m_NextWaypoint < waypoints.Length - 1)
        {
            m_NextWaypoint += direction;
        }
        else
        {
            if (loop)
            {
                m_NextWaypoint = 0;
            }
            else
            {
                direction *= -1;
                m_NextWaypoint += direction;
            }

        }
    }
}

