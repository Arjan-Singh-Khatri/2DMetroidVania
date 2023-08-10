using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.PackageManager.UI;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TwoHead : EnemyParentScript
{
    private Animator anim;
    [SerializeField] private float attackOneSpeed = 5;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private Transform[] wayPoints;

    public bool isHit = false;
    private int indexWayPoint = 0;
    private int healthOfTwoHead = 60;

    private float distanceToBeCovered;
    private float distanceCovered;
    [SerializeField]
    private bool chase = true;

    Vector3 scale;
    private AnimationClip[] animationClips;
    float timerForGettingHit;
    bool startTimerBool = false;

    float attackTwoTime = .5f;
    // Start is called before the first frame update
    private void Start()
    {
        anim=GetComponent<Animator>();
        distanceToBeCovered = Vector2.Distance(wayPoints[0].position, wayPoints[1].position);
        scale = transform.localScale;

        animationClips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips)
        {
            if (clip.name == "attack2")
            {
                attackTwoTime = clip.length;
            }


        }
    }

    private void Update()
    {
        if (healthOfTwoHead <= 0) return;
        if (chase)
        {
            anim.SetBool("attack1", true);
            distanceCovered = distanceToBeCovered - Vector2.Distance(transform.position, wayPoints[indexWayPoint].position);
            transform.position = Vector3.Lerp(transform.position, wayPoints[indexWayPoint].position, Time.deltaTime * (distanceCovered / distanceToBeCovered));
        }
        else
        {
            anim.SetBool("attack1", false);
        }
        if(Mathf.Abs(transform.position.x - wayPoints[indexWayPoint].position.x) < .5)
        {
            chase = false;
           
            scale.x *= -1;
            transform.localScale = scale;
            indexWayPoint++;
            if(indexWayPoint >=wayPoints.Length)
            {
                indexWayPoint = 0;
            }
            StartCoroutine(EnableChase());
        }
        if(startTimerBool)
        {
            StartTimer();
        }
    }

    private IEnumerator EnableChase()
    {
        yield return new WaitForSeconds(1.4f);
        chase = true;
    }

    //private void Attack1()
    //{
    //    anim.SetBool("attack1",true);
    //    distanceCovered = distanceToBeCovered - Vector2.Distance(transform.position, wayPoints[indexWayPoint].position);

    //    transform.position = Vector3.Lerp(transform.position, wayPoints[indexWayPoint].position, 0.1f*(distanceCovered/distanceToBeCovered));
    //}

    IEnumerator Attack2()
    {
        anim.SetTrigger("attack2");
        attackHitBox.SetActive(true);
        yield return new WaitForSeconds(attackTwoTime+1f);
        attackHitBox.SetActive(false);
        chase = true;
    }

    private void Hurt()
    {
        chase = false;
        anim.SetTrigger("Hurt");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (healthOfTwoHead <= 0) return;
        if (collision.CompareTag("PlayerAttackHitBox") && timerForGettingHit <= 0)
        {
            startTimerBool = true;
            PushBack(collision.gameObject);
            HealthDepleteEnemy(ref healthOfTwoHead);
            Hurt();
            if (healthOfTwoHead <= 0)
            {
                anim.SetTrigger("Death");
            }

        }
    }

    private void StartTimer()
    {
        timerForGettingHit = .7f;
        timerForGettingHit -= Time.deltaTime;
        if(timerForGettingHit <= 0)
        {
            startTimerBool = false;
        }
    }
}
