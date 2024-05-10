using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // FOR PLAYER
    public float _health;
    public Vector3 _position;
    public float damageMultiplyer;
    public float playerDamage;
    public int playerCharge;
    public float playerHeavyDamage;

    // FOR ENEMIES AND ITEMS
    public SerializeDictionaryForJson<string, bool> itemCollected;
    public SerializeDictionaryForJson<string, bool> bossesKilled;

    public GameData()
    {
        this._health = 100;
        this._position = new Vector3(9.77000046f, 15.96f, 0);
        this.playerCharge = 0;
        this.playerDamage = 15;
        this.damageMultiplyer = 1;
        this.playerHeavyDamage = 30; ;
        this.itemCollected = new SerializeDictionaryForJson<string, bool>();
        this.bossesKilled = new SerializeDictionaryForJson<string, bool>();
    }
}
