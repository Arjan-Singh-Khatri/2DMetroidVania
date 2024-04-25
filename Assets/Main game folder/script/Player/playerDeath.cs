using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class playerDeath : MonoBehaviour, IDataPersistance
{
    private Animator anim;
    private Rigidbody2D rig;
    private float health = 100;
    private float damageTakenCoolDown = 0f;
    private bool canTakeDamage = true;

    [SerializeField] Collider2D damageCollider;
    [SerializeField] SpriteRenderer _spriteRenderer;
    // turn collider off when canTakeDamage is off and turn it on when it is off


    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData._health = this.health;
    }

    public void LoadData(GameData gameData)
    {
        this.health = gameData._health;
    }

 
    private void Restartlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void IsHurt(float damage)
    {
        if (!canTakeDamage) return;
        canTakeDamage = false;
        TakeDamage(damage);
        StartCoroutine(HitAnimation());
    }

    IEnumerator ColliderTrigger()
    {
        damageCollider.enabled = false;
        yield return new WaitForSeconds(.8f);
        damageCollider.enabled = true;
    }

    public void TakeDamage(float damage)
    {
        DamageHolder.instance.damageMultiplier = 1;
        DamageHolder.instance.comboNumber = 0;
        StartCoroutine(ColliderTrigger());
        health -= damage;
        if (health <=0)
        {
            rig.bodyType = RigidbodyType2D.Static;
            anim.SetTrigger("death");
        }
    }

    protected IEnumerator HitAnimation()
    {
        Color alpha1 = new Color(255, 2555, 255, 255);
        Color aplha2 = new Color(255, 255, 255, 100);
        Color aplha3 = new Color(255, 255, 255, 170);

        _spriteRenderer.color = aplha2;
        yield return new WaitForSeconds(.5f);
        _spriteRenderer.color = alpha1;
        yield return new WaitForSeconds(.5f);
        _spriteRenderer.color = aplha3;
        yield return new WaitForSeconds(.5f);
        _spriteRenderer.color = alpha1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            rig.bodyType = RigidbodyType2D.Static;
            anim.SetTrigger("death");
        }
        if (collision.gameObject.CompareTag("StromHead"))
        {
            IsHurt(DamageHolder.instance.stromHead);
        }


        //Enemy Attack Colliders
        if (collision.gameObject.CompareTag("Strom"))
            IsHurt(DamageHolder.instance.strom);
        if (collision.gameObject.CompareTag("TwoHeadAttack2"))
            IsHurt(DamageHolder.instance.twoHeadAttackTwoDamage);
        if (collision.gameObject.CompareTag("FireBall"))
            IsHurt(DamageHolder.instance.fireBallDamage);
        if (collision.gameObject.CompareTag("CrabAttack"))
            IsHurt(DamageHolder.instance.crabAttack);
        if (collision.gameObject.CompareTag("SpikedSlimeAttack"))
            IsHurt(DamageHolder.instance.spikedSlimeAttack);


        // Enemies 

        if (collision.gameObject.CompareTag("TwoHead"))
            IsHurt(DamageHolder.instance.twoHeadDamage);
        if (collision.gameObject.CompareTag("Crab"))
            IsHurt(DamageHolder.instance.crab);
        if (collision.gameObject.CompareTag("SpikedSlimeSpikes"))
            IsHurt(DamageHolder.instance.spikdSlimeSpikes);
        if (collision.gameObject.CompareTag("Bat"))
            IsHurt(DamageHolder.instance.batDamage);
        if (collision.gameObject.CompareTag("SpikedSlime"))
            IsHurt(DamageHolder.instance.slimeDamage);
        if (collision.gameObject.CompareTag("Slime"))
            IsHurt(DamageHolder.instance.slimeDamage);

    }
}
