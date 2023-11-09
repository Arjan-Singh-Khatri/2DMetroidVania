using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHolder : MonoBehaviour
{
    public static DamageHolder instance;
    public float playerDamage = 7f;
    public float fireBallDamage = 15f;
    public float twoHeadDamage = 30f;
    public float twoHeadAttackTwoDamage = 40f;
    public float batDamage = 20f;
    public float slimeDamage = 15f;

    private void Start()
    {
        instance = this;    
        DontDestroyOnLoad(gameObject);
    }
}
