using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;


public class BatEnemy : EnemyParentScript
{
    private Animator anim;
    [SerializeField]
    private float speed = 5f;
    public bool chase = false;
    private Vector2 start_postion;
    private float distance;
    private Rigidbody2D rig;
    [SerializeField]
    private float dash_force = 15;
    [SerializeField]
    private float dashtime = 4f;
    public bool return_tostart = false;
    private float last_dash_time =0f;

    private int healthOfBat = 35;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        start_postion = transform.position;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (healthOfBat <= 0) return; 
        move();
        flip(player);
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= 4)
        {
            attack();
            rig.velocity = Vector2.zero;
        }
    }


    private void move()
    {
        if (chase)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        if (return_tostart)
        {
            transform.position = Vector3.MoveTowards(transform.position, start_postion, speed * Time.deltaTime);    
        }

    }



    private void attack()
    {
        if (Time.time - last_dash_time >= dashtime)
        {
            chase = false;
            anim.SetTrigger("attack");
            last_dash_time = Time.time;
        }
        chase = true;

    }
    public void dash_attack()
    {
        //rig.AddForce(dash_force, ForceMode2D.Impulse);
        // use rig.velocity = ____ and then rig.velocity = vector2.zero
        if (player.transform.position.x < transform.position.x)
        {
            rig.velocity = Vector3.right * dash_force * -1;
        }
        else if (player.transform.position.x >  transform.position.x)
        {
            rig.velocity = Vector3.right * dash_force;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack(collision.gameObject);
            HealthDepleteEnemy(ref healthOfBat);
            if (healthOfBat < 0)
                anim.SetTrigger("death");
        }

    }

}
