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
    private float health = 47;
    private float damageTakenCoolDown = 0f;
    private bool canTakeDamage = true;


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

    private void Update()
    {
        if (health < 1) return;
        if (!canTakeDamage)
        {
            damageTakenCoolDown += Time.deltaTime;
            if (damageTakenCoolDown > 1.3f)
            {
                damageTakenCoolDown = 0f;
                canTakeDamage = true;
            }
        }
    }

    public void Die()
    {
        rig.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death"); 
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
        anim.SetTrigger("hurt");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <1)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Enemy Attack Colliders
        if (collision.gameObject.CompareTag("Strom"))
            Debug.Log("");
        if (collision.gameObject.CompareTag("TwoHeadAttack2"))
            IsHurt(DamageHolder.instance.twoHeadAttackTwoDamage);
        if (collision.gameObject.CompareTag("FireBall"))
            IsHurt(DamageHolder.instance.fireBallDamage);
        if (collision.gameObject.CompareTag("CrabAttack"))
            IsHurt(DamageHolder.instance.crabAttack);
        if (collision.gameObject.CompareTag("SpikedSlimeAttack"))
            IsHurt(DamageHolder.instance.spikedSlimeAttack);
        if (collision.gameObject.CompareTag("SpikedSlimeSpikes"))
            IsHurt(DamageHolder.instance.spikdSlimeSpikes);


        // Enemies 
        if (collision.gameObject.CompareTag("Slime"))
            IsHurt(DamageHolder.instance.slimeDamage);
        if (collision.gameObject.CompareTag("TwoHead"))
            IsHurt(DamageHolder.instance.twoHeadDamage);
        if (collision.gameObject.CompareTag("Crab"))
            IsHurt(DamageHolder.instance.crab);
        if (collision.gameObject.CompareTag("Bat"))
            IsHurt(DamageHolder.instance.batDamage);
        if (collision.gameObject.CompareTag("SpikedSlime"))
            IsHurt(DamageHolder.instance.slimeDamage);
    }
}
