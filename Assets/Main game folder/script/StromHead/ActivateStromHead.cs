using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ActivateStromHead : EnemyParentScript , IDataPersistance
{
    [SerializeField] GameObject stromHead;
    private float timer = 0f;
    bool _killed = false;
    bool activated = false;

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

    // Update is called once per frame
    void Update()
    {
        if (!activated) { 
            timer += Time.deltaTime;
            activated = true;
        }
        if(timer > 5f){
            stromHead.SetActive(true);
        }
        if (stromHead.GetComponent<StromHead>()._killed)
        {
            _killed = true;
            EnemyDeath();
        }
    }


}
