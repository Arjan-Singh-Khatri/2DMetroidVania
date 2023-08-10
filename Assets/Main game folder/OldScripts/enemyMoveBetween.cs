using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class enemyMoveBetween : EnemyParentScript
{
    [SerializeField]
    private Transform [] enemyWayPoints;
    [SerializeField] float speed = 5;
    private int wayPointIndex = 0;

    private int healthOfslime = 60;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthOfslime <= 0) return;
        EnemyMove();
    }

    void EnemyMove()
    {
        if(Vector2.Distance(transform.position, enemyWayPoints[wayPointIndex].position) > .5)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyWayPoints[wayPointIndex].position,speed*Time.deltaTime);
        }else
        {
            wayPointIndex++;
            if (wayPointIndex >= enemyWayPoints.Length) 
            {
                wayPointIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack(collision.gameObject);
            HealthDepleteEnemy(ref healthOfslime);
            if (healthOfslime <= 0)
                EnemyDeath();
        }

    }
}
