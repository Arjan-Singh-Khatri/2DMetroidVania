using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playerDeath : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;
    private Collider2D col;


    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trap") || collision.gameObject.CompareTag("enemy"))
        {

            Die();
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

   
    
}
