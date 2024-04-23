using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private bool _attackPressedAgain;
    public bool isAttacking;
    public bool isHeavyAttacking;
    public bool _canAttack;
    private float _attackDownTime = 1.3f;  

    [SerializeField] GameObject _attackNormalCollider;
    [SerializeField] GameObject _followUpAttackCollider;
    [SerializeField] GameObject _attackHeavyCollider;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileTransform;


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("FollowUp", false);
    }

    // Update is called once per frame
    void Update(){
        
        AttackInputHandler();
        if (_canAttack) {
            _attackDownTime -= Time.deltaTime;
            if(_attackDownTime < 0f) {
                _attackDownTime = 1.2f;
                _canAttack = true;
            }
        }
    }

    #region Attack 

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
        _attackNormalCollider.SetActive(true);
    }

    public void FollowUpAttackCheck(){
        _attackNormalCollider.SetActive(false);
        if (!_attackPressedAgain || anim.GetBool("FollowUp")) {
            isAttacking = false;
            _attackPressedAgain = false;
            return; 
        }
        anim.SetBool("FollowUp", true);
        _followUpAttackCollider.SetActive(true);      
    }

    public void FollowUpAttackEnd()
    {
        isAttacking = false;
        _attackPressedAgain = false;
        anim.SetBool("FollowUp",false);
        _followUpAttackCollider.SetActive(false);
    }

    private void AttackHeavy(){
        // ParticleEffects of Ground Breaking halka 

        isHeavyAttacking = true;
        anim.SetTrigger("Attack2");
        _attackHeavyCollider.SetActive(true);
    }

    public void AttackHeavyEnd(){
        isHeavyAttacking = false;
        _attackHeavyCollider.SetActive(false);
    }

    private void InstantiateProjectile(){
        GameObject instantiatedObejct = Instantiate(projectile, projectileTransform.position, transform.rotation);
    }
    #endregion
}
