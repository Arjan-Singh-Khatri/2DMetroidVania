using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [SerializeField] private float _speed = 10f;
    private const float LIFE_TIME = 1.9f;
    private float localLifeTime = 0.1f;
    private Rigidbody2D rig;

    private void Start(){
        rig = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector2(.1f, .8f);
    }

    void Update() {
        ScaleOverLifeTime();
        MoveForward();   
    }

    void MoveForward(){
        transform.position += _speed * Time.deltaTime * Vector3.right ;
    }

    void ScaleOverLifeTime()
    {
        if (localLifeTime >= LIFE_TIME)
            Destroy(gameObject);

        localLifeTime += Time.deltaTime;
        float offset = Mathf.Lerp(2f, .1f, localLifeTime / LIFE_TIME);

        float randomScale = 1+ (float)Random.Range(-3, 7)/10;

        if (localLifeTime < LIFE_TIME / 2)
        {
            transform.localScale = new Vector2(offset, randomScale);
        }
        else if (localLifeTime > LIFE_TIME / 2){
            transform.localScale = new Vector2(offset, randomScale);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.layer == 7)
            Destroy(gameObject);
    }
}                                              
