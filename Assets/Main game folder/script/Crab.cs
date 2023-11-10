using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Crab : EnemyParentScript
{

    private Vector3 currentTransform;
    private Animator animator;
    private float distanceFromPlayer;
    private float attackDownTime =0;
    [SerializeField] GameObject crabCollider;
    [SerializeField]private float speed = 10f;

    void Start()
    {
        health = 25f;
        player = GameObject.FindGameObjectWithTag("Player");
        currentTransform = gameObject.transform.position;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (health <= 0) return;
        if (Mathf.Abs(currentTransform.x - player.transform.position.x) < 10f)
        {
            
            AttackPlayer();
        }
    }

    private void ChosseAttack()
    {
        int random = Random.Range(1, 5);
        switch (random) {
            case 1:
                animator.SetTrigger("Ability");
                break;
            case 2:
                animator.SetTrigger("Attack");
                break;
            case 3:
                animator.SetTrigger("Attack2");
                break;
            case 4:
                animator.SetTrigger("Attack3");
                break;
        }
        
    }

    private void AttackPlayer()
    {
        flip();
        transform.position = new Vector2(Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime).x,transform.position.y);
        if(Mathf.Abs(currentTransform.x - player.transform.position.x) <2f)
        {
            attackDownTime -= Time.deltaTime;
            if(attackDownTime < 0)
            {
                attackDownTime = 1.8f;
                crabCollider.SetActive(true);
                ChosseAttack();
            }
        }
    }

    private void AttackColliderOff()
    {
        crabCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref health);
            if (health <= 0)
            {
                animator.SetTrigger("death");
            }
        }
    }
}
