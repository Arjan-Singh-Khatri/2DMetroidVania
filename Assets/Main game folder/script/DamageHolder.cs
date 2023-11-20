using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHolder : MonoBehaviour
{
    public static DamageHolder instance;
    public float playerDamage = 15f;
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

    private void Start()
    {
        if(instance != null)
        {
            Destroy(instance);  
        }
        instance = this;    
        DontDestroyOnLoad(gameObject);
        
    }
}
