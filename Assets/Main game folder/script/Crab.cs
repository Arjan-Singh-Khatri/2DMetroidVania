using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Crab : EnemyParentScript
{
    private Vector2 followPosition;
    private Animator animator;
    [SerializeField] GameObject crabCollider;
    [SerializeField]private float speed = 1f;
    private float attackDownTime = 0;
    private bool isAttacking = false;
    private float startPoint;
    private bool _killed;


    void Start()
    {
        health = 25f;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        startPoint = transform.position.x;
        
    }

    #region Save And Load
    public void SaveData(ref GameData gameData)
    {
        if (gameData.enemyKilled.ContainsKey(this.enemyID))
        {
            gameData.enemyKilled.Remove(this.enemyID);
        }
        gameData.enemyKilled.Add(this.enemyID, _killed);
    }

    public void LoadData(GameData gameData)
    {
        gameData.enemyKilled.TryGetValue(this.enemyID, out _killed);
        if (_killed)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
        if (health <= 0) return;

        FollowPlayer();
        if (Mathf.Abs(transform.position.x - player.transform.position.x) <=2f && attackDownTime >= 1.4f )
        {
            AttackPlayer();
        }
        
        if (attackDownTime < 1.4f) { attackDownTime += Time.deltaTime; }

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
        crabCollider.SetActive(true);
        ChosseAttack();
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


    private void AttackEnd()
    {
        crabCollider.SetActive(false);
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
                _killed = true;
                animator.SetTrigger("death");
            }

           
        }
    }
}
