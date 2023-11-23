using System.Xml.Serialization;
using UnityEngine;

public class StromHead : EnemyParentScript
{
    [SerializeField] private Collider2D stromAttackCollider;
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private GameObject projectilePrefab;
    private Animator animator;
    private bool _killed;

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

    private void Teleport(){

    }

    private void StromAttack(){

        stromAttackCollider.enabled = true;
        animator.SetTrigger("Strom");
    } 
    private void StromAttackEnd() {

        stromAttackCollider.enabled = false;
    }

    private void ProjectileAttack(){
        InstantiateProjectiles();
    }

    private void InstantiateProjectiles() {
        
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
            if(health >= 0) {
                _killed = true;
                animator.SetTrigger("death");
            }
        }
            
    }
}
