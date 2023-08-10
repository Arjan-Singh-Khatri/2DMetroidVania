using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMoveJump : EnemyParentScript
{
    private Rigidbody2D rig;
    [SerializeField]
    private Transform[] enemyWayPoints;
    [SerializeField] float speed = 5;
    private int wayPointIndex = 0;
    [SerializeField] private float jumpSpeed = 3;
    [SerializeField] private float jumpInterval = 1f;
    private float nextJumpTime = 0f;

    private int healthOfslime = 60;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthOfslime <= 0) return;
        EnemyMove();
    }

    void EnemyMove()
    {
        if (Vector2.Distance(transform.position, enemyWayPoints[wayPointIndex].position) > .5)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyWayPoints[wayPointIndex].position, speed * Time.deltaTime);
            Hop();
        }
        else
        {
            wayPointIndex++;
            if (wayPointIndex >= enemyWayPoints.Length)
            {
                wayPointIndex = 0;
            }
        }
    }

    private void Hop()
    {
        if (Time.time >= nextJumpTime)
        {
            rig.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
            nextJumpTime = Time.time + jumpInterval;
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
