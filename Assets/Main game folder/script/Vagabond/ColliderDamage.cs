using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDamage : MonoBehaviour
{
    private float damage;
    private playerDeath playerDamage;
    void Start(){
        playerDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<playerDeath>();
        if (gameObject.name.CompareTo("HeavyAttackHitbox") == 0)
            damage = 35;
        else
            damage = 25;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player")) { 
            playerDamage.TakeDamage(damage);
        }
    }
}
