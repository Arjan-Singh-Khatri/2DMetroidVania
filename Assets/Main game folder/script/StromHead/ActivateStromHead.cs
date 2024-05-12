using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ActivateStromHead : EnemyParentScript , IDataPersistance
{
    [SerializeField] GameObject stromHead;
    bool _killed = false;


    private void Start(){
        StromHeadEvents.instance.onStromHeadKilled += TriggerKilled;
    }

    public void SaveData(ref GameData gameData)
    {
        if (gameData.bossesKilled.ContainsKey(this.enemyID))
        {
            gameData.bossesKilled.Remove(this.enemyID);
        }
        gameData.bossesKilled.Add(this.enemyID, _killed);
    }

    public void LoadData(GameData gameData)
    {
        gameData.bossesKilled.TryGetValue(this.enemyID, out _killed);
        if (_killed)
        {
            gameObject.SetActive(false);
        }
    }

    void ActivateEnemy() { 
        stromHead.SetActive(true);
    }

    void TriggerKilled() {
        _killed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player"))
            ActivateEnemy();
    }

    private void OnDisable()
    {
        StromHeadEvents.instance.onStromHeadKilled -= TriggerKilled;
    }
}