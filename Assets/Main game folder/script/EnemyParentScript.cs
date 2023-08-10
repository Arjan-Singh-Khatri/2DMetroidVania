using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void HealthDepleteEnemy(ref int healthOfSpecificEnemy)
    {
        healthOfSpecificEnemy -= 10;
    }

    protected void PushBack(GameObject player)
    {
        if (player.transform.position.x < transform.position.x)
            transform.position = transform.position - new Vector3(5, 0, 0);
        else 
            transform.position = transform.position - new Vector3(-5, 0, 0);
    }

    protected void flip(GameObject player)
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

    protected void EnemyDeath()
    {
        Destroy(gameObject);
    }
}
