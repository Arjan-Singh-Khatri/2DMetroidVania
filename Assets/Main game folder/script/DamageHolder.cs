using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DamageHolder : MonoBehaviour , IDataPersistance
{
    public static DamageHolder instance;


    #region Player Attributes
    public float comboNumber = 0;
    public int playerCharge = 0;

    public float damageMultiplier = 1 ; 
    public float playerDamage = 10f;
    public float playerHeavyDamage = 30f;
    #endregion

    #region Enemy Damage
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

    #endregion

    #region Save And Load
    public void SaveData(ref GameData gameData) { 
        gameData.playerDamage = this.playerDamage;
        gameData.playerHeavyDamage = this.playerHeavyDamage;
        gameData.playerCharge = this.playerCharge;   
        gameData.damageMultiplyer = this.damageMultiplier;
    }

    public void LoadData(GameData gameData) {
        this.damageMultiplier = gameData.damageMultiplyer;
        this.playerCharge = gameData.playerCharge;
        this.playerDamage = gameData.playerDamage;
        this.playerHeavyDamage = gameData.playerHeavyDamage;
    }
    #endregion

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    
    
}
