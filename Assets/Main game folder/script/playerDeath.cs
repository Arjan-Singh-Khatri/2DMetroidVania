using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playerDeath : MonoBehaviour
{
    [SerializeField] Collider2D playerColider;
    private Animator anim;
    private Rigidbody2D rig;
    private int health = 100;


    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        
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
        // Trigger An Animation 
        // Collider is turned off for a time 

    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
}
