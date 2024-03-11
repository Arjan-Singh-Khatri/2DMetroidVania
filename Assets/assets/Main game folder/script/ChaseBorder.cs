using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class ChaseBorder : EnemyParentScript , IDataPersistance
{
    private BatEnemy childBatEnemy;
    private bool _killed;
    
    private void Start()
    {
        childBatEnemy = GetComponentInChildren<BatEnemy>();
    }

    #region Save And Load

    public void SaveData(ref GameData gameData)
    {
        if (gameData.enemyKilled.ContainsKey(this.enemyID))
        {
            gameData.enemyKilled.Remove(this.enemyID);
        }
        gameData.enemyKilled.Add(this.enemyID, _killed);
    }

    public void LoadData(GameData gameData)
    {
        gameData.enemyKilled.TryGetValue(this.enemyID, out _killed);
        if (_killed)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    private void Update()
    {
        if(childBatEnemy.killed) { _killed = true; }
    }

 
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            childBatEnemy.chase = true;
            childBatEnemy.return_tostart = false;
            
        }
        if (collision.CompareTag("enemy") && collision.CompareTag("Player"))
        {
            childBatEnemy.chase = true;
            childBatEnemy.return_tostart = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            childBatEnemy.return_tostart = true ;
            childBatEnemy.chase = false;
        }
        if (collision.CompareTag("enemy"))
        {
            
            childBatEnemy.return_tostart = true;
            childBatEnemy.chase = false;
        }
    }
}

