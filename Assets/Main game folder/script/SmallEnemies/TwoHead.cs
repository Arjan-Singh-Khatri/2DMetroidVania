using DG.Tweening;
using System;
using TMPro;
using UnityEngine;


public class TwoHead : EnemyParentScript
{
    [Header("Required Components")]
    private Animator animator;
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform fireBallFirePosition;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject attackTwoHitBox;
    [SerializeField] private GameObject wallFirst;
    [SerializeField] private GameObject wallSecond;
    [SerializeField] AudioClip fireBallAudio;
    [SerializeField] AudioClip runAudio;
    [SerializeField] AudioClip attackAudio;

    [Header("Variables")]
    private bool isHurt = false;
    private float fireBallSpawnTimer = 2f;
    private float fireAttackTimer = 10f;
    private bool chase = false;
    [SerializeField] private float endPointXOne = 25.2f;
    [SerializeField] private float endPointXTwo = 64.84f;
    private bool _killed;
    private bool playerInArea = false;
    private float runDelayTimer = 0f;
    private Vector2 targetPosition;


    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = mixerGroup;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        targetPosition = new Vector2(0, transform.position.y);
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
        if (_killed || !playerInArea) return;

        if (health <= 0){
            EnemyDead();
        }

        flip();

        if(isHurt){

            SecondAttack();
            isHurt = false;
        }else{

            AttackPlayer();
        }
    }

    private void InstantiateFireBall()
    {
        _audioSource.PlayOneShot(fireBallAudio);
        Instantiate(fireBall,fireBallFirePosition.position,transform.rotation);
    }

    private void SecondAttack(){ 
        
        chase = false;
        animator.SetBool("attackOne", false);
        attackTwoHitBox.SetActive(true);
        _audioSource.PlayOneShot(attackAudio);
        animator.SetBool("attackTwo", true);
        isHurt = false;
    }

    private void SecondAttackEnd(){
        animator.SetBool("attackTwo", false);
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

    private void RunAttack(){

        if(transform.position.x >= endPointXOne+1 || transform.position.x <= endPointXTwo - 1) {

            runDelayTimer -= Time.deltaTime;
            // calculate the target vector with a delay so that its feel like the momentum is taking the enemy further
            if (runDelayTimer <= 0f){
                CalcualteTargetVector(); 
                runDelayTimer = 1.7f;
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }
         
    }

    private void CalcualteTargetVector(){

        if (player.transform.position.x < transform.position.x)
            targetPosition.x = player.transform.position.x - 5f;
        else
            targetPosition.x = player.transform.position.x + 5f;

        targetPosition.x = Mathf.Clamp(targetPosition.x, endPointXOne, endPointXTwo);

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
                animator.SetBool("attackOne", true);
                fireAttackTimer = 7f;
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

    private void EnemyDead() {
        _killed = true;
        DataPersistanceManager.Instance.SaveGame();
        animator.SetTrigger("Death");
        wallFirst.SetActive(false);
        wallSecond.SetActive(false);
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision){

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
