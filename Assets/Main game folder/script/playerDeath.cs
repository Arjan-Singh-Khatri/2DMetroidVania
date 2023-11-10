using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Cinemachine;

public class playerDeath : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;
    private float health = 100;


    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        
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

    private void IsHurt(int damage)
    {
        TakeDamage(damage);
        anim.SetTrigger("hurt");
        //Down Time for Getting Hurt Again i.e Collider is OFF for a second or two
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
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
        if (collision.gameObject.CompareTag("Slime"))
            TakeDamage(DamageHolder.instance.slimeDamage);
        if (collision.gameObject.CompareTag("TwoHead"))
            TakeDamage(DamageHolder.instance.twoHeadDamage);
        if(collision.gameObject.CompareTag("Bat"))
            TakeDamage(DamageHolder.instance.batDamage);
        if (collision.gameObject.CompareTag("TwoHeadAttack2"))
            TakeDamage(DamageHolder.instance.twoHeadAttackTwoDamage);
        if (collision.gameObject.CompareTag("Crab"))
            TakeDamage(DamageHolder.instance.crab);
    }
}
