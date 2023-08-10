using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireTrap : MonoBehaviour
{
    private bool FireOn;
    private playerDeath pl;
    private Animator amim;
    [SerializeField]
    private float speed;


    private void Start()
    {
        pl = FindObjectOfType<playerDeath>();
        amim  =GetComponent<Animator>();
    }

    void Update()
    {
        amim.speed = speed;
        if (FireOn && GetComponent<Collider2D>().IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>()))
        {
            pl.Die();
        }
    }

    private void fireOn()
    {
        FireOn = true;
    }

    private void fireOff()
    {
        FireOn = false;
    }

}
