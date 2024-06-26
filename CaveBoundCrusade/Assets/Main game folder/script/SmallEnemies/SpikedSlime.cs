using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedSlime : EnemyParentScript
{
    [SerializeField] GameObject spikes;
    [SerializeField] Transform spikesPosition;
    [SerializeField] GameObject slimeCollider;
    [SerializeField] private float speed = 1f;
    private Vector2 followPosition;
    private Animator animator;
    private float attackDownTime = 0;
    private bool isAttacking = false;
    private float startPoint;

    [SerializeField] AudioClip attackAudio;
    [SerializeField] AudioClip landingAudio;
    [SerializeField] AudioClip jumpingAudio;

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = mixerGroup;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        startPoint = transform.position.x;

    }


    // Update is called once per frame
    void Update()
    {

        if (health <= 0) return;
        
        // Safety 
        if(isAttacking && attackDownTime >= 2.5f)
            AttackEnd();    


        FollowPlayer();

        if (Mathf.Abs(transform.position.x- player.transform.position.x) <= 3f 
            && (player.transform.position.y-transform.position.y) < 5 && (player.transform.position.y - transform.position.y >-1) 
            && attackDownTime >= 2.5f)
        {
            AttackPlayer();
        }

        if (attackDownTime < 2.5f ) { attackDownTime += Time.deltaTime; }

    }

    private void FollowPlayer()
    {
        flip();
        if (PlayerInsideRange() && !isAttacking)
        {
            animator.SetBool("Run", true);
            followPosition.x = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime).x;
            followPosition.y = transform.position.y;
            transform.position = followPosition;
        }


    }

    private bool PlayerInsideRange()
    {
        if (Mathf.Abs(player.transform.position.x - startPoint) < 4f)
            return true;
        return false;
    }

    private void AttackPlayer()
    {
        animator.SetBool("Run", false);
        attackDownTime = 0f;
        isAttacking = true;
        slimeCollider.SetActive(true);
        ChosseAttack();
    }


    private void ChosseAttack()
    {

        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                animator.SetTrigger("Ability");
                break;
            case 2:
                animator.SetTrigger("Attack");
                break;
        }

    }

    private void TriggerAbility()
    {

        Instantiate(spikes,spikesPosition.position,transform.rotation);
    }

    private void AttackEnd()
    {
        
        slimeCollider.SetActive(false);
        isAttacking = false;
    }

    private void AttackAudio() {
        _audioSource.PlayOneShot(attackAudio);
    }

    private void LandingAudio(){
        _audioSource.PlayOneShot(landingAudio);
    }

    private void JumpingAudio() {
        _audioSource.PlayOneShot(jumpingAudio);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //player.GetComponent<playerDeath>().TakeDamage(DamageHolder.instance.spikedSlime);
            Events.instance.onPlayerTakeDamage(DamageHolder.instance.spikedSlime);
        }

        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier, ref health);
            if (health <= 0)
            {
                animator.SetTrigger("Death");
            }
        }else if (collision.CompareTag("HitBoxHeavy"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier, ref health);
            if (health <= 0)
            {
                animator.SetTrigger("death");
            }
        }
    }
}
