using Mono.Cecil;
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
    private bool isDying = false;
    private bool isBlocking = false;
    private bool isTired = false;
    private const float TIRED_TIME = 2.7f;
    private float isTiredTimer;
    private bool isHurt = false;

    [Header("DASH")]
    [SerializeField] private float _dashSpeed = 5f;
    [SerializeField] private float _canPerformDashDistance;
    [SerializeField] private float _dashCheckRight;
    [SerializeField] private float _dashCheckLeft;
    private float _dashProbabilityTimer;
    private bool isDashing = false;


    [Header("CHASE / FOLLOW")]
    [SerializeField] private float chaseMaxSpeed = 4f;
    [SerializeField] private float chaseMinSpeed = 2f;
    [SerializeField] private float _chaseForce = 10f;
    private float distanceBetween;
    private bool chase = true;

    [Header("Attack")]
    [SerializeField] private GameObject normalAttackHitbox;
    [SerializeField] private GameObject heavyAttackHitbox;
    [SerializeField] private GameObject normalAttackHitbox2;

    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private GameObject heavyProjectile;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private Transform heavyProjectileTransform;

    [SerializeField] protected float _damageNormal = 0f;
    [SerializeField] protected float _damageHeavy = 0f;

    private float attackCoolDownTimer;
    private int normalAttackChoosenCount;
    private int boarderForAttackChoice = 8;
    private bool isCharingAttack;
    private bool isHeavyAttacking;
    private bool isAttacking;
    private bool attackDowntime;
    private bool canAttack = true;


    [Header("Refernces")]
    private PlayerAttack script;

    [Header("AI")]
    private bool canChooseBehaviour = true;
    private float canChooseTimer = 2f;
    private bool canChooseTrigger = false;
    private float damageTaken = 0f;

    void Start() {
       
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
        health = 200f;
        script = player.GetComponent<PlayerAttack>();
    }

    void Update() {

        if (true)
            return;
        if (isDying)
            return;

        PhysicsAndCalcs();

        if (chase)
            Chase();

        // TIMER FOR BEHAVIOUR CHECK
        CanAttackCheck();

        // ENEMY STUNNED 
        TiredChecks();
        
        // ENEMY BEHAVIOUR 
        EnemyAI();

        if (canChooseTrigger)  
            CanChooseBehaviourTimer();
    }

    #region Enemy Behaviour
    void EnemyAI() {
        if (isCharingAttack || isAttacking || isHeavyAttacking || isBlocking || isTired || isDashing || isHurt) return;
        flip();
        var randomVar = Random.Range(0, 101);
        // Enemy Attack Patterns and Chase/ Dash perform logic
        if (hit.collider.name == player.name) {
            if (CanBeAggresive())
                Debug.Log("Attacked Or Blocked");
        }

        if (!canChooseBehaviour) return;

        AvoidProjectilesAttack();
        canChooseBehaviour = false;

        // Dash Away if 
        //if(damageTaken >= health/10)
        //    StartCoroutine(Dash(-direction));

        if (randomVar >= 0 && randomVar < 30) {
            animator.SetTrigger("NormalAttackB");
        }
        else if (randomVar >= 30 && randomVar <= 60) {
            animator.SetTrigger("HeavyAttackB");
        }
        else
            chase = true;

    }

    void AvoidProjectilesAttack(){
        // This one needes to be tested very properly !!
    }

    bool CanBeAggresive(){
        // based on current health and player health and player combo multiplyer
        // if player can be do damage enough to be get enemy health to a threshold health then no aggresive
        // if player health if low then agressive 
        // if both health lower than threshold then aggresive - the one to hit wins 
        
        return false;
    }

    void CanChooseBehaviourTimer() {
        if(!canChooseBehaviour) return;
        canChooseTimer -= Time.deltaTime;
        if(canChooseTimer < 0) {
            canChooseTimer =2 ;
            canChooseBehaviour = true;
            canChooseTrigger = false;
        }
    }

    void CanChooseToggle() {
        canChooseTrigger = true;
    }
    #endregion

    #region Chase
    void Chase(){
        _dashProbabilityTimer -= Time.deltaTime;
        if( _dashProbabilityTimer < 0){
            _dashProbabilityTimer = 2.5f; 
            PerformDashWithChecks(true);
        }

        if (animator.GetBool("Run") == false)
            animator.SetBool("Run", true);

        float lerpSpeed = distanceBetween / 35;
        float chaseSpeed = Mathf.Lerp(chaseMinSpeed, chaseMaxSpeed, lerpSpeed) ;
        Vector2 chaseVector = new(chaseSpeed * direction * _chaseForce , 0f);

        rigbody.AddForce(chaseVector , ForceMode2D.Force);
        
        // Timer maybe just to make sure it doesn't keep chasing for too long
    }

    #endregion

    #region Attack
    void ChooseAttack(){

        if (!canAttack) return;

        isCharingAttack = true;
        chase = false;
        int randomNumber = Random.Range(0, 10);

        animator.SetBool("Run", false);


        if (randomNumber < boarderForAttackChoice) {
            normalAttackChoosenCount += 1;
            if (normalAttackChoosenCount > 4)
                boarderForAttackChoice--;
            NormalAttack();
            
        }else{
            boarderForAttackChoice = 7;
            normalAttackChoosenCount = 0;
            HeavyAttack();
        }
    }

    void NormalAttack(){

        animator.SetBool("ChargeAttack",true);
    }

    void HeavyAttack()
    {
        // PARTICLE EFFECTS 
        animator.SetBool("ChargeHeavy", true);
    }

    void CanAttackCheck()
    {
        if (!canAttack)
        {
            attackCoolDownTimer -= Time.deltaTime;
            if (attackCoolDownTimer <= 0)
            {
                canAttack = true;
                attackCoolDownTimer = 1.7f;
            }
        }
    }

    #endregion

    #region Attack BAD CODE

    [ContextMenu("Normal")]
    void NormalAttackInstantiation() {
        
        Instantiate(normalProjectile, projectileTransform.position, Quaternion.identity);
    }

    [ContextMenu("Heavy")]
    void HeavyAttackInstantiation() { 
        Instantiate(heavyProjectile, heavyProjectileTransform.position, Quaternion.identity);
    }

    void NormalInstantiationWithProb() {
        var randomNumber = Random.Range(0, 101);
        if(randomNumber < 27)
            Instantiate(normalProjectile, projectileTransform.position, Quaternion.identity);
    }

    void HeavyInstantiationWithProb()
    {
        var randomNumber = Random.Range(0, 101);
        if (randomNumber < 33)
            Instantiate(heavyProjectile, heavyProjectileTransform.position, Quaternion.identity);
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
        animator.SetBool("ChargeAttack", false);
        canAttack = false;
        normalAttackHitbox2.SetActive(false);
        isAttacking = false;

    }

    void DeactivateHeavyAttackHitbox(){
        animator.SetBool("ChargeHeavy", false);
        heavyAttackHitbox.SetActive(false);
        isHeavyAttacking= false;
        //isTired = true;
        //isTiredTimer = TIRED_TIME;
        //animator.SetBool("Tired", true);
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
    void PerformDashWithChecks(bool prob){
        if (prob){
            if ((Mathf.Abs(player.transform.position.x - transform.position.x) <= _canPerformDashDistance) && (IsInDashBoundary()))
            {
                DashProbability();
            }
        }else{
            if ((Mathf.Abs(player.transform.position.x - transform.position.x) <= _canPerformDashDistance) && (IsInDashBoundary()))
            {
                StartCoroutine(Dash(direction));
            }
        }
    }

    void DashProbability()
    {
        int random = Random.Range(1, 101);
        if (random < 19){
            StartCoroutine(Dash(direction));
        }
    }

    IEnumerator BlockAttack(){
        chase = false;
        animator.SetBool("Run", false); 
        isBlocking = true;
        if (animator.GetBool("Block") == false)
            animator.SetBool("Block", true);

        yield return new WaitForSeconds(Random.Range(2, 5.5f));
        animator.SetBool("Block", false);
        isBlocking = false;
        if (hit.collider.name == player.name){
            int randomNumber = Random.Range(1, 101);
            if (randomNumber < 35)
                ChooseAttack();
        }
        CanChooseToggle();
        /*  -- if player is hitting then right after player stops then attack
            -- take a reference of the player script and check if the attack is in downtime
         */
    }

    void Tired() { 
    
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

    void TiredStateEnd()
    {
        isTired = false;
        animator.SetBool("Tired", false);
        canAttack = false;

    }


    #endregion

    #region DEATH And Damage
    private void TakeDamage(float Damage){
        isHurt = true;
        if (isTired)
            HealthDepleteEnemy(Damage * DamageHolder.instance.damageMultiplier, ref health);
        else
            HealthDepleteEnemy(Damage, ref health);

        //StartCoroutine(HitAnimation());
        if (health <= 0)
            Die();
    }

    
    void Die()
    {
        isDying = true;
        animator.SetTrigger("Death");
        gameObject.GetComponent<Collider2D>().enabled = false;
        rigbody.bodyType = RigidbodyType2D.Static;
    }

    void Destroy()
    {
        Destroy(gameObject);    
    }
    #endregion

    void PhysicsAndCalcs()
    {
        hit = Physics2D.CircleCast(transform.position, circleCastRadius, new Vector2(direction, 0));
        distanceBetween = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void OnDrawGizmos(){

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBlocking) return;
        Debug.Log("Damage Taken");
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
            TakeDamage(DamageHolder.instance.playerDamage *  DamageHolder.instance.damageMultiplier);
        else if (collision.gameObject.CompareTag("HeavyHitBox"))
            TakeDamage(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier);

        if (health < 0)
            Die();
            
    }
}
