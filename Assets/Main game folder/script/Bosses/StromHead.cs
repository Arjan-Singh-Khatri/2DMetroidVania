using System.Collections;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class StromHead : EnemyParentScript
{
    [SerializeField] private Collider2D stromAttackCollider;
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] firingLocations;
    private Animator animator;
    private bool _killed;
    private bool telePort = false;
    private float telePortTimer = 0f;
    private int telePortCount = 0;
    private float damageTaken = 0 ;

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

        
        
    }

    private void Frenzy()
    {
        // maybe an animation
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
                    // Animation off
                }
                Teleport();
                StartCoroutine(StromAttack());
            }

        }
    }
    private void ProjectileAttack()
    {

        // Maybe Use Event To Trigger The follow mechanic in projectile?
        
    }

    private void ProjectileInstantiate()
    {
        for(int i =0; i < firingLocations.Length; i++)
        {
            Instantiate(projectilePrefab, firingLocations[i]);
        }
    }

    private void Teleport(){
        int randomIndex = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[randomIndex].position;
    }

    private IEnumerator StromAttack(){

        yield return new WaitForSeconds(1.5f);
        stromAttackCollider.enabled = true;
        animator.SetTrigger("Strom");
        // Trigger Event -- StromAttackStart();

       
    } 
    private void StromAttackEnd() {

        stromAttackCollider.enabled = false;
        // Sleep Animation 
        // CountDownUntil Second Attack -- If Damage is Enough then the bool which start Countdown should be false
        // Trigger Event -- StromAttackEnd();
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

            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref this.health);
            damageTaken += DamageHolder.instance.playerDamage;
            if(health >= 0) {
                _killed = true;
                animator.SetTrigger("death");
            }
        }
            
    }
}
