using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class ChaseBorder : MonoBehaviour
{
    private  BatEnemy childBatEnemy;

    private void Start()
    {
        childBatEnemy = GetComponentInChildren<BatEnemy>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            childBatEnemy.chase = true;
            childBatEnemy.return_tostart = false;
        }
        if (collision.CompareTag("enemy") && collision.CompareTag("Player"))
        {
            childBatEnemy.chase = true;
            childBatEnemy.return_tostart = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            childBatEnemy.return_tostart = true ;
            childBatEnemy.chase = false;
        }
        if (collision.CompareTag("enemy"))
        {
            
            childBatEnemy.return_tostart = true;
            childBatEnemy.chase = false;}
        }
    }

