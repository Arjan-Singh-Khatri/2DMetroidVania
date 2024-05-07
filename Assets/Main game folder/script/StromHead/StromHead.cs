using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class StromHead : EnemyParentScript
{
    [Header("Serilizable")]
    [SerializeField] private Collider2D stromAttackCollider;
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] firingLocations;
    [SerializeField] private ParticleSystem projectileAttackParticle;

    [Header("Variables")]
    private Animator animator;
    
    public bool _killed = false;
    private float telePortTimer = 0f;
    private float telePortCount = 0f;
    private float damageTaken = 0 ;
    private float sleepTimer = 10f;
    private bool isAttacking = false;
    private float _count =0;
    private bool _frenzy = false;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = 100;
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

    // Update is called once per frame
    void Update(){
        if (_killed) return;
        
        if (_frenzy) StartFrenzy();

        if(damageTaken >20){
            damageTaken = 0; 
            StartFrenzy();
        }
        SleepState();
    }

    private void SleepState()
    {
        if (!isAttacking)
        {
            sleepTimer -= Time.deltaTime;
            if(sleepTimer < 0) {
                sleepTimer = 10f;
                animator.SetBool("Sleep", false);
                ChooseAttack(); 
            }
        }
    }
    private void ChooseAttack(){
        isAttacking = true;
        // Choose Either Frenzy Or Projectile Attack

        int randomNumberForChoice = Random.Range(0,11);
        if (randomNumberForChoice >= 4){
            LightingStrikeAttack();
        }
        else{
            StartFrenzy();
        }

    }

    private void StartFrenzy()
    {
        _frenzy = true;
        telePortTimer-= Time.deltaTime;
        if(telePortTimer < 0)
        {
            telePortTimer = 3f;
            telePortCount++;
            Teleport();
            if (telePortCount >= 5)
            {
                StartCoroutine(StromAttack());
                _frenzy = false;
                telePortTimer = 0f;
            }
        }
    }
    [ContextMenu("Lighting")]
    private void LightingStrikeAttack()
    {
        // To Call InstantiateFunction which instantiate the projectiles i.e lighting
        CallProjectileInstantiation();  
    }

    private void CallProjectileInstantiation(){
        projectileAttackParticle.Play();
        for(int i = 0; i < 3; i++)
        {
            _count++;
            Invoke(nameof(ProjectileInstantiate), i * 3);
        } 
    }

    private void ProjectileInstantiate()
    {
        // the actual instantiation
        for(int i =0; i < firingLocations.Length; i++)
        {
            GameObject instantiatedObject = Instantiate(projectilePrefab, firingLocations[i]);
        }
        
        // For how many rounds of lighting strikes to happen
        if (_count == 3)
        {
            _count = 0;
            isAttacking = false;
            projectileAttackParticle.Stop();
        }
    }

    private void Teleport(){
        int randomIndex = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[randomIndex].position;
    }

    private IEnumerator StromAttack(){

        yield return new WaitForSeconds(.7f);
        projectileAttackParticle.Play();
        stromAttackCollider.enabled = true;
        animator.SetTrigger("Strom");
        Events.instance.StromAttackStart();

    } 
    private void StromAttackEnd() {
        projectileAttackParticle.Stop();
        stromAttackCollider.enabled = false;
        Events.instance.StromAttackEnd();
        isAttacking = false;
        animator.SetBool("Sleep", true);
    }


    private void OnTriggerEnter2D(Collider2D collision){
        
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<playerDeath>().TakeDamage(DamageHolder.instance.stromHead);
        }

        if (collision.gameObject.CompareTag("PlayerAttackHitBox") || collision.gameObject.CompareTag("HeavyHitBox")){
            if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
            {
                HealthDepleteEnemy(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier, ref this.health);
                damageTaken += DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier;
            }else if (collision.gameObject.CompareTag("HeavyHitBox"))
            {
                HealthDepleteEnemy(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier, ref this.health);
                damageTaken += DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier;
            }

            if(health <= 0) {
                _killed = true;
                animator.SetTrigger("death");
            }
        }
            
    }
}
