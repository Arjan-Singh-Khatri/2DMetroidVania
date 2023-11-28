using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class StromHead : EnemyParentScript
{
    [SerializeField] private Collider2D stromAttackCollider;
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] firingLocations;
    [SerializeField] private ParticleSystem projectileAttackParticle;
    private Animator animator;
    private bool _killed;
    private bool telePort = false;
    private float telePortTimer = 0f;
    private int telePortCount = 0;
    private float damageTaken = 0 ;
    private float sleepTimer = 0;
    private bool isAttacking = false;
    private float _count;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = 100;
        animator = GetComponent<Animator>();
    }
    #region Save And Load
    #endregion

    // Update is called once per frame
    void Update(){

        if (_killed) return;
        if(damageTaken >=20) 
        {
            Frenzy();
        }
        if (isAttacking!){
            animator.SetBool("Sleep", true);
            sleepTimer -= Time.deltaTime;
            if(sleepTimer < 0) {
                sleepTimer = 5f;
                ChooseAttack();
                animator.SetBool("Sleep", false);
            }
        }
    }

    private void ChooseAttack(){
        // Choose Either Frenzy Or Projectile Attack
        int randomNumberForChoice = Random.Range(0, 11);
        if (randomNumberForChoice >= 4) {
            ProjectileAttack();
        } else
            Frenzy();
    }

    private void Frenzy()
    {
        isAttacking = true;
        if (telePort)
        {
            telePortTimer -= Time.deltaTime;
            if (telePortTimer <= 0)
            {
                telePortTimer = 2.5f;
                telePortCount++;
                if (telePortCount >= 5)
                {
                    telePort = false;
                    telePortTimer = 0f;
                }
                Teleport();
                StartCoroutine(StromAttack());
            }

        }
    }
    private void ProjectileAttack()
    {
        InvokeInstantiateFunction();  
    }

    private void InvokeInstantiateFunction()
    {
        projectileAttackParticle.Play();
        for(int i = 0; i < 3; i++)
        {
            _count++;
            Invoke(nameof(ProjectileInstantiate), i * 3);
        } 
    }

    private void ProjectileInstantiate()
    {
        
        bool follow;
        int randomNum = Random.Range(0, 2);
        if(randomNum ==0) follow = true;
        else follow = false;
        for(int i =0; i < firingLocations.Length; i++)
        {
            Instantiate(projectilePrefab, firingLocations[i]);
        }
        if(follow) { Events.instance.followPlayer(); }
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
        yield return new WaitForSeconds(1.3f);
        stromAttackCollider.enabled = true;
        animator.SetTrigger("Strom");
        //Events.instance.StromAttackStart();

       
    } 
    private void StromAttackEnd() {

        stromAttackCollider.enabled = false;
        isAttacking = false;
        //Events.instance.StromAttackEnd();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<playerDeath>().TakeDamage(DamageHolder.instance.stromHead);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("PlayerAttackHitBox")){
            animator.SetTrigger("hit");
            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref this.health);
            damageTaken += DamageHolder.instance.playerDamage;
            if(health >= 0) {
                _killed = true;
                animator.SetTrigger("death");
            }
        }
            
    }
}
