using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StromHeadBoss : MonoBehaviour
{
    private void Active()
    {
        gameObject.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Active(); 
    }
}
