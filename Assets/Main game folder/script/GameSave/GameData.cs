using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float _health;
    public Vector3 _position;
    public SerializeDictionaryForJson<string, bool> itemCollected;
    public SerializeDictionaryForJson<string, bool> enemyKilled;
    public SerializeDictionaryForJson<string, bool> bossesKilled;

    public GameData()
    {
        this._health = 100;
        this._position = new Vector3(9.77000046f, 15.96f, 0);
        // player combo 
        // player attack multiplyer
        this.itemCollected = new SerializeDictionaryForJson<string, bool>();
        this.bossesKilled = new SerializeDictionaryForJson<string, bool>();
    }
}
