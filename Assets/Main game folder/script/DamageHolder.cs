using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DamageHolder : MonoBehaviour
{
    public static DamageHolder instance;

    // NEED TO HAVE SAVE AND LOAD

    public float damageMultiplyer = 1 ;
    public float comboNumber = 0;

    public float damageMultiplier = 1 ; 
    public float playerDamage = 15f;
    public float playerHeavyDamage = 30f;

    public float fireBallDamage = 15f;
    public float twoHeadDamage = 30f;
    public float twoHeadAttackTwoDamage = 40f;
    public float batDamage = 20f;
    public float slimeDamage = 15f;
    public float crab = 10f;
    public float crabAttack = 20f;
    public float spikedSlime = 10f;
    public float spikedSlimeAttack = 15f;
    public float spikdSlimeSpikes = 20f;

    public float stromHead = 0;
    public float strom = 0;
    public float lightning = 0;

    private void Start()
    {
        instance = this;  
    }

    
    
}
