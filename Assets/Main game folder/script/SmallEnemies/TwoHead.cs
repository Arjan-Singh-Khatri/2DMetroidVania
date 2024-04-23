using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager.UI;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class TwoHead : EnemyParentScript
{
    [Header("Required Components")]
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform fireBallFirePosition;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject attackTwoHitBox;
    private Animator animator;



    [Header("Variables")]
    private bool isHurt = false;
    private float fireBallSpawnTimer = 2f;
    private float fireAttackTimer = 10f;
    private bool chase = false;
    private readonly float endPointXOne = 25.2f;
    private readonly float endPointXTwo = 64.84f;
    private bool _killed;

    private void Awake()
    {
        health = 60;
    }
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

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

    private void Update()
    {
        if (enemyDead) return;
        if (health <= 0)
        {
            _killed = true;
            animator.SetTrigger("Death");
            enemyDead = true;
            Events.instance.OnDeathTwoHeadBridgeActive();
            return;
        }
        flip();
        if(isHurt)
        {
            AttackTwo();
            isHurt = false;
        }else
        {
            AttackPlayer();
        }
    }

    private void InstantiateFireBall()
    {
        Instantiate(fireBall,fireBallFirePosition.position,transform.rotation);
    }

    
    private void AttackTwo()
    {
        chase = false;
        animator.SetBool("attack1", false);
        attackTwoHitBox.SetActive(true);
        animator.SetTrigger("attack2");
        isHurt = false;
    }
    public void AttackTwoEnd()
    {
        attackTwoHitBox.SetActive(false);    
    }
    
    private void FireBallAttack()
    {
        fireBallSpawnTimer -= Time.deltaTime;    
        if (fireBallSpawnTimer <= 0)
        {
            InstantiateFireBall();
            fireBallSpawnTimer = 2;
        }  
    }

    private void RunAttack()
    {

        if(transform.position.x >= endPointXOne+1 || transform.position.x <= endPointXTwo-1)
        {
            Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
            Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            transform.position = newPosition;
        }
         
    }

    private void AttackPlayer()
    {
        if (!chase)
        {
            fireAttackTimer -= Time.deltaTime;
            FireBallAttack();
            if(fireAttackTimer <= 0)
            {
                chase = true;
                animator.SetBool("attack1", true);
            }    
        }else
        {
            RunAttack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            isHurt = true;
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref health);

        }
    }
}
