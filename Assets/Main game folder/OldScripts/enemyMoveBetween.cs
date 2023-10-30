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


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = 40f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            EnemyDeath();
            return;
        }
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
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref health);

        }

    }
}
