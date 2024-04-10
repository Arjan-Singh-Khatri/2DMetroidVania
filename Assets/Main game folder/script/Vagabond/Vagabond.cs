using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private float _dashSpeed = 5f;
    //[SerializeField] private float blockDistance = 4f;
    

    [SerializeField] private float checkPlayerInRangeTimer;
    [SerializeField] private float chasingPlayerTimer;


    [Header("Variables for enemy AI")]
    private bool chase = true;
    private bool attackDowntime;
    //private bool isInBlockRange;
    private float chaseMaxSpeed = 4f;
    private float chaseMinSpeed = 1f;
    private float distanceBetween;
    private bool isDieing;
    private bool isBlocking;
    private bool isTired;
    private float isTiredTimer;
    private bool isDashing;

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
        
        
        hit = Physics2D.CircleCast(transform.position, circleCastRadius, new Vector2(direction, 0));
        distanceBetween = Mathf.Abs(player.transform.position.x - transform.position.x);

        TiredChecks();
        EnemyAI();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(Dash());
        }


    }
    #region AI
    void EnemyAI(){
        if (isCharingAttack || isAttacking || isHeavyAttacking || isBlocking || isTired) return;
        flip();
        if (hit.collider.name == player.name)
        {
            //if running away then no attack nor block
            //if not running away then either attack or block and if the damage multiplier is getting high of the player than more probability of blocks
        }
        if (chase)
        {
            Chase();   
        }else if(!chase && isDashing)
        {
            StartCoroutine(Dash());
        }
        
    }

   

    void Chase(){

        // HOW DO I WANT THE AI TO BE ?
        // I WANT IT TO DASH IF THE DASH WILL GET TO CLOSE ENOUGH TO THE PLAYER THAT THE PLAYER IS IN ATTACK RANGE WITH SOME PROBABILITY AND NO MATTER EXECUTE THE NORMAL ATTACK
        // IF THE HEALTH IS BELOW A THRESHOLD THEN A LITTLE LESS DASHES AND A BIT MORE BLOCKS
        // THE MORE TIME GOES ON WITHOUT ANY ATTACK OR BLOCKS FROM EITHER PLAYER OR ENEMEY  - INCREASE THE PROBABILITY OF THE DASHES 


        float lerpSpeed = distanceBetween / 35;
        float chaseSpeed = Mathf.Lerp(chaseMinSpeed, chaseMaxSpeed, lerpSpeed) ;
        Vector2 chaseVector = new(chaseSpeed * direction , 0f);

        rigbody.AddForce(chaseVector , ForceMode2D.Force);

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
    IEnumerator Dash()
    {
        isDashing = true;
        Debug.Log("DASH");
        Vector2 dashVector = new(direction * _dashSpeed, 0);
        //rigbody.velocity = dashVector * _dashSpeed; 
        rigbody.AddForce(dashVector, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.5f);
        isDashing = false;
        //animator.SetBool("Dash", true);
    }


    void BlockAttack(){ 
        // Animation and make it so that it takes no damage 
        // And then transition from block to either attack or dash back 
    }


    void DashAwayFromPlayer(){
        Debug.Log("Dashing awawy");
        chase = true;
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
