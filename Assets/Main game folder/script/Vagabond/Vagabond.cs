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
    // TO DO - MAKE AN ENTRY ANIMATION WHERE HE LIKE WAKES UP THEN TRANSITION THAT TO THE IDEL STATE 

    [Header("GET COMPONENTS")]
    private Animator animator;
    private Rigidbody2D rigbody;
    private Collider2D[] colliders;

    [Header("MISCELLANEOUS FOR NOW ")]
    [SerializeField] private float circleCastRadius = 3.8f;
    private bool isDying = false;
    private bool isBlocking = false;
    private bool isTired = false;
    private const float TIRED_TIME = 2.7f;
    private float isTiredTimer;
    // FOR PROJECTILES
    protected float directionForProjectile;

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
    public bool chase = false;
    private float distanceBetween;
    private float lerpSpeed = 0f;
    private float chaseSpeed = 0f;

    // JUMP
    [SerializeField] private float _jumpForce = 0f;
    private bool isJumping = false;
    private Vector2 _jumpForceVector = new();
    private bool toggleChase = false;

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
        playerHealthAfterHit = _playerDeathScript.health - _damageNormal;

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

        // TRYING TO MAKE THINGS LOOK CLEAN
        Math();
        // IF CHASE THEN CHASE THE PLAYER
        CalculateChaseVector();
        if (chase){
            Chase();
        }
        // TRY TO AVOID PLAYER PROJECTILE
        AvoidProjectiles();
        // ENEMY BEHAVIOUR 
        EnemyBehaviour();
        // TIMER TOO SEE IF THE VAGABOND CAN ATTACK AGAIN
        CanAttackCheck();
        // TIMER TO END STUNNED STATE OF VAGABOND
        StunnedTimer();
        // TO TRIGGER TIMER FOR VAGABOND TO CHOOSE BEHAVIOUR
        if (canChooseTrigger)  
            CanChooseBehaviourTimer();
    }

    #region Enemy Behaviour
    void EnemyBehaviour() {

        if (isCharingAttack || isAttacking || isHeavyAttacking || isTired || isDashing || isJumping) return;
        else if (isBlocking) { 
            flip();
            return;
        }
        
        // Enemy Flip and Determine Probabilities
        flip();
        DetermineProbabilities();

        // WHEN PLAYER IS IN ATTACK OR BLOCK DISTANCE
        var randomVar = Random.Range(0,101) ;

        if (IsPlayerInRange()) {
            if (randomVar <= probForChoiceBlock)
                StartCoroutine(BlockAttack());
            else
                ChooseAttack();
            canChooseBehaviour = false;
        }

        // CHOOSING WHAT TO DO BY ENEMY LONG RANGE ATTACKS AND CHASE BEHAVIOURS
        if (!canChooseBehaviour && isJumping) return;

        canChooseBehaviour = false;

        // Dash Away ???

        if (randomVar <= probForNormal){
            animator.SetTrigger("NormalAttackB");
        }
        else if (randomVar > probForNormal && randomVar <= probForHeavy){
            animator.SetTrigger("HeavyAttackB");
        }
        else if (randomVar > probForHeavy)
            chase = true;

    }

    void CanChooseToggle() {
        canChooseTrigger = true;
    }

    void CanChooseBehaviourTimer()
    {
        if (canChooseBehaviour) return;
        canChooseTimer -= Time.deltaTime;
        if (canChooseTimer < 0)
        {
            canChooseTimer = 2f;
            canChooseBehaviour = true;
            canChooseTrigger = false;
        }
    }

    private void DetermineProbabilities() {
        if (BeAggresive()) {
            probForChoiceBlock = 20f;
            probForNormal = 30f;
            probForHeavy = 60f;
        }else if (BeDefensive()) {
            probForChoiceBlock = 40f;
            probForNormal = 30f;
            probForHeavy = 80f;
        }
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

    #region MOVEMENT
    void Chase(){

        if (animator.GetBool("Run") == false)
            animator.SetBool("Run", true);

        _dashProbabilityTimer -= Time.deltaTime;
        if( _dashProbabilityTimer < 0){
            _dashProbabilityTimer =Random.Range(1.1f,1.5f); 
            PerformDashWithChecks(true);
        }
        rigbody.AddForce(chaseVector, ForceMode2D.Force);
        
        // Timer maybe just to make sure it doesn't keep chasing for too long
    }

    void CalculateChaseVector() {
        lerpSpeed = distanceBetween / 35;
        chaseSpeed = Mathf.Lerp(chaseMinSpeed, chaseMaxSpeed, lerpSpeed);
        chaseVector = new(chaseSpeed * direction * _chaseForce, 0f);
    }

    void Jump() {
        isJumping = true;
        animator.SetBool("Jump", true);
        if (chase) { 
            _jumpForceVector = new(chaseVector.x, _jumpForce);
            animator.SetBool("Run", false);
            chase = false;
            toggleChase = true;
        }
        else
            _jumpForceVector = new(0f, _jumpForce);
        rigbody.AddForce(_jumpForceVector, ForceMode2D.Impulse);
    }

    void AvoidProjectiles(){

        SetJumpFalse();
        
        if (isCharingAttack || isAttacking || isHeavyAttacking || isBlocking || isTired || isDashing) return;

        animator.SetFloat("ySpeed", rigbody.velocity.y);
        if (IsProjectileInRange() && !animator.GetBool("Jump")){
            Jump();
        }
    }

    void SetJumpFalse() {
        if ( (rigbody.velocity.y < -10f || rigbody.velocity.y == 0) && animator.GetBool("Jump")) { 
            animator.SetBool("Jump", false);
            isJumping = false;
            if (toggleChase) { 
                chase = true;
                toggleChase = false;
            }
        }
    }

    private bool IsPlayerInRange()
    {
        foreach (var col in colliders)
        {
            if (col.name == player.name)
                return true;
        }
        return false;
    }

    private bool IsProjectileInRange()
    {

        foreach (var col in colliders)
        {
            if (col.tag.CompareTo("PlayerProjectile") == 0)
                return true;
        }
        return false;
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
                attackCoolDownTimer = .7f;
            }
        }
    }

    #endregion

    #region Attack INSTANTIATIONS BAD CODE

    void NormalAttackInstantiation() {
        
        Instantiate(normalProjectile, projectileTransform.position, Quaternion.identity);
    }

    void HeavyAttackInstantiation() { 
        Instantiate(heavyProjectile, heavyProjectileTransform.position, Quaternion.identity);
    }

    void NormalInstantiationWithProb() {
        var randomNumber = Random.Range(0, 101);
        if(randomNumber < 15)
            Instantiate(normalProjectile, projectileTransform.position, Quaternion.identity);
    }

    void HeavyInstantiationWithProb()
    {
        var randomNumber = Random.Range(0, 101);
        if (randomNumber < 15)
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

    #region Block , Dash and Stunned

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

    IEnumerator BlockAttack() {
        chase = false;
        animator.SetBool("Run", false);
        isBlocking = true;
        if (animator.GetBool("Block") == false)
            animator.SetBool("Block", true);

        yield return new WaitForSeconds(Random.Range(2, 3.5f));

        animator.SetBool("Block", false);
        isBlocking = false;

         
        if(IsPlayerInRange() && !_playerAttackScript.isAttacking) {
            var randomVar = Random.Range(0, 101);
            if (randomVar < 90)
                ChooseAttack();
        }
        else{
            CanChooseToggle();
        }   
    }

    void Stunned() {
        isTired = true;
        _damageTaken = 0;

        // Animation play and Timer to take him out of the tired state and
        // Make other things false like chasing and attacking
        // Be careful
    }

    void StunnedTimer(){
        if (isTired)
        {
            isTiredTimer -= Time.deltaTime;
            if (isTiredTimer <= 0f)
            {
                isTiredTimer = TIRED_TIME;
                StunnedStateEnd();
            }
        }
    }

    void StunnedStateEnd()
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
        else {
            _damageTaken += Damage * DamageHolder.instance.damageMultiplier;
            HealthDepleteEnemy(Damage, ref health);
        }
        if (health < 1)
            Die();
    }

    void Die(){
        isDying = true;
        animator.SetTrigger("Death");
        gameObject.GetComponent<Collider2D>().enabled = false;
        rigbody.bodyType = RigidbodyType2D.Static;
    }

    void Destroy(){
        Destroy(gameObject);    
    }
    #endregion

    #region OTHER STUFF
    void Math(){
        directionForProjectile = direction;
        colliders = Physics2D.OverlapCircleAll(transform.position, circleCastRadius);
        distanceBetween = Mathf.Abs(player.transform.position.x - transform.position.x);

    }

    private void OnTriggerEnter2D(Collider2D collision){
        
        // PLAYER TAKES DAMAGE IF HIT BY A DASH
        if (isDashing && collision.gameObject.CompareTag("Player"))
            _playerDeathScript.TakeDamage(_damageNormal);

        // IF BLOCKING IGNORE COLLISIONS 
        if (isBlocking) return;

        // VAGABOND TAKING DAMAGE
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
            TakeDamage(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier);
        else if (collision.gameObject.CompareTag("HeavyHitBox"))
            TakeDamage(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier);
        else if (collision.gameObject.CompareTag("PlayerProjectile"))
            TakeDamage(DamageHolder.instance.playerHeavyDamage);
        
    }

    private void OnDrawGizmos(){
        
        //GIZMOS FOR OVERLAP CIRCLE
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }
    #endregion

}
