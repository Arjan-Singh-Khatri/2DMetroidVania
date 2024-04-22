using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [SerializeField] private float _speed = 10f;
    private const float LIFE_TIME = 2.5f;
    private float localLifeTime = 0f;
    private Rigidbody2D rig;

    private void Start(){
        rig = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector2(.5f, .8f);
    }

    void Update() {
        ScaleOverLifeTime();
        MoveForward();   
    }

    void MoveForward(){
        rig.AddForce(_speed * Vector2.right, ForceMode2D.Force);
    }

    void ScaleOverLifeTime()
    {
        if (localLifeTime >= LIFE_TIME)
            Destroy(gameObject);

        localLifeTime += Time.deltaTime;
    
        float randomScale = 1+ (float)Random.Range(-3, 7)/100;

        if (localLifeTime < LIFE_TIME / 2)
        {
            if (transform.localScale.x >= 2)
                return;
            transform.localScale = new Vector2(transform.localScale.x + 0.01f, randomScale);
        }
        else if (localLifeTime > LIFE_TIME / 2){
            if (transform.localScale.x <= .5)
                return;
            transform.localScale = new Vector2(transform.localScale.x - 0.01f, randomScale);
        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.layer == 7)
            Destroy(gameObject);
    }
}                                              
