using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour, IDataPersistance
{
    private Animator anim;
    public bool _attackPressedAgain = false;
    public bool isAttacking;
    public bool isHeavyAttacking;
    public bool _canAttack ;
    private float _attackDownTime = 1.3f;  

    [SerializeField] GameObject _attackNormalCollider;
    [SerializeField] GameObject _followUpAttackCollider;
    [SerializeField] GameObject _attackHeavyCollider;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileTransform;
    [SerializeField] int baseChargeNumber = 3;


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("FollowUp", false);
        _canAttack = true;
        _attackPressedAgain = false;
    }

    // Update is called once per frame
    void Update(){

        AttackInputHandler();
        AttackDownTime();
    }

    #region Save System
    public void LoadData(GameData gameData)
    {
        this.transform.position = gameData._position;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData._position = this.transform.position;
    }

    #endregion

    #region Attack 

    private void AttackDownTime()
    {
        if (!_canAttack)
        {
            _attackDownTime -= Time.deltaTime;
            if (_attackDownTime < 0f)
            {
                _attackDownTime = .7f;
                _attackPressedAgain = false;
                isAttacking = false;
                _canAttack = true;
            }
        }
    }

    private void AttackInputHandler() {
        if (!_canAttack) return;
        var playerMovement = GetComponent<PlayerMovement>();
        if (Input.GetKeyDown(KeyCode.C) && !_attackPressedAgain)
        {
            if (isAttacking)
            {
                _attackPressedAgain = true;
                _canAttack = false;
            }
            else
                Attack();
        }
        else if (Input.GetKeyDown(KeyCode.Z) && !isAttacking && !playerMovement.IsDashing && !playerMovement.IsSliding 
            && !playerMovement.IsJumping
            && !playerMovement.IsWallJumping
            ){

            AttackHeavy();
        }
    }

    private void Attack(){
        isAttacking = true;
        anim.SetTrigger("attack");
    }

    private void ColliderActivations()
    {
        _attackNormalCollider.SetActive(true);
    }

    private void AttackFollowActivation() {

        _followUpAttackCollider.SetActive(true);
    }

    private void DeactivateNormal()
    {
        _attackNormalCollider.SetActive(false);
    }

    private void DeactivateFollowUp() {

        _followUpAttackCollider.SetActive(false);
    }

    public void FollowUpAttackCheck(){
        if (_attackPressedAgain)
            anim.SetBool("FollowUp", true);
        else
        {
            isAttacking=false; ;
            _attackPressedAgain=false;
        }
    }

    public void FollowUpAttackEnd()
    {
        isAttacking = false;
        _attackPressedAgain = false;
        anim.SetBool("FollowUp",false);
    }

    private void AttackHeavy(){
        // ParticleEffects of Ground Breaking halka
        if (!EvaluateCharge()) return; 
        isHeavyAttacking = true;
        _canAttack = false;
        anim.SetTrigger("Attack2");
    }

    private bool EvaluateCharge() {
        if (DamageHolder.instance.playerCharge >= baseChargeNumber){
            DamageHolder.instance.playerCharge -= baseChargeNumber;
            return true;
        }
        return false;

    }

    private void ActivationHeavy()
    {
        _attackHeavyCollider.SetActive(true);
    }

    private void DeactivateHeavy() { 
    _attackHeavyCollider.SetActive(false);}

    public void AttackHeavyEnd(){
        isHeavyAttacking = false;
        _attackHeavyCollider.SetActive(false);
    }

    void HurtWhileAttacking(){
        isAttacking = false;
        anim.SetBool("FollowUp", false);
        _attackPressedAgain = false;
        DeactivateFollowUp();
        DeactivateHeavy();
        DeactivateNormal();
    }

    private void InstantiateProjectile(){
        GameObject instantiatedObejct = Instantiate(projectile, projectileTransform.position, transform.rotation);
    }
    #endregion
}
