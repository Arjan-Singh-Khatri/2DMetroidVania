using DG.Tweening;
using System;
using UnityEngine;


public class TwoHead : EnemyParentScript
{
    [Header("Required Components")]
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform fireBallFirePosition;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject attackTwoHitBox;
    private Animator animator;
    [SerializeField] private GameObject wallFirst;
    [SerializeField] private GameObject wallSecond;
    [SerializeField] private float maxBoarder;
    [SerializeField] private float minBoarder;

    [Header("Variables")]
    private bool isHurt = false;
    private float fireBallSpawnTimer = 2f;
    private float fireAttackTimer = 10f;
    private bool chase = false;
    private readonly float endPointXOne = 25.2f;
    private readonly float endPointXTwo = 64.84f;
    private bool _killed;
    private bool playerInArea = false;

    [SerializeField] AudioClip fireBallAudio;
    [SerializeField] AudioClip runAudio;
    [SerializeField] AudioClip attackAudio;
    private float runTimer = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = mixerGroup;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    #region Save And Load
    public void SaveData(ref GameData gameData)
    {
        if (gameData.bossesKilled.ContainsKey(this.enemyID))
        {
            gameData.bossesKilled.Remove(this.enemyID);
        }
        gameData.bossesKilled.Add(this.enemyID, _killed);
    }

    public void LoadData(GameData gameData)
    {
        gameData.bossesKilled.TryGetValue(this.enemyID, out _killed);
        if (_killed)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    private void Update()
    {
        if (enemyDead || !playerInArea) return;

        if (health <= 0)
        {
            _killed = true;
            DataPersistanceManager.Instance.SaveGame();
            animator.SetTrigger("Death");
            enemyDead = true;
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
        _audioSource.PlayOneShot(fireBallAudio);
        Instantiate(fireBall,fireBallFirePosition.position,transform.rotation);
    }

    private void AttackTwo()
    {
        chase = false;
        animator.SetBool("attack1", false);
        attackTwoHitBox.SetActive(true);
        _audioSource.PlayOneShot(attackAudio);
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
            runTimer -= Time.deltaTime;
            if(runTimer <= 0)
            {
                //_audioSource.PlayOneShot(runAudio);
                runTimer = 0.8f;
            }

            Vector2 targetPosition = new Vector2(0,player.transform.position.y);
            if (player.transform.position.x < 0)
                targetPosition.x = player.transform.position.x - 3f;
            else
                targetPosition.x = player.transform.position.x + 3f;
            targetPosition.x = Mathf.Clamp(targetPosition.x, maxBoarder, minBoarder);
            Vector2 newPosition = Vector2.MoveTowards( transform.position, targetPosition, Time.deltaTime * moveSpeed);

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

    public void PlayerInTwoHeadArea() {
        playerInArea = true;
        wallFirst.SetActive(true);
        wallSecond.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Player"))
        {
            //player.GetComponent<playerDeath>().TakeDamage(DamageHolder.instance.twoHeadDamage);
            Events.instance.onPlayerTakeDamage(DamageHolder.instance.twoHeadDamage);
        }

        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            isHurt = true;
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier, ref health);

        }else if(collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            isHurt = true;
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier, ref health);

        }
    }
}
