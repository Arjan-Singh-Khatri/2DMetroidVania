using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class ChaseBorder : MonoBehaviour
{
    public BatEnemy[] enemy_Array;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            foreach (BatEnemy Enemy in enemy_Array)
            {

                Enemy.chase = true;
                Enemy.return_tostart = false;

            }
        }
        if (collision.CompareTag("enemy") && collision.CompareTag("Player"))
        {
            foreach (BatEnemy Enemy in enemy_Array)
            {

                Enemy.chase = true;
                Enemy.return_tostart = false;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach(BatEnemy Enemy in enemy_Array)
            {
                    Enemy.return_tostart = true ;
                    Enemy.chase = false;
            }
        }
        if (collision.CompareTag("enemy"))
        {
            foreach (BatEnemy Enemy in enemy_Array)
            {
                Enemy.return_tostart = true;
                Enemy.chase = false;
            }
        }
    }

}
