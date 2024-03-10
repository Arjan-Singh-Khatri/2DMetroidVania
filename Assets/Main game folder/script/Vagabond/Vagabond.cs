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
    [SerializeField] private GameObject normalAttackHitbox2;


    private RaycastHit2D hit;

    [Header("Variables for enemy AI")]
    private bool chase;

    private bool attackDowntime;
    private bool isInBlockRange;
    [SerializeField]private float checkPlayerInRangeTimer;
    [SerializeField]private float chasingPlayerTimer;
    private float chaseMaxSpeed = 8f;
    private float chaseMinSpeed = 5f;
    private float distanceBetween;

    [Header("Attack Choose")]
    int normalAttackChoosenCount;
    int boarderNumberForChoice = 8;
    private bool isCharingAttack;
    private bool isHeavyAttacking;
    private bool isAttacking;
    private bool isBlocking;

    [SerializeField] string animationTesting;


    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        //if (Input.GetKeyDown(KeyCode.Escape)){
        //    Debug.Log("PRESSES KEY");
        //    animator.SetTrigger(animationTesting);   
        //}
        //return;
        hit = Physics2D.CircleCast(transform.position, circleCastRadius, new Vector2(direction, 0));
        distanceBetween = Mathf.Abs(player.transform.position.x - transform.position.x);
        if(distanceBetween <= blockDistance)
            isInBlockRange = true;
        
        EnemyAI();
        
    }
    #region AI
    void EnemyAI()
    {
        if (isCharingAttack || isAttacking || isHeavyAttacking || isBlocking) return;
        flip();
        if (hit.collider.name == player.name)
        {
            chase = false;
            if (isInBlockRange)
            {
                BlockAttack();
            }
            ChooseAttack();
        }
    }

    void Chase(){
        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseMaxSpeed*Time.deltaTime);
        Vector2 position = transform.position;
        Vector2 playerPosition = player.transform.position;

        float lerpSpeed = distanceBetween / 36;
        float chaseSpeed = Mathf.Lerp(chaseMinSpeed, chaseMaxSpeed, lerpSpeed) * Time.deltaTime;
        Vector2 newPos = new(0, transform.position.y)
        {
            x = Mathf.MoveTowards(position.x, playerPosition.x, chaseSpeed)
        };
        transform.position = newPos;

    }
    #endregion

    #region Attack
    void ChooseAttack()
    {
        int randomNumber = Random.Range(0, 10);
        if(randomNumber>boarderNumberForChoice) {
            ChargingAttack(nameof(NormalAttack));
            normalAttackChargeTime =+ 1;
            if (normalAttackChargeTime > 4)
                boarderNumberForChoice++;
        }else{
            ChargingAttack(nameof(NormalAttack));
            boarderNumberForChoice = 7;
        }
    }

    void ChargingAttack(string attackName){
        isCharingAttack = true;
        if (string.Equals(attackName, nameof(NormalAttack))){
            animator.SetTrigger("ChargeAttack");
        }
        else{
            animator.SetTrigger("ChargeHeavy");
        }
    }

    void NormalAttack(){
        isCharingAttack= false;
        isAttacking = true;
    }
    void HeavyAttack()
    {
        isCharingAttack = false;
        isHeavyAttacking = true;
    }

    void ActivateNormalAttckHitbox(){
        normalAttackHitbox.SetActive(true);
        
    }
    
    void ActivateNormalAttackHitbox2(){
        normalAttackHitbox.SetActive(false);
        normalAttackHitbox2.SetActive(true);
    }
    
    void ActivateHeavyAttackHitbox()
    {
        heavyAttackHitbox.SetActive(true);
    }

    void DeactivateNormalAttckHitbox(){
        normalAttackHitbox2.SetActive(false);
        isAttacking = false;
        DashAwayFromPlayer();
    }
    void DeactivateHeavyAttackHitbox()
    {
        heavyAttackHitbox.SetActive(false);
        //PLAY TIRED ANIMATION
    }

    void AfterHeavyTired(){
        isHeavyAttacking = false;
        DashAwayFromPlayer(); 
    }

    void DashAwayFromPlayer(){
        // USING ANOTHER RAYCAST IF THERE IS ANY WALL IN ANOTHER DIRECTION THEN DASH WHERE
        // IF NOT WALL IN ANY DIRECTION THEN JUST DASH BACK FROM PLAYER
    }
    #endregion

    #region Block and Dash
    void BlockAttack()
    {
        // Animation and make it so that it takes no damage 
        // And then transition from block to either attack or dash back 
    }

    void Dash(float dashDirection)
    {

    }
    #endregion
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }
}
