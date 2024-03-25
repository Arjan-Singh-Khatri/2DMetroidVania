using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Vagabond : EnemyParentScript
{
    private const float TIRED_TIME = 2.7f;
    // MAKE AN ENTRY ANIMATION WHERE HE LIKE WAKES UP THEN TRANSITION THAT TO THE IDEL STATE 
    private Animator animator;
    private Rigidbody2D rigbody;
    private RaycastHit2D hit;

    [Header("SERIALIZABLE FIELDS")]
    [SerializeField] private GameObject normalAttackHitbox;
    [SerializeField] private GameObject heavyAttackHitbox;
    [SerializeField] private GameObject normalAttackHitbox2;
    [SerializeField] private float circleCastRadius = 3.8f;
    [SerializeField] private float blockDistance = 4f;
    

    [SerializeField] private float checkPlayerInRangeTimer;
    [SerializeField] private float chasingPlayerTimer;


    [Header("Variables for enemy AI")]
    private bool chase = true;
    private bool attackDowntime;
    private bool isInBlockRange;
    private float chaseMaxSpeed = 8f;
    private float chaseMinSpeed = 5f;
    private float distanceBetween;
    private bool isDieing;
    private bool isBlocking;
    private bool isTired;
    private float isTiredTimer;

    [Header("Attack Choose")]
    int normalAttackChoosenCount;
    int boarderForAttackChoice = 8;
    private bool isCharingAttack;
    private bool isHeavyAttacking;
    private bool isAttacking;
  


    [Header("For Testing")]
    [SerializeField] string animationTesting;

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if (isDieing)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("PRESSES KEY");
            animator.SetTrigger(animationTesting);
        }
        TiredChecks();
        return;
        hit = Physics2D.CircleCast(transform.position, circleCastRadius, new Vector2(direction, 0));
        distanceBetween = Mathf.Abs(player.transform.position.x - transform.position.x);
        if(distanceBetween <= blockDistance)
            isInBlockRange = true;
        
        EnemyAI();

    }
    #region AI
    void EnemyAI(){
        if(isTired){
            isTiredTimer += Time.deltaTime;
            if (isTiredTimer >=TIRED_TIME)
            {
                TiredStateEnd();
            }
        }
        if (isCharingAttack || isAttacking || isHeavyAttacking || isBlocking || isTired) return;
        flip();
        if (chase)
        {
            Chase();   
        }
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
        // ANIMATION AND LOOP THE ANIMATION
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
    void ChooseAttack(){
        int randomNumber = Random.Range(0, 10);
        if(randomNumber<boarderForAttackChoice) {
            ChargingAttack(nameof(NormalAttack));
            normalAttackChoosenCount = + 1;
            if (normalAttackChoosenCount > 4)
                boarderForAttackChoice++;
        }else{
            ChargingAttack(nameof(HeavyAttack));
            boarderForAttackChoice = 7;
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
    void HeavyAttack() {
        isCharingAttack = false;
        isHeavyAttacking = true;
    }

    #endregion

    #region Activations
    void ActivateNormalAttckHitbox(){
        normalAttackHitbox.SetActive(true);
        
    }
    
    void ActivateNormalAttackHitbox2(){
        normalAttackHitbox.SetActive(false);
        normalAttackHitbox2.SetActive(true);
    }
    
    void ActivateHeavyAttackHitbox(){
        heavyAttackHitbox.SetActive(true);
    }

    void DeactivateNormalAttckHitbox(){
        normalAttackHitbox2.SetActive(false);
        isAttacking = false;
        DashAwayFromPlayer();
    }

    void DeactivateHeavyAttackHitbox(){
        heavyAttackHitbox.SetActive(false);
        isHeavyAttacking= false;
        isTired = true;
        isTiredTimer = TIRED_TIME;
        animator.SetBool("Tired", true);
    }
    void TiredStateEnd()
    {
        isTired = false;
        animator.SetBool("Tired", false);
        DashAwayFromPlayer();
    }
    #endregion

    #region Block , Dash and Tired
    void BlockAttack(){
        // Animation and make it so that it takes no damage 
        // And then transition from block to either attack or dash back 
    }
    void Dash(float dashDirection){

    }
    void DashAwayFromPlayer(){
        Debug.Log("Dashing awawy");
        //chase = true;
        // USING ANOTHER RAYCAST IF THERE IS ANY WALL IN ANOTHER DIRECTION THEN DASH WHERE
        // IF NOT WALL IN ANY DIRECTION THEN JUST DASH BACK FROM PLAYER
    }

    void TiredChecks()
    {
        if (isTired)
        {
            isTiredTimer -= Time.deltaTime;
            if (isTiredTimer <= 0f)
            {
                isTiredTimer = TIRED_TIME;
                TiredStateEnd();
            }
        }
    }
    #endregion

    #region DEATH

    [ContextMenu("DIE")]
    void Die()
    {
        isDieing = true;
        animator.SetTrigger("Death");
        gameObject.GetComponent<Collider2D>().enabled = false;
        rigbody.bodyType = RigidbodyType2D.Static;
    }

    void Destroy()
    {
        Destroy(gameObject);    
    }
    #endregion

    private void OnDrawGizmos(){
        
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }
}
