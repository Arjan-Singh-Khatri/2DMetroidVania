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
    #region VARIABLES AND COMPONENTS
    // MAKE AN ENTRY ANIMATION WHERE HE LIKE WAKES UP THEN TRANSITION THAT TO THE IDEL STATE 
    [Header("GET COMPONENTS")]
    private Animator animator;
    private Rigidbody2D rigbody;
    private RaycastHit2D hit;
    private Collider2D[] colliders;

    [Header("MISCELLANEOUS FOR NOW ")]
    [SerializeField] private float circleCastRadius = 3.8f;
    //[SerializeField] private float blockDistance = 4f;
    private bool isDying = false;
    private bool isBlocking = false;
    private bool isTired = false;
    private const float TIRED_TIME = 2.7f;
    private float isTiredTimer;

    [Header("DASH")]
    [SerializeField] private float _dashSpeed = 5f;
    [SerializeField] private float _maxDashThreshold = 10f;
    [SerializeField] private float _minDashThreshold = 0f;
    [SerializeField] private float _dashCheckRight;
    [SerializeField] private float _dashCheckLeft;
    private float _dashProbabilityTimer;
    private bool isDashing = false;

    [Header("MOVEMENT CHASE AND JUMP")]
    //CHASE
    [SerializeField] private float chaseMaxSpeed = 4f;
    [SerializeField] private float chaseMinSpeed = 2f;
    [SerializeField] private float _chaseForce = 10f;
    private Vector2 chaseVector = new();
    private float lerpSpeed = 0f;
    private float chaseSpeed = 0f;
    private float distanceBetween;
    private bool chase = false;

    // JUMP
    [SerializeField] private float _jumpForce = 0f;
    private float _ySpeed;
    private Vector2 _jumpForceVector = new();

    [Header("Attack")]
    [SerializeField] private GameObject normalAttackHitbox;
    [SerializeField] private GameObject heavyAttackHitbox;
    [SerializeField] private GameObject normalAttackHitbox2;

    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private GameObject heavyProjectile;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private Transform heavyProjectileTransform;

    private float attackCoolDownTimer;
    private int normalAttackChoosenCount;
    private int boarderForAttackChoice = 8;
    private bool isCharingAttack;
    private bool isHeavyAttacking;
    private bool isAttacking;
    private bool canAttack = true;

    [Header("Damage")]
    [SerializeField] protected float _damageNormalProjectile = 0f;
    [SerializeField] protected float _damageHeavyProjectile = 0f;
    [SerializeField] protected float _damageNormal = 0f;
    [SerializeField] protected float _damageHeavy = 0f;

    [Header("Refernces")]
    private PlayerAttack _playerAttackScript;
    private playerDeath _playerDeathScript;
    private float playerHealthAfterHit;

    [Header("AI")]
    private const float ORIGINAL_HEALTH = 200; // RANDOM VALUE RIGHT NOW WILL DECIDE LATER
    private bool canChooseBehaviour = true;
    private float canChooseTimer = 2f;
    private bool canChooseTrigger = false;
    private float criticalHealth;
    private float _damageTaken = 0f;

    [Header("Probabilities For Attack")]
    private float probForNormal = 0f;
    private float probForHeavy = 0f;
    private float probForChoiceBlock = 0f;
    #endregion

    void Start() {
       
        // References
        player = GameObject.FindGameObjectWithTag("Player");
        _playerAttackScript = player.GetComponent<PlayerAttack>();
        _playerDeathScript = player.GetComponent<playerDeath>();
        playerHealthAfterHit = _playerDeathScript.health - _damageNormalProjectile;

        // GameObject components
        animator = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();

        // For boss logic
        health = ORIGINAL_HEALTH;
        criticalHealth = ORIGINAL_HEALTH/5; // Twenty Percent Of the Original Health

    }

    void Update() {

        if (isDying)
            return;
        // JUST ALL THE CALCULATIONS AND PHYSICS CAST 
        flip();
        colliders = Physics2D.OverlapCircleAll(transform.position, circleCastRadius);

        distanceBetween = Mathf.Abs(player.transform.position.x - transform.position.x);

        //AvoidProjectilesAttack();
        return;

        if (chase)
        {
            CalculateChaseVector();
            Chase();
        }
            

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
        
        if (isCharingAttack || isAttacking || isHeavyAttacking || isBlocking || isTired || isDashing) return;
        
        // Enemy Flip and Determine Probabilities
        flip();
        DetermineProbabilities();

        // WHEN PLAYER IS IN ATTACK DISTANCE
        var randomVar = Random.Range(0,101) ;
        
        foreach(var col in colliders) { 
            if(col.name == player.name) 
            {
                if (randomVar <= probForChoiceBlock)
                    StartCoroutine(BlockAttack());
                else
                    ChooseAttack();
                canChooseBehaviour = false;
            }
        }

        // CHOOSING WHAT TO DO BY ENEMY LONG RANGE ATTACKS AND CHASE BEHAVIOURS
        if (!canChooseBehaviour) return;

        canChooseBehaviour = false;

        // Dash Away left to do If needed IG

        if (randomVar <= probForNormal) {
            Debug.Log("NormalB");
        }
        else if (randomVar > probForNormal && randomVar <= probForHeavy) {
            Debug.Log("HeavyB");
        }
        else if(randomVar > probForHeavy)
            chase = true;

    }

    [ContextMenu("CANCHOOSETOGGLE")]
    void CanChooseToggle() {
        canChooseTrigger = true;
    }

    void CanChooseBehaviourTimer()
    {
        if (canChooseBehaviour) return;
        canChooseTimer -= Time.deltaTime;
        if (canChooseTimer < 0)
        {
            canChooseTimer = 4f;
            canChooseBehaviour = true;
            canChooseTrigger = false;
        }
    }

    private void DetermineProbabilities() {
        if (BeAggresive()) {
            probForChoiceBlock = 40f;
            probForNormal = 30f;
            probForHeavy = 60f;
        }else if (BeDefensive()) {
            probForChoiceBlock = 60f;
            probForNormal = 30f;
            probForHeavy = 80f;
        }
    }

    private void DashAway() { 
        // AFTER SOME BEHAVIOUR DASH AWAY IF THERE IS LESS THAN SOME THRESHOLD DISTANCE BETWEEN VAGABOND AND THE PLAYER
    }
    #endregion

    #region Aggresion and Defensive Behaviour

    bool BeAggresive()
    {
        // BASE CONDITION 
        if ((health > criticalHealth && !HitCauseCriticalHealth()) || DefenseiveOverride())
            return true;
        
        return false;
    }

    bool BeDefensive()
    {
        // BASE CONDITION
        if ((health < criticalHealth || HitCauseCriticalHealth()) && !DefenseiveOverride())
            return true;
        
        return false;
    }
    
    bool DefenseiveOverride() {

        if (playerHealthAfterHit <= 30)
            return true;
        return false;
    }

    bool HitCauseCriticalHealth()
    {
        if (playerHealthAfterHit <= criticalHealth)
            return true;
        return false;
    }


    #endregion

    #region MOVEMENTS
    void Chase(){
        _dashProbabilityTimer -= Time.deltaTime;
        if( _dashProbabilityTimer < 0){
            _dashProbabilityTimer = 2.5f; 
            PerformDashWithChecks(true);
        }

        if (animator.GetBool("Run") == false)
            animator.SetBool("Run", true);

        rigbody.AddForce(chaseVector, ForceMode2D.Force);
        
        // Timer maybe just to make sure it doesn't keep chasing for too long
    }

    void CalculateChaseVector() {
        lerpSpeed = distanceBetween / 35;
        chaseSpeed = Mathf.Lerp(chaseMinSpeed, chaseMaxSpeed, lerpSpeed);
        chaseVector = new(chaseSpeed * direction * _chaseForce, 0f);
    }

    void Jump() {
        if (chase)
            _jumpForceVector = new(chaseVector.x, _jumpForce);
        else
            _jumpForceVector = new(0f, _jumpForce);
        // Addforce to the rigidBody
        // ANIMATION --set value of ySpeed for the blend tree

    }

    void AvoidProjectilesAttack()
    {
        if (isCharingAttack || isAttacking || isHeavyAttacking || isBlocking || isTired || isDashing) return;

        if (hit.collider.gameObject.CompareTag("PlayerProjectile"))
            Jump();
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

    #region Attack INSTANTIATIONS BAD CODE

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
        CanChooseToggle();

    }

    void DeactivateHeavyAttackHitbox(){
        animator.SetBool("ChargeHeavy", false);
        heavyAttackHitbox.SetActive(false);
        isHeavyAttacking= false;
        CanChooseToggle();
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
            if ((Mathf.Abs(player.transform.position.x - transform.position.x) <= _maxDashThreshold) 
                && (Mathf.Abs(player.transform.position.x - transform.position.x) >= _minDashThreshold) && (IsInDashBoundary()))
            {
                DashProbability();
            }
        }else{
            if ((Mathf.Abs(player.transform.position.x - transform.position.x) <= _maxDashThreshold)
                && (Mathf.Abs(player.transform.position.x - transform.position.x) >= _minDashThreshold) && (IsInDashBoundary()))
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
        if (hit.collider.name == player.name && _playerAttackScript.isAttacking == false){
            var randomVar = Random.Range(0, 101);
            if(randomVar < 90)
                ChooseAttack();
        }else {
            CanChooseToggle();
        }

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

    #region OTHER STUFF

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collison {collision.gameObject.name}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBlocking) return;

        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
            TakeDamage(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier);
        else if (collision.gameObject.CompareTag("HeavyHitBox"))
            TakeDamage(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier);
        else if (collision.gameObject.CompareTag("PlayerProjectile"))
            TakeDamage(DamageHolder.instance.playerHeavyDamage);

        if (health < 0)
            Die();      
    }

    private void OnDrawGizmos(){

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }

    #endregion
}
