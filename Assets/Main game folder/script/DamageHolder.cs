using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHolder : MonoBehaviour
{
    public static DamageHolder instance;
    public float playerDamage = 7f;
    public float fireBallDamage = 15f;
    public float TwoHeadDamage = 30f;
    public float TwoHeadAttack2 = 40f;
    public float BatDamage = 20f;
    public float slimeDamage = 15f;

    private void Start()
    {
        instance = this;    
        DontDestroyOnLoad(gameObject);
    }
}
