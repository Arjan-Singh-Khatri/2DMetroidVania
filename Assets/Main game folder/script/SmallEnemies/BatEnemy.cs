using UnityEngine;

public class BatEnemy : EnemyParentScript
{    
    // Serialize Fields
    [SerializeField]private float speed = 5f;
    [SerializeField] private float dash_force = 15;
    [SerializeField] private float dashtime = 2f;

    // Private variables
    private Animator anim;
    private Rigidbody2D rig;
    private Vector2 start_postion;
    private float distance;
    private float last_dash_time =2f;

    //Public 
    public bool killed;
    public bool chase = false;
    public bool return_tostart = false;

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
        #region Enemy Death
        if (enemyDead) return;
        if (health <= 0)
        {
            killed = true;
            anim.SetTrigger("death");
            enemyDead = true;
            return;
        }
        #endregion

        Move();
        last_dash_time += Time.deltaTime;
        if (distance <= dashtime)
        {
            Attack();
            
        }
    }

    private void Move()
    {
        flip();
        if (chase)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        if (return_tostart)
        {
            transform.position = Vector3.MoveTowards(transform.position, start_postion, speed * Time.deltaTime);    
        }
        distance = Vector2.Distance(transform.position, player.transform.position);
    }

    private void Attack()
    {
        if (last_dash_time > 2f)
        {
            chase = false;
            anim.SetTrigger("attack");
            last_dash_time = 0f;
        }
        

    }
    private void DashAttack()
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

    private void StopAttack()
    {
        rig.velocity = Vector2.zero;
        chase = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<playerDeath>().TakeDamage(DamageHolder.instance.batDamage);
        }

        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier, ref health);
        }else if (collision.CompareTag("HeavyHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier, ref health);
        }

    }
}
