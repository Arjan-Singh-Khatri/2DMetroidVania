
using UnityEngine;



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
     



    private void Awake()
    {
        health = 35;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        start_postion = transform.position;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDead) return;
        if (health <= 0)
        {
            anim.SetTrigger("death");
            enemyDead = true;
            return;
        }
        
        move();
        flip();
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= 4)
        {
            Attack();
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



    private void Attack()
    {
        if (Time.time - last_dash_time >= dashtime)
        {
            chase = false;
            anim.SetTrigger("attack");
            last_dash_time = Time.time;
        }
        chase = true;

    }
    public void DashAttack()
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
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref health);
        }

    }

}
