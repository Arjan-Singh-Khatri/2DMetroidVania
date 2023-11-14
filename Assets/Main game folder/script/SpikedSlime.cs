using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedSlime : EnemyParentScript
{
    private Vector2 followPosition;
    private Animator animator;
    [SerializeField] GameObject slimeCollider;
    [SerializeField] private float speed = 1f;
    private float attackDownTime = 0;
    private bool isAttacking = false;
    private float startPoint;

    [SerializeField] GameObject spikes;
    [SerializeField] Transform spikesPosition;

    void Start()
    {
        health = 25f;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        startPoint = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0) return;

        FollowPlayer();
        if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 2f && attackDownTime >= 2f)
        {
            AttackPlayer();
        }

        if (attackDownTime < 2f) { attackDownTime += Time.deltaTime; }

    }

    private void FollowPlayer()
    {
        flip();
        if (PlayerInsideRange() && !isAttacking)
        {
            animator.SetBool("Run", true);
            followPosition.x = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime).x;
            followPosition.y = transform.position.y;
            transform.position = followPosition;
        }


    }

    private bool PlayerInsideRange()
    {
        if (Mathf.Abs(player.transform.position.x - startPoint) < 6f)
            return true;
        return false;
    }

    private void AttackPlayer()
    {
        animator.SetBool("Run", false);
        attackDownTime = 0f;
        isAttacking = true;
        slimeCollider.SetActive(true);
        ChosseAttack();
    }


    private void ChosseAttack()
    {

        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                animator.SetTrigger("Ability");
                break;
            case 2:
                animator.SetTrigger("Attack");
                break;
        }

    }

    private void TriggerAbility()
    {
        Instantiate(spikes,spikesPosition.position,transform.rotation);
    }

    private void AttackEnd()
    {
        slimeCollider.SetActive(false);
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            animator.SetTrigger("Hit");
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref health);
            if (health <= 0)
            {
                animator.SetTrigger("death");
            }
        }
    }
}
