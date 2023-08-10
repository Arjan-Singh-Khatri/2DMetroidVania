using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.TextCore.Text;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StromHead : MonoBehaviour
{
    //private Transform transform;
    [SerializeField] private Transform[] waypoints;
    private int waypointIndex = 2;
    //[SerializeField]private float TeleportTime = 10f;
    private Animator anim;
    private float TimeInterval = 10f;
    private bool IsAttacking = false;





    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {

        BossMove();

    }

    private void BossMove  () 
    {
        
        if (!IsAttacking && Time.time >= TimeInterval) 
        {
            int random = waypointIndex;
            do
            {
                // Generate a new random number
                waypointIndex = Random.Range(0, waypoints.Length);
            }
            while (waypointIndex == random);
            transform.position = waypoints[waypointIndex].position;
            StartCoroutine(Attack());
            TimeInterval += Time.time; 
        }
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private IEnumerator Attack()
    {
        // Hit box getting activated and animation 
        anim.SetBool("Attack", true);
        IsAttacking=true;
        
        // HitBox part
        Collider2D Hit_box = GameObject.FindGameObjectWithTag("EnemyHitBox").GetComponent<Collider2D>();
        Hit_box.enabled = true;

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // Hit box and animation both getting disabled 

        anim.SetBool("Attack", false);
        IsAttacking = false;
        Hit_box.enabled = false;
    }


 
}
