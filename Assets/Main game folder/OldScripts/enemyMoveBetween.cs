using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class enemyMoveBetween : EnemyParentScript, IDataPersistance
{
    
    [SerializeField] float speed = 5;
    private int wayPointIndex = 0;
    private float startPoint;
    private Vector2[] endPonints = new Vector2[2];
    private bool _killed;


    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position.x;
        endPonints[0] = new Vector3(startPoint - 5, transform.position.y);
        endPonints[1] = new Vector3(startPoint + 5, transform.position.y);
        player = GameObject.FindGameObjectWithTag("Player");
        health = 40f;
        wayPointIndex = Random.Range(0, 2);
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
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            _killed = true;
            EnemyDeath();
            return;
        }
        EnemyMove();
    }
    

    void EnemyMove()
    {
        if (Vector2.Distance(transform.position, endPonints[wayPointIndex]) > .5)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPonints[wayPointIndex], speed * Time.deltaTime);
            
        }
        else
        {
            wayPointIndex++;
            if (wayPointIndex >= endPonints.Length)
            {
                wayPointIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<playerDeath>().TakeDamage(DamageHolder.instance.slimeDamage);
        }

        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref health);

        }
        else if (collision.CompareTag("HeavyHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier, ref health);

        }

    }
}
