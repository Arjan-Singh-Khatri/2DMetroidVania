using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StromHeadBoss : MonoBehaviour
{
    [SerializeField] GameObject StromHead;
    private void Active()
    {
        StromHead.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Active(); 
    }
}
