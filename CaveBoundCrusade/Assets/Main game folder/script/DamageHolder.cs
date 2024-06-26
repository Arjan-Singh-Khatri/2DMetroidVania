using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public  float fireBallDamage = 15f;
    public  float twoHeadDamage = 30f;
    public  float twoHeadAttackTwoDamage = 40f;
    public  float batDamage = 20f;
    public  float slimeDamage = 15f;
    public  float crab = 10f;
    public  float crabAttack = 20f;
    public  float spikedSlime = 10f;
    public  float spikedSlimeAttack = 15f;
    public  float spikdSlimeSpikes = 20f;

    // STROMHEAD
    public  float stromHead = 10;
    public  float strom = 100;
    public  float lightning = 20;

    //volume
    public float musicVol = 0;
    public float sfxVol = 0;
    #endregion

    #region Save And Load
    public void SaveData(ref GameData gameData) {
        gameData.playerDamage = this.playerDamage;
        gameData.playerHeavyDamage = this.playerHeavyDamage;
        gameData.playerCharge = this.playerCharge;   
        gameData.damageMultiplyer = this.damageMultiplier;
        gameData.musicVol = this.musicVol;
        gameData.sfxVol = this.sfxVol;
    }

    public void LoadData(GameData gameData) {
        this.damageMultiplier = gameData.damageMultiplyer;
        this.playerCharge = gameData.playerCharge;
        this.playerDamage = gameData.playerDamage;
        this.playerHeavyDamage = gameData.playerHeavyDamage;
        this.musicVol = gameData.musicVol;
        this.sfxVol = gameData.sfxVol;
    }
    #endregion

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
         fireBallDamage = 25f;
         twoHeadDamage = 40f;
         twoHeadAttackTwoDamage = 60f;
         batDamage = 30f;
         slimeDamage = 10f;
         crab = 20f;
         crabAttack = 35f;
         spikedSlime = 15f;
         spikedSlimeAttack = 35f;
         spikdSlimeSpikes = 20f;

            // STROMHEAD
         stromHead = 10;
         strom = 100;
         lightning = 20;
    }

}
