using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour
{

    private Animator anim;
    [SerializeField]
    private float bounceforce = 10f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<Collider2D>().IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>()))
        {
            Vector2 direction = Vector2.up;
            Rigidbody2D rig = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            rig.AddForce(direction * bounceforce, ForceMode2D.Impulse);
        }
    }
}
