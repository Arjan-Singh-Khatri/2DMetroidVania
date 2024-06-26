using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateVagabond : EnemyParentScript, IDataPersistance
{
    [SerializeField] GameObject vagaBond;
    bool _killed = false;


    private void Start(){
        VagabondEvents.instance.onBossDead += TriggerKilled;
    }

    public void SaveData(ref GameData gameData){
        if (gameData.bossesKilled.ContainsKey(this.enemyID))
        {
            gameData.bossesKilled.Remove(this.enemyID);
        }
        gameData.bossesKilled.Add(this.enemyID, _killed);
    }

    public void LoadData(GameData gameData){
        gameData.bossesKilled.TryGetValue(this.enemyID, out _killed);
        if (_killed)
        {
            gameObject.SetActive(false);
        }
    }

    void ActivateEnemy(){
        StartCoroutine(vagaBond.GetComponent<Vagabond>().Activation());
    }

    void TriggerKilled(){
        _killed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player"))
            ActivateEnemy();
    }

    private void OnDisable(){
        VagabondEvents.instance.onBossDead -= TriggerKilled;
    }
}
