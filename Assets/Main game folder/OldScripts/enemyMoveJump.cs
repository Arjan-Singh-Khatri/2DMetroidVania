using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMoveJump : EnemyParentScript , IDataPersistance
{
    private Rigidbody2D rig;
    private float startPoint;
    private Vector2[] endPonints = new Vector2[2];
    [SerializeField] float speed = 5;
    private int wayPointIndex = 0;
    [SerializeField] private float jumpDistance = 4.5f;
    private float nextJumpTime = 2f;
    private bool _killed; 

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position.x;
        endPonints[0] = new Vector3(startPoint-5, transform.position.y);
        endPonints[1] = new Vector3(startPoint + 5, transform.position.y);
        player = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody2D>();
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

        Hop();
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

    private void Hop()
    {
        nextJumpTime -= Time.deltaTime;
        if(nextJumpTime <= 0) {
            nextJumpTime = Random.Range(3,6);
            rig.AddForce(Vector2.up * jumpDistance, ForceMode2D.Impulse);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplyer, ref health);
        }
        else if (collision.CompareTag("HeavyHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier, ref health);

        }

    }
}
