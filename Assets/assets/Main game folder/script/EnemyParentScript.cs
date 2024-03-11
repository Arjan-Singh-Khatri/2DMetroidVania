using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentScript : MonoBehaviour
{
    protected GameObject player;
    protected float health;
    protected bool enemyDead;
    protected float direction;
    [SerializeField] protected string enemyID;

    [ContextMenu("GUID ID GENERATE")]
    private void GenerateGUID()
    {
        enemyID = System.Guid.NewGuid().ToString();
    }
    
    

    protected void HealthDepleteEnemy(float attackPower, ref float healthofEnemy)
    {
        healthofEnemy -= attackPower;
        
    }

    protected void PushBack()
    {
        if (player.transform.position.x <= transform.position.x)
            transform.position = transform.position + new Vector3(5, 0, 0);
        else 
            transform.position = transform.position + new Vector3(-5, 0, 0);
    }

    protected void flip()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            direction = -1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            direction = 1;  
        }

    }

    protected void EnemyDeath()
    {
        Destroy(gameObject);
    }
}