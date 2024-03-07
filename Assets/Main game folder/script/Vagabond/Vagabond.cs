using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Vagabond : EnemyParentScript
{
    private Animator animator;
    private Rigidbody2D rigbody;
    
    [SerializeField] private float circleCastRadius = 3.8f;
    [SerializeField] private float blockDistance = 4f;
    [SerializeField] private float heavyAttackChargeTime = 0;
    [SerializeField] private float normalAttackChargeTime = 0;
    [SerializeField] private GameObject normalAttackHitbox;
    [SerializeField] private GameObject heavyAttackHitbox;

    private RaycastHit2D hit;

    [Header("Variables for enemy AI")]
    private bool chase;
    private bool chaseStopUntiPlayerInRange;
    private bool isChargingAttack;
    private bool isHurt;
    private bool isTired;
    private bool attackDowntime;
    private bool playerComingTowardUs;
    [SerializeField]private float checkPlayerInRangeTimer;
    [SerializeField]private float chasingPlayerTimer;
    private float chaseMaxSpeed = 8f;
    private float chaseMinSpeed = 5f;

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        hit = Physics2D.CircleCast(transform.position, circleCastRadius, new Vector2(direction,0));
        flip();
        //EnemyAI();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            animator.SetTrigger("Attack");
        }
    }

    void EnemyAI()
    {
        if(hit.collider.name == player.name)
        {
            chase = false;
            ChooseAttack();
        }
        if (chase) Chase();
    }

    void Chase(){
        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseMaxSpeed*Time.deltaTime);
        Vector2 position = transform.position;
        Vector2 playerPosition = player.transform.position;

        float lerpSpeed = Mathf.Abs(player.transform.position.x - transform.position.x) / 36;
        float chaseSpeed = Mathf.Lerp(chaseMinSpeed, chaseMaxSpeed, lerpSpeed) * Time.deltaTime;
        Vector2 newPos = new(0, transform.position.y)
        {
            x = Mathf.MoveTowards(position.x, playerPosition.x, chaseSpeed)
        };
        transform.position = newPos;

    }

    void DashBack(){

    }

    #region Attack And Block
    void ChooseAttack()
    {
        /// Choose one attack 
        // Charge that Attack 
        // Attack 
    }

    void NormalAttack()
    {
        
    }

    void ActivateNormalAttckHitbox()
    {
        normalAttackHitbox.SetActive(true);
    }
    void DeactivateNormalAttckHitbox()
    {
        normalAttackHitbox.SetActive(false);
    }

    void HeavyAttack()
    {
        
    }

    void ActivateHeavyAttackHitbox()
    {
        heavyAttackHitbox.SetActive(true);
    }

    void DeactivateHeavyAttackHitbox()
    {
        heavyAttackHitbox.SetActive(false);
    }

    void BlockAttack()
    {
        // Animation and make it so that it takes no damage 
        // And then transition from block to either attack or dash back 
    }
    #endregion

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }
}
