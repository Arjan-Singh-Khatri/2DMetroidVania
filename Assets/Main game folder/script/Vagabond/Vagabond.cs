using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Vagabond : EnemyParentScript
{

    // MAKE AN ENTRY ANIMATION WHERE HE LIKE WAKES UP THEN TRANSITION THAT TO THE IDEL STATE 
    [Header("GET COMPONENTS")]
    private Animator animator;
    private Rigidbody2D rigbody;
    private RaycastHit2D hit;

    [Header("MISCELLANEOUS FOR NOW ")]
    [SerializeField] private float circleCastRadius = 3.8f;
    //[SerializeField] private float blockDistance = 4f;
    private bool isDieing;
    private bool isBlocking;
    private bool isTired;
    private const float TIRED_TIME = 2.7f;
    private float isTiredTimer;

    [Header("DASH")]
    [SerializeField] private float _dashSpeed = 5f;
    [SerializeField] private float _canPerformDashDistance;
    [SerializeField] private float _dashCheckRight;
    [SerializeField] private float _dashCheckLeft;
    private bool isDashing;


    [Header("CHASE / FOLLOW")]
    [SerializeField] private float chaseMaxSpeed = 4f;
    [SerializeField] private float chaseMinSpeed = 2f;
    [SerializeField] private float _chaseForce = 10f;
    private float distanceBetween;
    private bool chase = true;

    [Header("Attack ")]
    [SerializeField] private GameObject normalAttackHitbox;
    [SerializeField] private GameObject heavyAttackHitbox;
    [SerializeField] private GameObject normalAttackHitbox2;
    [SerializeField] private float checkPlayerInRangeTimer;
    [SerializeField] private float chasingPlayerTimer;
    private float attackCoolDownTimer;
    private int normalAttackChoosenCount;
    private int boarderForAttackChoice = 8;
    private bool isCharingAttack;
    private bool isHeavyAttacking;
    private bool isAttacking;
    private bool attackDowntime;
    private bool canAttack = true;


    
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
    }

    void Update(){

        if (isDieing)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
            StartCoroutine(Dash(direction));

        hit = Physics2D.CircleCast(transform.position, circleCastRadius, new Vector2(direction, 0));
        distanceBetween = Mathf.Abs(player.transform.position.x - transform.position.x);

        CanAttackCheck();
        TiredChecks();
        EnemyAI();

    }
    #region AI
    void EnemyAI(){
        if (isCharingAttack || isAttacking || isHeavyAttacking || isBlocking || isTired || isDashing) return;

        flip();

        if (hit.collider.name == player.name)
        {
            ChooseAttack();
        }
        if (chase)
        {
            Chase();   
        }
        
    }

    void Chase(){
        //PerformDashWithChecks();

        if (animator.GetBool("Run") == false)
            animator.SetBool("Run", true);

        float lerpSpeed = distanceBetween / 35;
        float chaseSpeed = Mathf.Lerp(chaseMinSpeed, chaseMaxSpeed, lerpSpeed) ;
        Vector2 chaseVector = new(chaseSpeed * direction * _chaseForce , 0f);

        rigbody.AddForce(chaseVector , ForceMode2D.Force);

    }
    #endregion

    #region Attack
    void ChooseAttack(){

        if (!canAttack) return;

        isCharingAttack = true;
        chase = false;
        int randomNumber = Random.Range(0, 10);

        animator.SetBool("Run", false);

        if(randomNumber < boarderForAttackChoice) {
            normalAttackChoosenCount += 1;
            if (normalAttackChoosenCount > 4)
                boarderForAttackChoice++;
            ChargingAttack(nameof(NormalAttack));
            
        }else{
            boarderForAttackChoice = 7;
            ChargingAttack(nameof(HeavyAttack));
        }
    }

    void ChargingAttack(string attackName){
        if (string.Equals(attackName, nameof(NormalAttack))){
            NormalAttack();
        }
        else{
            HeavyAttack();
        }
    }

    void NormalAttack(){

        animator.SetTrigger("ChargeAttack");
        
    }
    void HeavyAttack() {

        animator.SetTrigger("ChargeHeavy");

    }

    void CanAttackCheck()
    {
        if (!canAttack)
        {
            attackCoolDownTimer -= Time.deltaTime;
            if (attackCoolDownTimer <= 0)
            {
                canAttack = true;
                attackCoolDownTimer = 2.7f;
            }
        }
    }

    #endregion

    #region Attack Activations
    void ActivateNormalAttckHitbox(){
        isCharingAttack = false;
        isAttacking = true;
        normalAttackHitbox.SetActive(true);
    }
    
    void ActivateNormalAttackHitbox2(){

        normalAttackHitbox.SetActive(false);
        normalAttackHitbox2.SetActive(true);
    }
    
    void ActivateHeavyAttackHitbox(){
        isCharingAttack = false;
        isHeavyAttacking = true;
        heavyAttackHitbox.SetActive(true);
    }

    void DeactivateNormalAttckHitbox(){
        canAttack = false;
        normalAttackHitbox2.SetActive(false);
        isAttacking = false;
        chase = true;
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
        canAttack = false;
        chase = true;
    }
    #endregion

    #region Block , Dash and Tired

    IEnumerator Dash(float direction){
        animator.SetBool("Dash", true);
        isDashing = true;
        Vector2 dashVector = new(direction * _dashSpeed, 0);
        rigbody.AddForce(dashVector, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);
        isDashing = false;
        animator.SetBool("Dash", false);
    }
    bool IsInDashBoundary(){
        if ((transform.position.x < _dashCheckRight) && (transform.position.x > _dashCheckLeft))
            return true;
        return false;
    }
    void PerformDashWithChecks(){
        if((Mathf.Abs(player.transform.position.x - transform.position.x) <= _canPerformDashDistance) && (IsInDashBoundary()) ) {
            DashProbability();
        }
    }

    void DashProbability()
    {
        int random = Random.Range(1, 101);
        if (random < 19)
            StartCoroutine(Dash(direction));
    }

    void BlockAttack(){ 
        // Animation and make it so that it takes no damage 
        // And then transition from block to either attack or dash back 
    }

    void TiredChecks(){
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }
}
