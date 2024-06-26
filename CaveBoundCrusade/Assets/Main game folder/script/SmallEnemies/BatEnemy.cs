
using UnityEngine;

public class BatEnemy : EnemyParentScript
{    
    // Serialize Fields
    [SerializeField]private float speed = 5f;
    [SerializeField] private float dash_force = 15;
    [SerializeField] private float dashMinDistance = 2f;

    // Private variables
    private Animator anim;
    private Rigidbody2D rig;
    private Vector2 start_postion;
    private float distance;
    private float last_dash_time =2f;

    //Public 
    public bool chase = false;
    public bool return_tostart = false;
    public bool _killed = false;

    [SerializeField] private AudioClip _batAttack;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = mixerGroup;
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
            _killed = true;
            anim.SetTrigger("death");
            enemyDead = true;
            return;
        }
        #endregion

        Move();

        last_dash_time += Time.deltaTime;

        if (distance <= dashMinDistance)
        {
            Attack();
            
        }
    }

    private void Move()
    {
       
        if (chase)
        { 
            flip();
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
        if (last_dash_time > 3f)
        {
            chase = false;
            anim.SetBool("attack",true);
            last_dash_time = 0f;
        }
        

    }

    private void AttackAudio() {
        _audioSource.PlayOneShot(_batAttack);
    }

    private void DashAttack()
    {
        //rig.AddForce(dash_force, ForceMode2D.Impulse);
        // use rig.velocity = ____ and then rig.velocity = vector2.zero
        if (player.transform.position.x < transform.position.x)
        {
            rig.velocity = -1 * dash_force * Vector3.right;
        }
        else if (player.transform.position.x >  transform.position.x)
        {
            rig.velocity = Vector3.right * dash_force;
        }
    }

    private void StopAttack()
    {
        anim.SetBool("attack", false);
        rig.velocity = Vector2.zero;
        chase = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (chase)
                Events.instance.onPlayerTakeDamage(DamageHolder.instance.batDamage);
            else
                Events.instance.onPlayerTakeDamage(DamageHolder.instance.batDamage * 1.25f);
        }

        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier, ref health);
        }else if (collision.CompareTag("HitBoxHeavy"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier, ref health);
        }

    }
}
